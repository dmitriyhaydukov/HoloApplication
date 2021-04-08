using HoloCommon.Interfaces;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace HoloCommon.MemoryManagement
{
    public class MemoryWriter : MemoryBaseProcessor
    {
        private delegate void WriteAction(BinaryWriter binaryWriter);

        private static void ExecuteWrite(WriteAction writeAction, long offset)
        {
            using (MemoryMappedFile mmf = GetMMF())
            {
                using (MemoryMappedViewStream viewStream = CreateViewStream(mmf, offset))
                {
                    using (BinaryWriter binWriter = new BinaryWriter(viewStream))
                    {
                        writeAction(binWriter);
                    }
                }
            }
        }

        public static void Write<T>(T obj, IBinarySerialization<T> serialization, long offset = 0)
        {
            byte[] bytes = serialization.Serialize(obj);
            int length = bytes.Length;

            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(length);
                binaryWriter.Write(bytes);
            };

            ExecuteWrite(writeAction, offset);          
        }

        public static void Write(bool value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(byte value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(byte[] buffer, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(buffer);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(byte[] buffer, int index, int count, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(buffer, index, count);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(char value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(char[] chars, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(chars);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(char[] chars, int index, int count, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(chars, index, count);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(decimal value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(double value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(short value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }
        public static void Write(int value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(long value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(sbyte value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(float value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(string value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(ushort value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(uint value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }

        public static void Write(ulong value, long offset = 0)
        {
            WriteAction writeAction = (binaryWriter) =>
            {
                binaryWriter.Write(value);
            };

            ExecuteWrite(writeAction, offset);
        }
    }
}