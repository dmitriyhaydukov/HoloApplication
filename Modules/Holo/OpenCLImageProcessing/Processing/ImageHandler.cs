using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using OpenTK.Graphics.OpenGL;
using Processing.Computing;
using Processing.Utils;

namespace Processing
{
    public enum ImagePixelFormat
    {
        [SizeInBytes(sizeof(byte))] Byte = 32,
        [SizeInBytes(sizeof(float))] Float = 64
    }

    public enum ImageFormat
    {
        [ChannelsCount(1)] Greyscale = 1,
        [ChannelsCount(3)] RGB = 2,
        [ChannelsCount(4)] RGBA = 4,
        [ChannelsCount(2)] AmplitudePhase = 8,
        [ChannelsCount(2)] RealImaginative = 16
    }

    public class ImageUpdatedEventData
    {
        public bool ReloadControls { get; private set; }

        public ImageUpdatedEventData(bool reloadControls)
        {
            ReloadControls = reloadControls;
        }
    }

    public interface IImageHandler
    {
        bool Ready { get; }
        IDictionary<string, object> Tags { get; }
        event Action<ImageUpdatedEventData> ImageUpdated;
        int Width { get; }
        int Height { get; }
        int PixelSizeInBytes { get; }
        ImagePixelFormat PixelFormat { get; }
        ImageFormat Format { get; }
        int? OpenGlTextureId { get; }
        ComputeImage2D ComputeBuffer { get; }
        void UploadToComputingDevice(bool forceUpdate = false);
        void FreeComputingDevice();
        void Update();
        void DownloadFromComputingDevice();
        Bitmap ToBitmap(int channel = 0);
        IImageHandler ExtractSelection(ImageSelection selection);

        // Для отладки
        bool Removed { get; set; }
        bool InOperationScope { get; set; }
    }

    public class ImageHandler : IImageHandler, IDisposable
    {
        protected Dictionary<string, object> _tags { get; set; } = new Dictionary<string, object>();

        protected byte[] Data;

        public bool Ready => true;

        public event Action<ImageUpdatedEventData> ImageUpdated;
        public IDictionary<string, object> Tags => _tags;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ImagePixelFormat PixelFormat { get; private set; }
        public ImageFormat Format { get; private set; }
        public int? OpenGlTextureId { get; private set; }
        public int PixelSizeInBytes => PixelFormat.GetAttribute<SizeInBytesAttribute>().Value * Format.GetAttribute<ChannelsCountAttribute>().Value;
        public ComputeImage2D ComputeBuffer { get; private set; }

        // Для отладки
        public bool Removed { get; set; }
        public bool InOperationScope { get; set; }

        protected ImageHandler()
        {
        }

        public void UploadToComputingDevice(bool forceUpdate = false)
        {
            if (OpenGlTextureId.HasValue && !forceUpdate)
                return;
            
            var textureId = OpenGlTextureId ?? GL.GenTexture();
            OpenGlErrorThrower.ThrowIfAny();

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            OpenGlErrorThrower.ThrowIfAny();

            PixelInternalFormat pixelInternalFormat;
            OpenTK.Graphics.OpenGL.PixelFormat pixelFormat;
            PixelType pixelType;

            // TODO: Нужно создавать текстуру заново при изменении размера изображения!

            unsafe
            {
                fixed (byte* p = Data)
                {
                    if (PixelFormat == ImagePixelFormat.Byte)
                        pixelType = PixelType.UnsignedByte;
                    else if (PixelFormat == ImagePixelFormat.Float)
                        pixelType = PixelType.Float;
                    else
                        throw new NotSupportedException();

                    switch ((uint) Format)
                    {
                        case (uint) ImageFormat.Greyscale:
                            pixelInternalFormat = PixelFormat == ImagePixelFormat.Byte ? PixelInternalFormat.R8 : PixelInternalFormat.R32f;
                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Red;
                            break;
                        case (uint) ImageFormat.AmplitudePhase:
                        case (uint) ImageFormat.RealImaginative:
                            pixelInternalFormat = PixelFormat == ImagePixelFormat.Byte ? PixelInternalFormat.Rg8 : PixelInternalFormat.Rg32f;
                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rg;
                            break;
                        case (uint) ImageFormat.RGB:
                            pixelInternalFormat = PixelFormat == ImagePixelFormat.Byte ? PixelInternalFormat.Rgb8 : PixelInternalFormat.Rgb32f;
                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
                            break;
                        case (uint) ImageFormat.RGBA:
                            pixelInternalFormat = PixelFormat == ImagePixelFormat.Byte ? PixelInternalFormat.Rgba8 : PixelInternalFormat.Rgba32f;
                            pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
                            break;
                        default:
                            throw new NotSupportedException($"Неподдерживаемая комбинация {nameof(PixelFormat)} = {PixelFormat}, {nameof(Format)} = {Format}.");
                    }
                }
            }

            if (OpenGlTextureId.HasValue)
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, pixelFormat, pixelType, Data);
            else
                GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, Width, Height, 0, pixelFormat, pixelType, Data);

