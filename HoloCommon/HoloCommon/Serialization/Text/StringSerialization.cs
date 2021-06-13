using System;
using System.Text;
using HoloCommon.MemoryManagement;
using HoloCommon.Models.Charting;

using HoloCommon.Interfaces;

namespace HoloCommon.Serialization.Text
{
    public class StringSerialization : IBinarySerialization<String>
    {
        public byte[] Serialize(String obj)
        {
            if (obj == null)
            {
                return new byte[0];
            }

            return Encoding.Unicode.GetBytes(obj);

            /*
            int count = Encoding.Unicode.GetByteCount(obj) + TypeSizes.SIZE_INT;
            byte[] resBytes = new byte[count];
            byte[] lengthBytes = BitConverter.GetBytes(obj.Length);
            byte[] objBytes = Encoding.Unicode.GetBytes(obj);

            Array.Copy(lengthBytes, 0, resBytes, 0, TypeSizes.SIZE_INT);
            Array.Copy(objBytes, 0, resBytes, TypeSizes.SIZE_INT, objBytes.Length);
            
            return resBytes;   
            */
        }
        public String Deserialize(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            return Encoding.Unicode.GetString(bytes);           
        }
    }
}