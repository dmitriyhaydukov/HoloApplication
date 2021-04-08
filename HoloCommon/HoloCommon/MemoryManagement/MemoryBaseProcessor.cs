using System.IO.MemoryMappedFiles;

namespace HoloCommon.MemoryManagement
{
    public class MemoryBaseProcessor
    {
        public static string MMF_NAME = "HoloMemory";
        public static long MMF_SIZE = 1024 * 1024 * 1024;

        public static MemoryMappedFile CreateMMF()
        {
            return MemoryMappedFile.CreateNew(MMF_NAME, MMF_SIZE);
        }

        public static MemoryMappedFile GetMMF()
        {
            return MemoryMappedFile.OpenExisting(MMF_NAME);
        }

        public static MemoryMappedViewStream CreateViewStream(MemoryMappedFile mmf, long offset = 0)
        {
            return mmf.CreateViewStream(offset, 0);
        }
    }
}