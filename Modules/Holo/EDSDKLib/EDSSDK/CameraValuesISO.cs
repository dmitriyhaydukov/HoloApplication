using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDSDKLib.EDSSDK
{
    public class CameraValuesISO
    {
        public static Dictionary<UInt32, String> GetDictionary()
        {
            Dictionary<UInt32, String> isoDictionary = new Dictionary<UInt32, String>();
            
            isoDictionary.Add(0x00000000, "Auto ISO");
            isoDictionary.Add(0x00000028, "ISO 6");
            isoDictionary.Add(0x00000030, "ISO 12");
            isoDictionary.Add(0x00000038, "ISO 25");
            isoDictionary.Add(0x00000040, "ISO 50");
            isoDictionary.Add(0x00000048, "ISO 100");
            isoDictionary.Add(0x0000004b, "ISO 125");
            isoDictionary.Add(0x0000004d, "ISO 160");
            isoDictionary.Add(0x00000050, "ISO 200");
            isoDictionary.Add(0x00000053, "ISO 250");
            isoDictionary.Add(0x00000055, "ISO 320");
            isoDictionary.Add(0x00000058, "ISO 400");
            isoDictionary.Add(0x0000005b, "ISO 500");
            isoDictionary.Add(0x0000005d, "ISO 640");
            isoDictionary.Add(0x00000060, "ISO 800");
            isoDictionary.Add(0x00000063, "ISO 1000");
            isoDictionary.Add(0x00000065, "ISO 1250");
            isoDictionary.Add(0x00000068, "ISO 1600");
            isoDictionary.Add(0x00000070, "ISO 3200");
            isoDictionary.Add(0x00000078, "ISO 6400");
            isoDictionary.Add(0x00000080, "ISO 12800");
            isoDictionary.Add(0x00000088, "ISO 25600");
            isoDictionary.Add(0x00000090, "ISO 51200");
            isoDictionary.Add(0x00000098, "ISO 102400");
            
            isoDictionary.Add(0xffffffff, "N/A");

            return isoDictionary;
        }
    }
}
