using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using Cloo;
using Infrastructure;
using Processing.Computing;
using Processing.DataBinding;

namespace Processing.DataProcessors
{
    public abstract class SingleImageOutputDataProcessorBase : IDataProcessor, IImageHandler
    {
        protected bool Initialized;

        public event Action Updated;
        public event Action<ImageUpdatedEventData> ImageUpdated;
        public event Action<IImageHandler> OnImageFinalize;
        public event Action<IImageHandler> OnImageCreate;

        private ImageHandler _output;

        [Output("Output")]
        [SubfieldGroup]
        public ImageHandler Output
        {
            get => _output;
            set
            {
                if (_output != null)
                    _output.ImageUpdated -= OutputImageUpdated;

                _output = value;
                if (_output != null)
                    _output.ImageUpdated += OutputImageUpdated;
            }
        }

        public bool Ready => Output != null && Output.Ready;
        public IDictionary<string, object> Tags => Output?.Tags;
        public int Width => Output?.Width ?? throw new NullReferenceException();
        public int Height => Output?.Height ?? throw new NullReferenceException();
        public int PixelSizeInBytes => Output?.PixelSizeInBytes ?? throw new NullReferenceException();
        public ImagePixelFormat PixelFormat => Output?.PixelFormat ?? throw new NullReferenceException();
        public ImageFormat Format => Output?.Format ?? throw new NullReferenceException();
        public int? OpenGlTextureId => Output?.OpenGlTextureId;
        public ComputeImage2D ComputeBuffer => Output?.ComputeBuffer;

        // Для отладки
        public bool Removed
        {
            get => Output?.Removed ?? true;
            set
            {
                if (Output != null)
                    Output.Removed = value;
            }
        }

        public bool InOperationScope
        {
            get => Output?.InOperationScope ?? false;
            set
            {
                if (Output != null)
                    Output.InOperationScope = value;
            }}

        public void UploadToComputingDevice(bool forceUpdate = false)
        {
            Output?.UploadToComputingDevice(forceUpdate);
        }

        public void FreeComputingDevice()
        {
            Output?.FreeComputingDevice();
        }

        public void Update()
        {
            Output?.Update();
        }

        public void DownloadFromComputingDevice()
        {
            Output?.DownloadFromComputingDevice();
        }

        public Bitmap ToBitmap(int channel = 0)
        {
            return Output?.ToBitmap(channel);
        }

        public IImageHandler ExtractSelection(ImageSelection selection)
        {
            return Output?.ExtractSelection(selection);
        }

        public virtual void Initialize()
        {
            if (!Initialized)
                Singleton.Get<ImageHandlerRepository>().Add(this);

            Initialized = true;
        }

        public virtual void InputUpdated(PropertyInfo propertyInfo)
        {
        }
        protected virtual void OutputImageUpdated(ImageUpdatedEventData e)
        {
            ImageUpdated?.Invoke(e);
        }

        protected void OnImageUpdated(ImageUpdatedEventData e)
        {
            ImageUpdated?.Invoke(e);
        }
        public abstract void Awake();

        public abstract void FreeResources();

        protected void OnUpdated()
        {
            Updated?.Invoke();
        }

        public virtual void Dispose()
        {
            Singleton.Get<ImageHandlerRepository>().Remove(this);
        }

        [Action(TooltipText = "Сохранить")]
        public void SaveImage()
        {
            OnImageFinalize?.Invoke(Output);
        }

        protected void CreateImage(IImageHandler image)
        {
            OnImageCreate?.Invoke(image);
        }

        protected void CreateOrUpdateOutputWithSameParametres(IImageHandler image, string title, ImagePixelFormat? pixelFormat = null, ImageFormat? format = null)
        {
            CreateOrUpdateImageWithSameParametres(image, title, ref _output);
        }

        protected void CreateOrUpdateImageWithSameParametres(IImageHandler image, string title, ref ImageHandler imageToCheck, ImagePixelFormat? pixelFormat = null, ImageFormat? format = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (imageToCheck == null || !image.SizeEquals(imageToCheck))
            {
                imageToCheck?.FreeComputingDevice();

                imageToCheck = ImageHandler.Create(title, image.Width, image.Height, format ?? image.Format, pixelFormat ?? image.PixelFormat);
            }
        }

        protected ImageOperationScope StartOperationScope(params IImageHandler[] affectedImages)
        {
            return new ImageOperationScope(this, affectedImages);
        }

        protected class ImageOperationScope : IDisposable
        {
            private SingleImageOutputDataProcessorBase _processor;
            private OpenClApplication.SingleOperationContext _singleOperationContext;
            public ImageOperationScope(SingleImageOutputDataProcessorBase processor, params IImageHandler[] affectedImages)
            {
                foreach (var affectedImage in affectedImages)
                {
                    if (!affectedImage.OpenGlTextureId.HasValue)
                        affectedImage.UploadToComputingDevice();
                }

                _processor = processor;
                _singleOperationContext = new OpenClApplication.SingleOperationContext(affectedImages);
            }

            public void Dispose()
            {
                _singleOperationContext.Dispose();

                _processor.OnUpdated();
                _processor.OnImageUpdated(new ImageUpdatedEventData(false));
                _processor.Output.Update();
            }
        }
    }
}
