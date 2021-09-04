using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace rab1
{
    public class RecodingHelper
    {
        // Индекс массива определяет значение интенсивности, с которого перекодировать
        // Период синусоиды 256 точек
        
        public static int[] CreateRecodingArray
        (
            int[] idealArray,
            string directoryPath,
            int rowIndex,
            int colIndex
        )
        {
            int count = 256;

            int[] resRecodingArray = new int[count];
            int[] originalIntensitiesArray = GetIntensitiesArrayFromImages(directoryPath, rowIndex, colIndex);
            
            for (int j = 0; j < originalIntensitiesArray.Length; j++)
            {
                int origIntensity = originalIntensitiesArray[j];
                int correctedIntensity = idealArray[origIntensity];
                resRecodingArray[origIntensity] = correctedIntensity;
            }

            return resRecodingArray;
        }

        public static int[] GetIntensitiesArrayFromImages(string directoryPath, int rowIndex, int colIndex)
        {
            int count = 256;
            int[] resArray = new int[count];
            
            string[] filePaths = Directory.GetFiles(directoryPath);

            IList<string> fileList = SortFiles(filePaths, count);

            for (int k = 0; k < count; k++)
            {
                string filePath = fileList[k];
                Bitmap img = new Bitmap(filePath);
                Color pixel = img.GetPixel(colIndex, rowIndex);
                resArray[k] = GetIntensity(pixel);
            }

            return resArray;
        }
        
        private static int GetIntensity(Color color)
        {
            return color.A;
        }

        private static IList<string> SortFiles(IList<string> list, int filesCount)
        {
            return
                list.Select(x => new { FilePath = x, SequenceNumber = ExtractSequenceNumberFromFilePath(x) })
                .OrderBy(x => x.SequenceNumber).Take(filesCount).Select(x => x.FilePath).ToList();
        }

        private static int ExtractSequenceNumberFromFilePath(string filePath)
        {
            string fielName = Path.GetFileNameWithoutExtension(filePath);
            return int.Parse(fielName);
        }
    }
}
