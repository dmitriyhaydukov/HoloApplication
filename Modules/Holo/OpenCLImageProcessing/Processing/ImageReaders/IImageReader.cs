using System.IO;
using Common;

namespace Processing.ImageReaders
{
    public interface IImageReader
    {
        string FormatFilter { get; }
        string FormatDescription { get; }

        ImageHandler Read(BinaryReader reader);
        ImageHandler Read(Stream stream);
        bool CanRead(BinaryReader reader);
        bool CanRead(Stream stream);
    }

    public abstract class ImageReaderBase : IImageReader
    {
        public virtual string FormatFilter => "";
        public virtual string FormatDescription => "";

        public abstract ImageHandler Read(BinaryReader reader);

        public virtual ImageHandler Read(Stream stream)
        {
            return Read(new BinaryReader(stream));
        }

        public abstract bool CanRead(BinaryReader reader);

        public bool CanRead(Stream stream)
        {
            return CanRead(new BinaryReader(stream));
        }
    }

    public static class ImageReaderExtensions
    {
        public static string GetFileFilter(this IImageReader self)
        {
            if (self.FormatFilter.IsNullOrEmpty())
                return "All files (*.*)|*.*";

            return $"{self.FormatDescription} ({self.FormatFilter})|{self.FormatFilter}";
        }
    }
}
