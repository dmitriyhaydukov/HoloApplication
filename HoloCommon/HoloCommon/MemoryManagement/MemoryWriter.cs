using HoloCommon.Interfaces;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoloCommon.MemoryManagement
{
    public class MemoryWriter : MemoryBaseProcessor
    {
        private delegate int WriteAction(BinaryWriter binaryWriter);

        private static int ExecuteWrite(WriteAction writeAction, long offset)
        {
            using (MemoryMappedFile mmf = GetMMF())
            {
                using (MemoryMappedViewStream viewStream = CreateViewStream(mmf, offset))
                {
                    using (BinaryWriter binWriter = new BinaryWriter(viewStream))
                    {
                        return writeAction(binWriter);
                    }
                }
            }
        }

        public static int Write<T>(T obj, IBinarySerialization<T> serialization, long offset = 0)
        {
            byte[] bytes = serialization.Serialize(obj);
            int length = bytes.Length;
                                    
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(length);
                binaryWriter.Write(bytes);

                return TypeSizes.SIZE_INT + length;
            };

            return ExecuteWrite(writeAction, offset);     
        }
        
        public static int WriteCollection<T>(IEnumerable<T> collection, IBinarySerialization<T> serialization, long offset = 0)
        {
            int count = collection.Count();
            int zc = Write(count, offset);
            long currentOffset = offset + zc;
            int totalSize = zc;

            foreach (T obj in collection)
            {
                int size = Write<T>(obj, serialization, currentOffset);
                currentOffset = currentOffset + size;
                totalSize = totalSize + size;
            }

            return totalSize;
        }  

        public static int Write(bool value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_BOOL;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(byte value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_BYTE;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(byte[] buffer, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(buffer);
                return TypeSizes.SIZE_BYTE * buffer.Length;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(byte[] buffer, int index, int count, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(buffer, index, count);
                return TypeSizes.SIZE_BYTE * count;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(char value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_CHAR;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(char[] chars, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(chars);
                return TypeSizes.SIZE_CHAR * chars.Length;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(char[] chars, int index, int count, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(chars, index, count);
                return TypeSizes.SIZE_CHAR * count;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(decimal value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_DECIMAL;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(double value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_DOUBLE;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(short value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_SHORT;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(int value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_INT;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(long value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_LONG;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(sbyte value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_SBYTE;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(float value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_FLOAT;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(string value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                int bytesCount = Encoding.Unicode.GetByteCount(value) + 1;
                return bytesCount;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(ushort value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_USHORT;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(uint value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_UINT;
            };

            return ExecuteWrite(writeAction, offset);
        }

        public static int Write(ulong value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
                return TypeSizes.SIZE_ULONG;
            };

            return ExecuteWrite(writeAction, offset);
        }
    }
}