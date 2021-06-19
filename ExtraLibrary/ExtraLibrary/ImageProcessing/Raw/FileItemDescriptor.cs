using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraLibrary.ImageProcessing.Raw
{
    public class FileItemDescriptor
    {
        public long Offset { get; set; }
        public long Length { get; set; }
        public Type Type { get; set; }
        public string Description { get; set; }
    }
}
