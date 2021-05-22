using HoloCommon.Interfaces;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Collections.Generic;
using System.Linq;

namespace HoloCommon.MemoryManagement
{
    public class MemoryReader : MemoryBaseProcessor
    {
        private delegate T ReadAction<T>(BinaryReader binaryReadaer);

        private static T ExecuteRead<T>(ReadAction<T> readAction, long offset)
        {
            using (MemoryMappedFile mmf = GetMMF())
            {
                using (MemoryMappedViewStream viewStream = CreateViewStream(mmf, offset))
                {
                    using (BinaryReader binReader = new BinaryReader(viewStream))
                    {
                        return readAction(binReader);
                    }
                }
            }
        }

        public static T Read<T>(IBinarySerialization<T> serialization, long offset = 0)
        {
            ReadAction<T> readAction = (binaryReader) =>
            {
                T obj = default(T);
                int length = binaryReader.ReadInt32();
                byte[] bytes = binaryReader.ReadBytes(length);
                obj = serialization.Deserialize(bytes);
                return obj;
            };

            return ExecuteRead<T>(readAction, offset);          
        }

        public static IEnumerable<T> ReadCollection<T>(IBinarySerialization<T> serialization, long offset = 0)
        {
            List<T> collection = new List<T>();
            int count = ReadInt32(offset);

            T obj = default(T);
            int length = 0;
            byte[] bytes = null;

            ReadAction<T> readAction = (binaryReader) =>
            {
                obj = default(T);
                length = binaryReader.ReadInt32();
                bytes = binaryReader.ReadBytes(length);
                obj = serialization.Deserialize(bytes);
                return obj;
            };

            long currentOffset = offset + TypeSizes.SIZE_INT;
            for (int k = 0; k < count; k++)
            {
                ExecuteRead<T>(readAction, currentOffset);
                collection.Add(obj);
                currentOffset = currentOffset + TypeSizes.SIZE_INT + length;
            }

            return collection;
        }

        public static bool ReadBoolean(long offset = 0)
        {
            ReadAction<bool> readAction = (binaryReader) =>
            {
                return binaryReader.ReadBoolean();
            };

            return ExecuteRead<bool>(readAction, offset);
        }

        public static byte ReadByte(long offset = 0)
        {
            ReadAction<byte> readAction = (binaryReader) =>
            {
                return binaryReader.ReadByte();
            };

            return ExecuteRead<byte>(readAction, offset);
        }

        public static byte[] ReadBytes(int count, long offset = 0)
        {
            ReadAction<byte[]> readAction = (binaryReader) =>
            {
                return binaryReader.ReadBytes(count);
            };

            return ExecuteRead<byte[]>(readAction, offset);
        }

        public static char ReadChar(long offset = 0)
        {
            ReadAction<char> readAction = (binaryReader) =>
            {
                return binaryReader.ReadChar();
            };

            return ExecuteRead<char>(readAction, offset);
        }

        public static char[] ReadChars(int count, long offset = 0)
        {
            ReadAction<char[]> readAction = (binaryReader) =>
            {
                return binaryReader.ReadChars(count);
            };

            return ExecuteRead<char[]>(readAction, offset);
        }

        public static decimal ReadDecimal(long offset = 0)
        {
            ReadAction<decimal> readAction = (binaryReader) =>
            {
                return binaryReader.ReadDecimal();
            };

            return ExecuteRead<decimal>(readAction, offset);
        }

        public static double ReadDouble(long offset = 0)
        {
            ReadAction<double> readAction = (binaryReader) =>
            {
                return binaryReader.ReadDouble();
            };

            return ExecuteRead<double>(readAction, offset);
        }

        public static short ReadInt16(long offset = 0)
        {
            ReadAction<short> readAction = (binaryReader) =>
            {
                return binaryReader.ReadInt16();
            };

            return ExecuteRead<short>(readAction, offset);
        }

        public static int ReadInt32(long offset = 0)
        {
            ReadAction<int> readAction = (binaryReader) =>
            {
                return binaryReader.ReadInt32();
            };

            return ExecuteRead<int>(readAction, offset);
        }

        public static long ReadInt64(long offset = 0)
        {
            ReadAction<long> readAction = (binaryReader) =>
            {
                return binaryReader.ReadInt64();
            };

            return ExecuteRead<long>(readAction, offset);
        }

        public static sbyte ReadSByte(long offset = 0)
        {
            ReadAction<sbyte> readAction = (binaryReader) =>
            {
                return binaryReader.ReadSByte();
            };

            return ExecuteRead<sbyte>(readAction, offset);
        }

        public static float ReadSingle(long offset = 0)
        {
            ReadAction<float> readAction = (binaryReader) =>
            {
                return binaryReader.ReadSingle();
            };

            return ExecuteRead<float>(readAction, offset);
        }

        public static string ReadString(long offset = 0)
        {
            ReadAction<string> readAction = (binaryReader) =>
            {
                return binaryReader.ReadString();
            };

            return ExecuteRead<string>(readAction, offset);
        }

        public static ushort ReadUInt16(long offset = 0)
        {
            ReadAction<ushort> readAction = (binaryReader) =>
            {
                return binaryReader.ReadUInt16();
            };

            return ExecuteRead<ushort>(readAction, offset);
        }

        public static uint ReadUInt32(long offset = 0)
        {
            ReadAction<uint> readAction = (binaryReader) =>
            {
                return binaryReader.ReadUInt32();
            };

            return ExecuteRead<uint>(readAction, offset);
        }

        public static ulong ReadUInt64(long offset = 0)
        {
            ReadAction<ulong> readAction = (binaryReader) =>
            {
                return binaryReader.ReadUInt64();
            };

            return ExecuteRead<ulong>(readAction, offset);
        }
    }
}