            OpenGlErrorThrower.ThrowIfAny();

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new[] {(int) TextureMinFilter.Nearest});
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new[] {(int) TextureMagFilter.Nearest});

            OpenGlErrorThrower.ThrowIfAny();
            OpenGlTextureId = textureId;

            var computeContext = Singleton.Get<OpenClApplication>().ComputeContext;
            ComputeBuffer = ComputeImage2D.CreateFromGLTexture2D(computeContext, ComputeMemoryFlags.ReadWrite, (int) TextureTarget.Texture2D, 0, textureId);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void DownloadFromComputingDevice()
        {
            if (PixelFormat == ImagePixelFormat.Byte)
                throw new NotImplementedException();

            var app = Singleton.Get<OpenClApplication>();
            var channelCount = this.GetChannelsCount();
            Data = new byte[sizeof(float) * Width * Height * channelCount];

            var kernelName = "copyImageToBuffer";
            if (channelCount > 1)
                kernelName = kernelName + "2";

            app.Acquire(this);

            unsafe
            {
                fixed (byte* dp = Data)
                {
                    int bufferSize = Width * Height * channelCount;
                    var computeBuffer = new ComputeBuffer<float>(app.ComputeContext, ComputeMemoryFlags.None, bufferSize);
                    
                    app.ExecuteKernel(kernelName, Width, Height, this, computeBuffer);

                    app.Queue.Read(computeBuffer, true, 0, bufferSize, (IntPtr) dp, null);
                }
            }

            app.Release(this);
        }

        public IImageHandler ExtractSelection(ImageSelection selection)
        {
            throw new NotImplementedException();
        }

        public void FreeComputingDevice()
        {
            try
            {
                ComputeBuffer?.Dispose();
            }
            catch (Exception ex)
            {
                DebugLogger.Warning(ex);
            }

            if (OpenGlTextureId.HasValue)
                GL.DeleteTexture(OpenGlTextureId.Value);
        }

        public void Update()
        {
            ImageUtils.PopulateMinMax(this);
            _tags[ImageHandlerTagKeys.Thumbnail] = ThumbnailGenerator.Generate(this);
            ImageUpdated?.Invoke(new ImageUpdatedEventData(false));
        }

        public void UpdateFromBitmap(Bitmap bmp)
        {
            if (Format != ImageFormat.Greyscale && PixelFormat != ImagePixelFormat.Byte)
                throw new InvalidOperationException($"{nameof(ImageHandler)} имеет недопустимый формат пикселей: {PixelFormat}");

            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                throw new InvalidOperationException($"Неподдерживаемый формат изображения [{bmp.PixelFormat}]. Поддерживаются только форматы {nameof(System.Drawing.Imaging.PixelFormat.Format24bppRgb)} и {nameof(System.Drawing.Imaging.PixelFormat.Format32bppArgb)}.");

            if (Format == ImageFormat.Greyscale)
                Data = ImageUtils.BitmapAsGreyscaleToByteArray(bmp, out _, out _);
            else
            {
                Data = ImageUtils.BitmapToByteArray(bmp);

                Format = bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb
                    ? ImageFormat.RGB
                    : ImageFormat.RGBA;
            }

            Width = bmp.Width;
            Height = bmp.Height;
            
            _tags.SetOrAdd(ImageHandlerTagKeys.Thumbnail, ThumbnailGenerator.Generate(bmp));

            if (OpenGlTextureId.HasValue)
                UploadToComputingDevice(true);

            Update();
        }

        public Bitmap ToBitmap(int channel = 0)
        {
            if (PixelFormat == ImagePixelFormat.Byte)
                throw new NotImplementedException();

            var imageHandlerChannelsCount = this.GetChannelsCount();
            if (channel >= imageHandlerChannelsCount)
                throw new InvalidOperationException();

            var bitmap = new Bitmap(Width, Height);
            const int bytesPerPixel = 3;
            var bdata = bitmap.LockBits(Rectangle.FromLTRB(0, 0, Width, Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // TODO: дубликация с ImageUtils.BitmapFromArray
            unsafe
            {
                fixed (byte* dp = Data)
                {
                    var fp = (float*) dp;
                    var ip = (byte*) bdata.Scan0;
                    int length = Data.Length / (sizeof(float) * imageHandlerChannelsCount);

                    var min = fp[0];
                    var max = fp[0];
                    float cur;

                    for (var i = 0; i < length; i++)
                    {
                        cur = fp[channel];

                        if (min > cur)
                            min = cur;
                        if (max < cur)
                            max = cur;

                        fp += imageHandlerChannelsCount;
                    }

                    var k = 255 / (max - min);
                    byte val = 0;
                    fp = (float*)dp;

                    for (var i = 0; i < length; i++)
                    {
                        cur = fp[channel];

                        if (cur > 0)
                            val = 0;

                        val = (byte)((cur - min) * k);
                        ip[0] = val;
                        ip[1] = val;
                        ip[2] = val;
                        ip += bytesPerPixel;
                        fp += imageHandlerChannelsCount;
                    }
                }
            }

            bitmap.UnlockBits(bdata);
            return bitmap;
        }
        
        public static ImageHandler FromBitmap(Bitmap bmp, string title = null)
        {
            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                throw new InvalidOperationException($"Неподдерживаемый формат изображения [{bmp.PixelFormat}]. Поддерживаются только форматы {nameof(System.Drawing.Imaging.PixelFormat.Format24bppRgb)} и {nameof(System.Drawing.Imaging.PixelFormat.Format32bppArgb)}.");

            var imageHandler = new ImageHandler
            {
                Data = ImageUtils.BitmapToByteArray(bmp),
                PixelFormat = ImagePixelFormat.Byte,
                Format = bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb
                    ? ImageFormat.RGB
                    : ImageFormat.RGBA,
                Width = bmp.Width,
                Height = bmp.Height
            };

            Singleton.Get<ImageHandlerRepository>().Add(imageHandler);

            if (title == null)
                title = "Bitmap " + Singleton.Get<ImageHandlerRepository>().GetAll().Count;

            imageHandler._tags.Add(ImageHandlerTagKeys.Title, title);
            imageHandler._tags.Add(ImageHandlerTagKeys.Thumbnail, Utils.ThumbnailGenerator.Generate(bmp));
            imageHandler.Update();

            return imageHandler;
        }

        public static ImageHandler Create(string title, int width, int height, ImageFormat imageFormat, ImagePixelFormat imagePixelFormat)
        {
            var imageHandler = new ImageHandler();
            imageHandler.Data = new byte[width * height * imageFormat.GetAttribute<ChannelsCountAttribute>().Value * imagePixelFormat.GetAttribute<SizeInBytesAttribute>().Value];
            imageHandler.Width = width;
            imageHandler.Height = height;
            imageHandler.PixelFormat = imagePixelFormat;
            imageHandler.Format = imageFormat;
            imageHandler._tags.Add(ImageHandlerTagKeys.Title, title);
            imageHandler._tags.Add(ImageHandlerTagKeys.Thumbnail, Utils.ThumbnailGenerator.Generate());
            imageHandler.Update();

            return imageHandler;
        }

        public static ImageHandler FromBitmapAsGreyscale(Bitmap bmp, string title = null)
        {
            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb && bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                throw new InvalidOperationException($"Неподдерживаемый формат изображения [{bmp.PixelFormat}]. Поддерживаются только форматы {nameof(System.Drawing.Imaging.PixelFormat.Format24bppRgb)},{nameof(System.Drawing.Imaging.PixelFormat.Format32bppRgb)} и {nameof(System.Drawing.Imaging.PixelFormat.Format32bppArgb)}.");

            var imageHandler = new ImageHandler
            {
                Data = ImageUtils.BitmapAsGreyscaleToByteArray(bmp, out float min, out float max),
                PixelFormat = ImagePixelFormat.Float,
                Format = ImageFormat.Greyscale,
                Width = bmp.Width,
                Height = bmp.Height
            };
            
            Singleton.Get<ImageHandlerRepository>().Add(imageHandler);

            if (title == null)
                title = "Bitmap " + Singleton.Get<ImageHandlerRepository>().GetAll().Count;

            imageHandler._tags.Add(ImageHandlerTagKeys.MaximumValueF, max);
            imageHandler._tags.Add(ImageHandlerTagKeys.MinimumValueF, min);
            imageHandler._tags.Add(ImageHandlerTagKeys.Title, title);
            imageHandler._tags.Add(ImageHandlerTagKeys.Thumbnail, ThumbnailGenerator.Generate(bmp));
            imageHandler.Update();

            return imageHandler;
        }
        
        public void Dispose()
        {
            FreeComputingDevice();
        }
    }

    internal class SizeInBytesAttribute : Attribute
    {
        public int Value { get; }

        public SizeInBytesAttribute(int value)
        {
            Value = value;
        }
    }

    internal class ChannelsCountAttribute : Attribute
    {
        public int Value { get; set; }

        public ChannelsCountAttribute(int value)
        {
            Value = value;
        }
    }

    public static class ImageHandlerExtensions
    {
        public static float GetRatio(this IImageHandler self)
        {
            return (float) self.Width / self.Height;
        }

        public static int GetChannelsCount(this IImageHandler self)
        {
            return self.Format.GetAttribute<ChannelsCountAttribute>().Value;
        }

        public static bool SizeEquals(this IImageHandler self, IImageHandler other)
        {
            return self.Width == other.Width && self.Height == other.Height;
        }

        public static FloatRange GetValueRangeForChannel(this IImageHandler self, int channel)
        {
            if (channel == 0)
                return new FloatRange((float)self.Tags[ImageHandlerTagKeys.MinimumValueF], (float)self.Tags[ImageHandlerTagKeys.MaximumValueF]);
            if (channel == 1)
                return new FloatRange((float) self.Tags[ImageHandlerTagKeys.MinimumValue2F], (float) self.Tags[ImageHandlerTagKeys.MaximumValue2F]);
            throw new NotImplementedException();
        }

        public static bool IsReady(this IImageHandler self)
        {
            return self != null && self.Ready;
        }

        public static string GetTitle(this IImageHandler self)
        {
            self.Tags.TryGetValue(ImageHandlerTagKeys.Title, out object title);
            return (string) title ?? "Image";
        }
    }

    public static class ImageHandlerTagKeys
    {
        public const string Title = "title";
        public const string Source = "source";
        public const string FileSystemPath = "fs_path";
        public const string Thumbnail = "thumbnail";
        public const string MinimumValueF = "minimum_value_float";
        public const string MaximumValueF = "maximum_value_float";
        public const string MinimumValue2F = "minimum_value_2_float";
        public const string MaximumValue2F = "maximum_value_2_float";
    }
}