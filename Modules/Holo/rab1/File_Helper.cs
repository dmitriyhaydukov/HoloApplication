using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;
//using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using ClassLibrary;

namespace rab1
{
    class File_Helper
    {


        public static ZArrayDescriptor loadImage()
        {
            OpenFileDialog dialog1 = new OpenFileDialog();
            dialog1.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
            dialog1.FilterIndex = 1;
            dialog1.RestoreDirectory = true;
          
           
            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    dialog1.InitialDirectory = dialog1.FileName;

                    Image newImage = Image.FromFile(dialog1.FileName);
                    ZArrayDescriptor z_array = new ZArrayDescriptor(newImage.Width, newImage.Height);
                    z_array = Util_array.getArrayFromImage(newImage);

                    return z_array;
                    
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("class File_Helper Ошибка при чтении изображения" + ex.Message); return null; }
            }
            return null;
        }

        public static ZArrayDescriptor loadImage_from_File(string str)
        {
            
                try
                {
               
                Image newImage = Image.FromFile(str);
                ZArrayDescriptor z_array = new ZArrayDescriptor(newImage.Width, newImage.Height);
                z_array = Util_array.getArrayFromImage(newImage);

                return z_array;
                
            }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("class File_Helper loadImage_from_File Ошибка при чтении изображения" + ex.Message); return null; }
            
           
        }

        public static void saveImage(PictureBox pictureBox01)
        {
            SaveFileDialog dialog1 = new SaveFileDialog();
            //dialog1.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp";
            dialog1.Filter = "Images (*.JPG)|*.JPG|" + "Images (*.BMP)|*.BMP|" + "All files (*.*)|*.*";
            dialog1.FilterIndex = 1;
            dialog1.RestoreDirectory = true;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox01.Image.Save(dialog1.FileName);
                    dialog1.InitialDirectory = dialog1.FileName;
                    //string_dialog = dialog1.FileName;

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(" Ошибка при записи файла " + ex.Message);
                }
            }

        }
        //------------------------------------------------------------------------------------------------  ZArrayDescriptor 
        public static void saveZArray(ZArrayDescriptor arrayDescriptor)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    if ((stream = saveFileDialog.OpenFile()) != null)
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        serializer.Serialize(stream, arrayDescriptor);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка при записи файла double : " + ex.Message);
                }
            }
        }

        public static ZArrayDescriptor loadZArray()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            BinaryFormatter deserializer = new BinaryFormatter();
                            ZArrayDescriptor savedArray = (ZArrayDescriptor)deserializer.Deserialize(myStream);
                            myStream.Close();
                            return savedArray;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка при чтении файла double  :" + ex.Message);
                    return null;
                }
            }

            return null;
        }

// --------------------------------------------------------------------------------------------------------------------------
        public static void saveZComplex(ZComplexDescriptor arrayDescriptor)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "complex files (*.zcmp)|*.zcmp|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream;
                    if ((stream = saveFileDialog.OpenFile()) != null)
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        serializer.Serialize(stream, arrayDescriptor);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка при записи файла complex : " + ex.Message);
                }
            }
        }

        public static ZComplexDescriptor loadZComplex()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "array files (*.zcmp)|*.zcmp|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            BinaryFormatter deserializer = new BinaryFormatter();
                            ZComplexDescriptor savedArray = (ZComplexDescriptor)deserializer.Deserialize(myStream);
                            myStream.Close();
                            return savedArray;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка при чтении файла double  :" + ex.Message);
                    return null;
                }
            }

            return null;
        }

        //---------------------------------------------------------------------------------------------------------- END

    }
}
