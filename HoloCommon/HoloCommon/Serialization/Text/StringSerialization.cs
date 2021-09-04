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