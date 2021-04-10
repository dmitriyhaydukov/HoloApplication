using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace ImageReader.Imaging
{
    public class WriteableBitmapCreator {
        
        //------------------------------------------------------------------------------------------------
        //Создание изображения из файла
        public static WriteableBitmap CreateWriteableBitmapFromFile( string fileName ) {
            BitmapImage bitmapImage = BitmapImageCreator.CreateBitmapImageFromFile( fileName );
            WriteableBitmap writeableBitmap = new WriteableBitmap( bitmapImage );
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //Создание изображения
        public static WriteableBitmap CreateWriteableBitmap(
            int pixelWidth, int pixelHeight,
            int dpiX, int dpiY,
            PixelFormat pixelFormat
        ) {
            WriteableBitmap writeableBitmap = new WriteableBitmap
                ( pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat, null );
            return writeableBitmap;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------  
        //------------------------------------------------------------------------------------------------
    }
}
