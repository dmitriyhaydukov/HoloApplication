using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Reflection;
using System.Diagnostics;
using rab1.Forms;
using ClassLibrary;
using System.Numerics;
using System.IO;

namespace rab1
{
    public delegate void FunctionPointer(object sender, EventArgs eventArgs);

    public partial class Form1 : Form
    {

        // -----------------------------------------------------------------------------------------------------------
        Image[] img = new Image[12];
        public static ZArrayDescriptor[] zArrayDescriptor = new ZArrayDescriptor[12];     // Иконки справа
        public static ZArrayDescriptor zArrayPicture = new ZArrayDescriptor();            // Массив для главного окна
        public static ZArrayDescriptor zArrayPictureOriginal = new ZArrayDescriptor();
        public PictureBox[] pictureBoxArray = null;
        public static ZComplexDescriptor[] zComplex = new ZComplexDescriptor[3];

        public static int regImage = 0;                           // Номер изображения (0-11)
        public static int regComplex = 0;                         // Номер Complex (0-3)

        int scaleMode = 0;                          // Масштаб изображения

        public static int X1, Y1, X2, Y2, X3, Y3, X4, Y4;        // Глобальные точки задания координат

        Form f_filt;                               // Для Фильтрации
        TextBox tb1_filt; //, tb2_filt, tb3_filt;
        int k_filt = 1;

        string string_dialog; // = "D:\\Студенты\\Эксперимент";       




        int cursorMode = 0;
        Point downPoint;



        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);
        // private delegate void StringParameterDelegate(List<Point3D> newList);

        //CustomPictureBox firsPictureBox;
        //CustomPictureBox secondPictureBox;

        //int batchProcessingFlag = 0;

        //private double currentScaleRatio = 1;
        private double initialScaleRatio = 1;
        private double afterRemovingScaleRatio = 1;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                
        GraphFormHost phaseDifferenceGraphFormHost = null;
        Form phaseDifferenceAltGraphicForm = null;

        GraphFormHost curvesGraphFormHost = null;
        Form curvesAltGraphicForm = null;

        CurvesGraph curvesGraph = null;
        Pain_t_Core core = null;

        public Form1()
        {
            InitializeComponent();

            //ShooterSingleton.init();

            this.pictureBoxArray = new PictureBox[]
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12
            };

            ADD_Math.VisualRegImage = this.Vizual_regImage;
            PSI.VisualRegImage = this.Vizual_regImage;

            Form_Filtr.VisualRegImage = this.Vizual_regImage;
            Model_Sin.VisualRegImage = this.Vizual_regImage;
            Model_Sin.VisualRegImageAsRaw = this.Vizual_regImageAsRaw;

            FrenelForm.Complex_pictureBox = this.Complex_pictureBox;
            FrenelForm.VisualRegImage = this.Vizual_regImage;

            ADD_Math.ComplexPictureImage = this.Complex_pictureBox;

            CorrectBr.VisualRegImage = this.Vizual_regImage;
            CorrectBr.TakePhoto12 = this.FastTakePhoto;
            CorrectBr.TakePhoto = this.FastTakePhoto;
            //ADD_Cmplx.VisualRegImage = this.Vizual_regImage;

            Corr256.VisualRegImage = this.Vizual_regImage;

            Teorema1.VisualComplex = this.Complex_pictureBox;
            Teorema1.VisualArray   = this.Vizual_Picture_Array;

            Super.VisualComplex = this.Complex_pictureBox;
            Super.VisualArray = this.Vizual_Picture_Array;

            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Vizual_Picture_Array()  // Визуадизация главного окна
            {
                Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            }
        private void imageTaken(Image newImage)
        {
            //изображение получено
            pictureBox1.Image = newImage;
            //  ShooterSingleton.imageCaptured -= imageTaken;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                Режимы графика
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton9_CheckedChanged(object sender, EventArgs e)        // Координаты 1 точки
        {
            if (radioButton913.Checked == true) { cursorMode = 0; }
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)        // Координаты 2 точки
        {
            if (radioButton16.Checked == true) { cursorMode = 1; }
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)       // Координаты 3 точки
        {
            if (radioButton17.Checked == true) { cursorMode = 2; }
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)       // Координаты 4 точки
        {
            if (radioButton18.Checked == true) { cursorMode = 3; }
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)       // График из zArray
        {
            //if (cursorMode == 7) { radioButton15.Checked = false; cursorMode = 0;  return;  }
            //if (cursorMode != 7) { radioButton15.Checked = true;  cursorMode = 7;  return; }

            if (radioButton15.Checked == true) { cursorMode = 7; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                    MouseMove  на основном окне
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox01_MouseMove(object sender, MouseEventArgs e)
        {
            if (cursorMode != 7) return;
            if (zArrayPicture == null) {  return; }

            int w_max = zArrayPicture.width;
            int h_max = zArrayPicture.height;

            int rx = pictureBox01.Width;
            int ry = pictureBox01.Height;


            int x = (int)e.X;
            int y = (int)e.Y;

            if (scaleMode == 1)                                                     // Подогнанный режим отображения      
            {
                x = pdgn_scale(w_max, h_max, rx, ry, x, y, 1);
                y = pdgn_scale(w_max, h_max, rx, ry, x, y, 2);
            }
            imageWidth.Text  = x.ToString();
            imageHeight.Text = y.ToString();
           
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                    MouseClick  на основном окне
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static int pdgn_scale(int w_max, int h_max, int rx, int ry, int x, int y, int k)
        {
            double kx = 1, ky = 1;

            if (w_max > h_max)               // Ширина больше высоты
            {
                kx = (double)w_max / rx;
                x = (int)(x * kx);
                int y0 = (int)(ry - h_max / kx) / 2;
                int y1 = ry - y0;

                if (y < y0) y = 0;
                if (y > y1) y = h_max;
                if ((y >= y0) && y <= y1) y = (int)((y - y0) * kx);
                // MessageBox.Show("y0= " + y0 +  " y1= " + y1 );
            }
            if (w_max < h_max)
            {
                ky = (double)h_max / ry;
                y = (int)(y * ky);
                int x0 = (int)(rx - w_max / ky) / 2;  //  int rx = pictureBox01.Width;
                int x1 = ry - x0;

                if (x < x0) x = 0;
                if (x > x1) x = w_max;
                if ((x >= x0) && x <= x1) x = (int)((x - x0) * ky);
            }

            if (w_max == h_max)
            {
                ky = (double)h_max / ry;
                y = (int)(y * ky);
                int x0 = (int)(rx - w_max / ky) / 2;

                //int x1 = ry - x0;
                int x1 = rx - x0;

                if (x < x0) x = 0;
                if (x > x1) x = w_max;
                if ((x >= x0) && x <= x1) x = (int)((x - x0) * ky);
            }
            if (k == 1) return x; else return y;

        }
/// <summary>
///      Клик мышкой на центральном окне
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            //if (pictureBox01.Image == null)  {  return;   }
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }

            int w_max = zArrayPicture.width;
            int h_max = zArrayPicture.height;


            imageWidth.Text = w_max.ToString();                                                        // Отображение размера файла -> imageWidth.Text
            imageHeight.Text = h_max.ToString();

            int rx = pictureBox01.Width;
            int ry = pictureBox01.Height;

         
            int x = 0, y = 0;

            x = (int)e.X; y = (int)e.Y;

            // MessageBox.Show("Width= " + rx + " Height= " + ry + "x= " + x + " y= " + y);    
            if (scaleMode == 1)                                                                            // Подогнанный режим отображения      
            {
                x = pdgn_scale(w_max, h_max, rx, ry, x, y, 1);
                y = pdgn_scale(w_max, h_max, rx, ry, x, y, 2);
            }
            switch (cursorMode)   // Задание 4 точек
            {
                case 0: X1 = x; Y1 = y;  textBox3.Text = x.ToString(); textBox4.Text  = y.ToString(); break;
                case 1: X2 = x; Y2 = y;  textBox5.Text = x.ToString(); textBox6.Text  = y.ToString(); break;
                case 2: X3 = x; Y3 = y;  textBox7.Text = x.ToString(); textBox8.Text  = y.ToString(); break;
                case 3: X4 = x; Y4 = y;  textBox9.Text = x.ToString(); textBox10.Text = y.ToString(); break;
            }
           // MessageBox.Show("zArrayPicture "+ zArrayPicture.height);
            if (zArrayPicture.height != 0 || zArrayPicture.width != 0) { textBox11.Text = zArrayPicture.array[X1, Y1].ToString(); }  // Величина точки           

            // z1.Text = pictureBox01.Image.Height.ToString();

            /*  if (cursorMode != 0)
              {
                  if (cursorMode == 2)  {    ImageProcessor.floodImage(e.X, e.Y, Color.Black, pictureBox01.Image);      }
                  return;
              }
             * */
            if (cursorMode == 7)   // Режим построения графика из массива (Масштаб должен быть реальным или подогнанным)
            {

                int xx = (int)e.X;
                int yy = (int)e.Y;

                if (scaleMode == 1)                                                    // Подогнанный режим отображения      
                {
                    xx = pdgn_scale(w_max, h_max, rx, ry, xx, yy, 1);
                    yy = pdgn_scale(w_max, h_max, rx, ry, xx, yy, 2);
                }


                if (radioButton22.Checked != true)                                      // по X
                {
                    double[] buf = new double[zArrayPicture.width];
                    buf = Graphic_util.Graph_x(zArrayPicture, yy);

                    if (cbAltChart.Checked)
                    {
                        ShowAltGraphic(buf);
                    }
                    else
                    {
                        Graphic graphic_x = new Graphic(zArrayPicture.width, yy, buf);
                        graphic_x.Show();
                    }
                }
                else                                                                     // по Y
                {
                    double[] buf1 = new double[zArrayPicture.height];
                    buf1 = Graphic_util.Graph_y(zArrayPicture, xx);

                    if (cbAltChart.Checked)
                    {
                        ShowAltGraphic(buf1);
                    }
                    else
                    {
                        Graphic graphic_y = new Graphic(zArrayPicture.height, xx, buf1);
                        graphic_y.Show();
                    }
                }
                pictureBox01.Invalidate();


                //ImageHelper.drawGraph(pictureBox01.Image, e.X, e.Y, currentScaleRatio);
            }


        }
        //--------------------------------------------------------------
        private void ShowAltGraphic(double[] buf)
        {

            GraphFormHost graphFormHost = new GraphFormHost();
            IList<GraphInfo> graphCollection = new List<GraphInfo>();

            Point2D[] graphPoints = new Point2D[buf.Length];
            for (int j = 0; j < buf.Length; j++)
            {
                graphPoints[j] = new Point2D(j, buf[j]);
            }

            GraphInfo graphInfo = new GraphInfo("Graphic", System.Windows.Media.Colors.Black, graphPoints, true, false);
            graphCollection.Add(graphInfo);

            graphFormHost.GraphInfoCollection = graphCollection;

            Form form = new Form();
            form.Height = 300;
            form.Width = 900;
            graphFormHost.Dock = DockStyle.Fill;
            form.Controls.Add(graphFormHost);
            form.Show();
            
        }

        //  -----------------------------------------------------------      График 3D
        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Graph3D graph_3D = new Graph3D(zArrayPicture);
            Graph3D graph_3D = new Graph3D(pictureBox01);
            graph_3D.Show();
        }
        //  -----------------------------------------------------------      График 2D
        private void dToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Graphic2D graph_2D = new Graphic2D(this);
            graph_2D.Show();
        }
        // -----------------------------------------------------------      Refresh
        private void button9_Click(object sender, EventArgs e)
        {
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // -----------------------------------------------------------------------------Ограничение четырех массивов (9,10,11,12)
        //private void выделениеКругаToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        // }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>   // Удаление трапеии
        /// <param name="e"></param>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        public struct Coords
        {
            public double x, y;

            public Coords(double p1, double p2)
            {
                x = p1;
                y = p2;
            }
        }
        private void удалениеТрапеции4Массивов13ToolStripMenuItem_Click(object sender, EventArgs e)
        {


           Coords[] X = new Coords[4];
           
            X[0] = new Coords(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text));
            X[1] = new Coords(Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text));
            X[2] = new Coords(Convert.ToDouble(textBox7.Text), Convert.ToDouble(textBox8.Text));
            X[3] = new Coords(Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox10.Text));

            zArrayDescriptor[regComplex * 4]   = File_Change_Size.Change_trapezium(zArrayDescriptor[regComplex*4], X);
            zArrayDescriptor[regComplex * 4+1] = File_Change_Size.Change_trapezium(zArrayDescriptor[regComplex * 4+1], X);
            zArrayDescriptor[regComplex * 4+2] = File_Change_Size.Change_trapezium(zArrayDescriptor[regComplex * 4+2], X);
            zArrayDescriptor[regComplex * 4+3] = File_Change_Size.Change_trapezium(zArrayDescriptor[regComplex * 4+3], X);

            Vizual_regImage(regComplex * 4);
            Vizual_regImage(regComplex * 4 + 1);
            Vizual_regImage(regComplex * 4 + 2);
            Vizual_regImage(regComplex * 4 + 3);

        }
        /// <summary>
        ///  Удаление трапеции из центрального окна по 4 точкам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void удалениеТрапецииzArrayPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Coords[] X = new Coords[4];

            X[0] = new Coords(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text));
            X[1] = new Coords(Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text));
            X[2] = new Coords(Convert.ToDouble(textBox7.Text), Convert.ToDouble(textBox8.Text));
            X[3] = new Coords(Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox10.Text));

            zArrayPicture = File_Change_Size.Change_trapezium(zArrayPicture, X);

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        // Простое вырезение прямоугольника (8,9,10,11)
        private void выделениеПрямоугольникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Coords[] X = new Coords[4];

            X[0] = new Coords(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text));
            X[1] = new Coords(Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text));
            X[2] = new Coords(Convert.ToDouble(textBox7.Text), Convert.ToDouble(textBox8.Text));
            X[3] = new Coords(Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox10.Text));


            zArrayDescriptor[regComplex * 4]     = File_Change_Size.Change_rectangle(zArrayDescriptor[regComplex * 4], X);
            zArrayDescriptor[regComplex * 4 + 1] = File_Change_Size.Change_rectangle(zArrayDescriptor[regComplex * 4 + 1], X);
            zArrayDescriptor[regComplex * 4 + 2] = File_Change_Size.Change_rectangle(zArrayDescriptor[regComplex * 4 + 2], X);
            zArrayDescriptor[regComplex * 4 + 3] = File_Change_Size.Change_rectangle(zArrayDescriptor[regComplex * 4 + 3], X);

            Vizual_regImage(regComplex * 4);
            Vizual_regImage(regComplex * 4 + 1);
            Vizual_regImage(regComplex * 4 + 2);
            Vizual_regImage(regComplex * 4 + 3);
        }
        // Простое вырезение прямоугольника zArrayPicture
        private void выделениеПрямоугольникаИзЦентральногоОкнаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Coords[] X = new Coords[4];

            X[0] = new Coords(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text));
            X[1] = new Coords(Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text));
            X[2] = new Coords(Convert.ToDouble(textBox7.Text), Convert.ToDouble(textBox8.Text));
            X[3] = new Coords(Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox10.Text));

            zArrayPicture = File_Change_Size.Change_rectangle(zArrayPicture, X);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        private void рисованиеПрямоугольникаToolStripMenuItem_Click(object sender, EventArgs e) // Рисование прямоугольника по 4 выделенным точкам в zArrayPicture
        {

            if (zArrayPicture.height == 0) { MessageBox.Show(" zArrayPicture == null"); return; }

            int X1, X2, X3, X4, Y1, Y2, Y3, Y4;

            X1 = Convert.ToInt32(textBox3.Text);
            Y1 = Convert.ToInt32(textBox4.Text);

            X2 = Convert.ToInt32(textBox5.Text);
            Y2 = Convert.ToInt32(textBox6.Text);

            X3 = Convert.ToInt32(textBox7.Text);
            Y3 = Convert.ToInt32(textBox8.Text);

            X4 = Convert.ToInt32(textBox9.Text);
            Y4 = Convert.ToInt32(textBox10.Text);

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);  

            Vizual.Vizual_Rect_Ris(zArrayPicture, pictureBox01, minx, miny, maxx, maxy);

        }

        private void рисованиеОкружностиToolStripMenuItem_Click(object sender, EventArgs e) // Рисование окружности по 4 выделенным точкам в zArrayPicture
        {
            //if (regImage != 8) { MessageBox.Show(" regImage =" + regImage + "!= 8"); return; }


            if (zArrayPicture.height == 0) { MessageBox.Show(" zArrayPicture == null"); return; }

            int X1, X2, X3, X4, Y1, Y2, Y3, Y4;

            X1 = Convert.ToInt32(textBox3.Text);
            Y1 = Convert.ToInt32(textBox4.Text);

            X2 = Convert.ToInt32(textBox5.Text);
            Y2 = Convert.ToInt32(textBox6.Text);

            X3 = Convert.ToInt32(textBox7.Text);
            Y3 = Convert.ToInt32(textBox8.Text);

            X4 = Convert.ToInt32(textBox9.Text);
            Y4 = Convert.ToInt32(textBox10.Text);

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);

            int rx = (maxx - minx) / 2;
            int ry = (maxy - miny) / 2;
            int r = Math.Min(rx, ry);
            int x0 = minx + r;
            int y0 = miny + r;

            Vizual.Vizual_Circle_Ris(zArrayPicture, pictureBox01, x0, y0, r);

        }

        // Выделение круга по 4 выделенным точкам в 8,9,10 и 11 zArrayDescriptor[]

        private void выделениеКвадратаПо4ВыделеннымТочкамToolStripMenuItem_Click(object sender, EventArgs e)  
        {

            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

            int X3 = Convert.ToInt32(textBox7.Text);
            int Y3 = Convert.ToInt32(textBox8.Text);

            int X4 = Convert.ToInt32(textBox9.Text);
            int Y4 = Convert.ToInt32(textBox10.Text);

            File_Change_Size.Change_r(zArrayDescriptor, pictureBox9, pictureBox10, pictureBox11, pictureBox12,X1, X2, X3, X4, Y1, Y2, Y3, Y4);
            Vizual_regImage(8); Vizual_regImage(9); Vizual_regImage(10); Vizual_regImage(11);
        }

        private int min4x(int X1, int X2, int X3, int X4)
        {
            int min = int.MaxValue;

            if (X1 < min) min = X1;
            if (X2 < min) min = X2;
            if (X3 < min) min = X3;
            if (X4 < min) min = X4;
            return min;
        }

        private int max4x(int X1, int X2, int X3, int X4)
        {
            int max = 0;

            if (X1 > max) max = X1;
            if (X2 > max) max = X2;
            if (X3 > max) max = X3;
            if (X4 > max) max = X4;
            return max;
        }

        private void выделениеКругаПо4ТочкамToolStripMenuItem_Click(object sender, EventArgs e)  // Выделение круга в zArrayPicture
        {
            //if (regImage != 8) { MessageBox.Show(" regImage =" + regImage + "!= 8"); return; }


            if (zArrayPicture.height == 0) { MessageBox.Show(" zArrayPicture == null"); return; }

            int X1, X2, X3, X4, Y1, Y2, Y3, Y4;

            X1 = Convert.ToInt32(textBox3.Text);
            Y1 = Convert.ToInt32(textBox4.Text);

            X2 = Convert.ToInt32(textBox5.Text);
            Y2 = Convert.ToInt32(textBox6.Text);

            X3 = Convert.ToInt32(textBox7.Text);
            Y3 = Convert.ToInt32(textBox8.Text);

            X4 = Convert.ToInt32(textBox9.Text);
            Y4 = Convert.ToInt32(textBox10.Text);

            int minx = min4x(X1, X2, X3, X4);
            int maxx = max4x(X1, X2, X3, X4);
            int miny = min4x(Y1, Y2, Y3, Y4);
            int maxy = max4x(Y1, Y2, Y3, Y4);

            int rx = (maxx - minx) / 2;
            int ry = (maxy - miny) / 2;
            int r = Math.Min(rx, ry);
            int x0 = minx + r;
            int y0 = miny + r;

            Vizual.Vizual_Circle(zArrayPicture, pictureBox01, x0, y0, r);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        // -----------------Ограничение массива прямоугольником zArrayPiture
        private void выделениеКвадратаПо4ёмВыделеннымТочкам9101112ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

            int X3 = Convert.ToInt32(textBox7.Text);
            int Y3 = Convert.ToInt32(textBox8.Text);

            int X4 = Convert.ToInt32(textBox9.Text);
            int Y4 = Convert.ToInt32(textBox10.Text); 

            zArrayPicture = File_Change_Size.Change_4(zArrayPicture, X1, X2, X3, X4, Y1, Y2, Y3, Y4);

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       /* private void rB_Checked(object sender)
        {
            regImage = 0;

            RadioButton rb = sender as RadioButton;

            if (rb == radioButton1)  { regImage = 0; }
            if (rb == radioButton2)  { regImage = 1; }
            if (rb == radioButton3)  { regImage = 2; }
            if (rb == radioButton4)  { regImage = 3; }
            if (rb == radioButton5)  { regImage = 4; }
            if (rb == radioButton6)  { regImage = 5; }
            if (rb == radioButton7)  { regImage = 6; }
            if (rb == radioButton8)  { regImage = 7; }
            if (rb == radioButton9)  { regImage = 8; }
            if (rb == radioButton10) { regImage = 9; }
            if (rb == radioButton11) { regImage = 10; }
            if (rb == radioButton14) { regImage = 11; }

            if (rb == radioButton19) { regComplex = 0; }
            if (rb == radioButton20) { regComplex = 1; }
            if (rb == radioButton21) { regComplex = 2; }
            // MessageBox.Show(" regImage = " + regImage + " regComplex = " + regComplex);

            // if (img[regImage] != null)
            // {
            //imageWidth.Text = img[regImage].Width.ToString();
            //imageHeight.Text = img[regImage].Height.ToString();
            // }
        }
        */
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //regImage = 0;

            RadioButton rb = sender as RadioButton;

            if (rb == radioButton1) { regImage = 0; }
            if (rb == radioButton2) { regImage = 1; }
            if (rb == radioButton3) { regImage = 2; }
            if (rb == radioButton4) { regImage = 3; }
            if (rb == radioButton5) { regImage = 4; }
            if (rb == radioButton6) { regImage = 5; }
            if (rb == radioButton7) { regImage = 6; }
            if (rb == radioButton8) { regImage = 7; }
            if (rb == radioButton9) { regImage = 8; }
            if (rb == radioButton10) { regImage = 9; }
            if (rb == radioButton11) { regImage = 10; }
            if (rb == radioButton14) { regImage = 11; }

            if (rb == radioButton19) { regComplex = 0; }
            if (rb == radioButton20) { regComplex = 1; }
            if (rb == radioButton21) { regComplex = 2; }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     <-
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void button3_Click(object sender, EventArgs e)          // Стрелка влево
        {
            //rB_Checked(sender);
            //MessageBox.Show(" regImage = " + regImage + " regComplex = " + regComplex);
            if (zArrayDescriptor[regImage] == null) return;
            applyScaleModeToPicturebox();


            int nx = zArrayDescriptor[regImage].width;
            int ny = zArrayDescriptor[regImage].height;
            zArrayPicture = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    zArrayPicture.array[i, j] = zArrayDescriptor[regImage].array[i, j];

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

            // currentScaleRatio = 1;
            applyScaleModeToPicturebox();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     ->
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button12_Click(object sender, EventArgs e)  // Стрелка вправо
        {
            if (zArrayPicture == null) return;

            img[regImage] = pictureBox01.Image;

            int nx = zArrayPicture.width;
            int ny = zArrayPicture.height;

            zArrayDescriptor[regImage] = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    zArrayDescriptor[regImage].array[i, j] = zArrayPicture.array[i, j];

            Vizual_regImage(regImage);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ZGR_File(int i)
        {
            zArrayDescriptor[i] = File_Helper.loadImage();
            Vizual_regImage(i);
            //catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }					
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) { ZGR_File(0); }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e) { ZGR_File(1); }
        private void pictureBox3_MouseClick_1(object sender, MouseEventArgs e) { ZGR_File(2); }
        private void pictureBox4_MouseClick(object sender, MouseEventArgs e) { ZGR_File(3); }
        private void pictureBox5_MouseClick(object sender, MouseEventArgs e) { ZGR_File(4); }
        private void pictureBox6_MouseClick(object sender, MouseEventArgs e) { ZGR_File(5); }
        private void pictureBox7_MouseClick(object sender, MouseEventArgs e) { ZGR_File(6); }
        private void pictureBox8_MouseClick(object sender, MouseEventArgs e) { ZGR_File(7); }
        private void pictureBox9_MouseClick(object sender, MouseEventArgs e) { ZGR_File(8); }
        private void pictureBox10_MouseClick(object sender, MouseEventArgs e) { ZGR_File(9); }
        private void pictureBox11_MouseClick(object sender, MouseEventArgs e) { ZGR_File(10); }
        private void pictureBox12_MouseClick(object sender, MouseEventArgs e) { ZGR_File(11); }

       

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //               Отображение окна от 0 до 11
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public void Vizual_regImage(int k)
        {
            Vizual.Vizual_Picture(zArrayDescriptor[k], pictureBoxArray[k]);
        }

        public void Vizual_regImageAsRaw(int k)
        {
            Vizual.Vizual_PictureAsRaw(zArrayDescriptor[k], pictureBoxArray[k]);
        }
          
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //               Отображение комплексных чисел Complex(int regComplex)
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Complex_pictureBox(int regComplex)
        {
            if (zComplex[regComplex] == null) { MessageBox.Show("Complex_pictureBox:  zComplex[regComplex] == NULL"); return; }

            int width  = zComplex[regComplex].width;
            int height = zComplex[regComplex].height;
            double[,] Image_double = new double[width, height];

            //MessageBox.Show("regComplex " + Convert.ToString(regComplex) + "width " + Convert.ToString(width) + "height " + Convert.ToString(height));

            Image_double = Furie.Re(zComplex[regComplex].array);
            zArrayDescriptor[regComplex*4] = new ZArrayDescriptor(Image_double);             Vizual_regImage(regComplex * 4);

            Image_double = Furie.Im(zComplex[regComplex].array);
            zArrayDescriptor[regComplex*4 + 1] = new ZArrayDescriptor(Image_double);         Vizual_regImage(regComplex * 4 + 1);

            Image_double = Furie.Amplituda(zComplex[regComplex].array);
            zArrayDescriptor[regComplex*4 + 2] = new ZArrayDescriptor(Image_double);         Vizual_regImage(regComplex * 4 + 2);

            Image_double = Furie.Faza(zComplex[regComplex].array);
            zArrayDescriptor[regComplex * 4 + 3] = new ZArrayDescriptor(Image_double);       Vizual_regImage(regComplex * 4 + 3);

        }
      
        //--------------------------------------------------------------------------------------------------------------------------------------
        //                                        Ввод-вывод
        //--------------------------------------------------------------------------------------------------------------------------------------
        private void ZGRToolStripMenuItem_Click(object sender, EventArgs e)   // Загрузить файл в pictureBox01 и ZArrayDescriptor zArrayPicture
        {
            zArrayPicture = File_Helper.loadImage();
            if (zArrayPicture != null) Vizual.Vizual_Picture(zArrayPicture, pictureBox01);     // Отображение на pictureBox01
                                                                                               //zArrayPicture.Double_Picture(pictureBox01);
                                                                                               // Отображение на pictureBox01

            zArrayPictureOriginal = new ZArrayDescriptor(zArrayPicture);

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SAVEToolStripMenuItem_Click(object sender, EventArgs e)  // Сохранить файл из pictureBox01 в графический файл
        {
            File_Helper.saveImage(pictureBox01);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void сохранитьZaarayВФайлToolStripMenuItem_Click(object sender, EventArgs e)        //   Сохранить Zarray в файл (double)
        {
            File_Helper.saveZArray(zArrayPicture);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void загрузитьИзФайлаdoubleToolStripMenuItem_Click(object sender, EventArgs e)       //  Загрузить из файла (double  Zarray)
        {
            zArrayPicture = File_Helper.loadZArray();
            if (zArrayPicture != null) Vizual.Vizual_Picture(zArrayPicture, pictureBox01);           // Отображение на pictureBox01
        }

        private void загрузитьComplexToolStripMenuItem_Click(object sender, EventArgs e)             // Загрузить в ZComplex[regComplex] из файла
        {
            zComplex[regComplex] = File_Helper.loadZComplex();
            Complex_pictureBox(regComplex);

        }

        private void сщхранитьComplexZComplexToolStripMenuItem_Click(object sender, EventArgs e)         // Сохранить ZComplex[regComplex] в файл 
        {
            File_Helper.saveZComplex(zComplex[regComplex]);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string SaveString(string string_dialog, int k)
        {

            string strk = k.ToString();

            string string_rab = string_dialog;

            if (string_dialog.Contains("11.")) { string_rab = string_dialog.Replace("11.", "12."); return string_rab; }
            if (string_dialog.Contains("10.")) { string_rab = string_dialog.Replace("10.", "11."); return string_rab; }
            if (string_dialog.Contains("9.")) { string_rab = string_dialog.Replace("9.", "10."); return string_rab; }


            //MessageBox.Show("SaveString  ERROR - Первый файл должен оканчиваться на 9" + string_dialog + " string_rab - " + string_rab);

            return null;

        }
        private string SaveString8(string string_dialog, int k)
        {

            string strk = k.ToString();

            string string_rab = string_dialog;

            if (string_dialog.Contains("1."))  { string_rab = string_dialog.Replace("1.", "2.");    return string_rab; }
            if (string_dialog.Contains("2."))  { string_rab = string_dialog.Replace("2.", "3.");    return string_rab; }
            if (string_dialog.Contains("3."))  { string_rab = string_dialog.Replace("3.", "4.");    return string_rab; }
            if (string_dialog.Contains("4."))  { string_rab = string_dialog.Replace("4.", "5.");    return string_rab; }
            if (string_dialog.Contains("5."))  { string_rab = string_dialog.Replace("5.", "6.");    return string_rab; }
            if (string_dialog.Contains("6."))  { string_rab = string_dialog.Replace("6.", "7.");    return string_rab; }
            if (string_dialog.Contains("7."))  { string_rab = string_dialog.Replace("7.", "8.");    return string_rab; }
            if (string_dialog.Contains("8."))  { string_rab = string_dialog.Replace("8.", "9.");    return string_rab; }
            if (string_dialog.Contains("9."))  { string_rab = string_dialog.Replace("9.", "10.");   return string_rab; }
            if (string_dialog.Contains("10.")) { string_rab = string_dialog.Replace("10.", "11.");  return string_rab; }
            if (string_dialog.Contains("11.")) { string_rab = string_dialog.Replace("11.", "12.");  return string_rab; }


            //MessageBox.Show("SaveString  ERROR - Первый файл должен оканчиваться на 9" + string_dialog + " string_rab - " + string_rab);

            return null;

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ZGR_File(string str, int i)   // Перегруженный ZGR_File(int i)
        {
            zArrayDescriptor[i] = File_Helper.loadImage_from_File(str);
            Vizual_regImage(i);
            //catch (Exception ex) { MessageBox.Show(" Ошибка " + ex.Message); }					
        }

        private void загрузить418ToolStripMenuItem_Click(object sender, EventArgs e)  // Загрузить 4 файла в 9,10,11,12
        {
            var dialog1 = new OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;

                    progressBar1.Visible = true;
                    progressBar1.Minimum = 1;
                    progressBar1.Maximum = 5;
                    progressBar1.Value = 1;
                    progressBar1.Step = 1;

                    for (int i = 0; i < 4; i++)
                    {
                        ZGR_File(str, regComplex*4+i);
                        str = SaveString8(str, i);                          //if (str == null) break;  // Неправильное имя файла
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        progressBar1.PerformStep();
                    }

                    progressBar1.Value = 1;
                }
                catch (Exception ex) { MessageBox.Show("загрузить418ToolStripMenuItem_Click Ошибка " + ex.Message); }
            }
        }


        private void Save8ToolStripMenuItem_Click(object sender, EventArgs e)   // Загрузить 8 файлов в 0,1,...,7
        {
            var dialog1 = new OpenFileDialog();
            dialog1.InitialDirectory = string_dialog;

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = 9;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;

                    for (int i = 0; i < 8; i++)
                    {
                        ZGR_File(str,  i);
                        str = SaveString8(str, i); //if (str == null) break;  // Неправильное имя файла
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        progressBar1.PerformStep();
                    }                 
                }
                catch (Exception ex) { MessageBox.Show("загрузить418ToolStripMenuItem_Click Ошибка " + ex.Message); }
            }
            progressBar1.Value = 1;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     private void загрузить49ю12ToolStripMenuItem_Click(object sender, EventArgs e)
        //     {

        //     }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void userChoosedSize(Size newSize)
        {
            for (int i = 0; i < 8; i++)
            {
                img[i] = ImageProcessor.ResizeBitmap((Bitmap)img[i], newSize.Width, newSize.Height);

                switch (i)
                {
                    case 0: pictureBox1.Image = img[i]; pictureBox1.Invalidate(); break;
                    case 1: pictureBox2.Image = img[i]; pictureBox2.Invalidate(); break;
                    case 2: pictureBox3.Image = img[i]; pictureBox3.Invalidate(); break;
                    case 3: pictureBox4.Image = img[i]; pictureBox4.Invalidate(); break;
                    case 4: pictureBox5.Image = img[i]; pictureBox5.Invalidate(); break;
                    case 5: pictureBox6.Image = img[i]; pictureBox6.Invalidate(); break;
                    case 6: pictureBox7.Image = img[i]; pictureBox7.Invalidate(); break;
                    case 7: pictureBox8.Image = img[i]; pictureBox8.Invalidate(); break;
                }
            }

            if (img[regImage] != null)
            {
                //imageWidth.Text = img[regImage].Width.ToString();
                //imageHeight.Text = img[regImage].Height.ToString();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void сохранить8КадровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog1 = new SaveFileDialog();
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "(*.JPG)|*.JPG|(*.bmp)|*.bmp";

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int count = 8;

                    Bitmap newBitmap = null;

                    for (int k = 0; k < count; k++)
                    {
                        PictureBox pictureBox = pictureBoxArray[k];
                        newBitmap = new Bitmap(pictureBox.Image);
                        int num = k + 1;

                        string directory = Path.GetDirectoryName(dialog1.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(dialog1.FileName);
                        string extension = Path.GetExtension(dialog1.FileName);
                        string filePath = Path.Combine(directory, fileName + num.ToString() + extension);

                        newBitmap.Save(filePath);
                    }
                    
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Ошибка при записи файла " + ex.Message);
                }
            }
        }
        /// <summary>
        ///                                                 Сохранить 4 файла 8,9,10,11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


        private void сохранить4Кадра9101112ToolStripMenuItem_Click(object sender, EventArgs e)  // Сохранить 4 файла из 8,9,10,11 с добавлением цифр 9,10,11,12
        {
            var dialog1 = new SaveFileDialog();
            dialog1.InitialDirectory = string_dialog;
            dialog1.Filter = "(*.JPG)|*.JPG|(*.bmp)|*.bmp";
            string str1;

            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = 5;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dialog1.InitialDirectory = dialog1.FileName;
                    string_dialog = dialog1.FileName;
                    string str = string_dialog;

                    PictureBox[] array = new PictureBox[]
                    {
                        pictureBoxArray[8],
                        pictureBoxArray[9],
                        pictureBoxArray[10],
                        pictureBoxArray[11]
                    };

                    int count = array.Length;
                    Bitmap newBitmap = null;

                    for (int k = 0; k < count; k++)
                    {
                        PictureBox pictureBox = array[k];
                        newBitmap = new Bitmap(pictureBox.Image);
                        int num = k + 9;

                        string directory = Path.GetDirectoryName(dialog1.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(dialog1.FileName);
                        string extension = Path.GetExtension(dialog1.FileName);
                        string filePath = Path.Combine(directory, fileName + num.ToString() + extension);

                        newBitmap.Save(filePath);
                        progressBar1.PerformStep();
                    }
                    
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (Exception ex) { MessageBox.Show(" Ошибка при записи файла " + ex.Message); }
            }
            progressBar1.Value = 1;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageProcessed(Bitmap newImage)
        {
            FazaClass.imageProcessed -= imageProcessed;
            SetControlPropertyThreadSafe(pictureBox01, "Image", newImage);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void captureImage(object sender, EventArgs e)
        {
            // ShooterSingleton.imageCaptured += imageTaken;
            // ShooterSingleton.getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Cadr8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundImagesGeneratorForm newForm = new BackgroundImagesGeneratorForm();
            newForm.oneImageOfSeries += oneImageCaptured;
            newForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void oneImageCaptured(Image newImage, int imageNumber)
        {
            if (imageNumber == 1) { pictureBox1.Image = newImage; }
            else if (imageNumber == 2) { pictureBox2.Image = newImage; }
            else if (imageNumber == 3) { pictureBox3.Image = newImage; }
            else if (imageNumber == 4) { pictureBox4.Image = newImage; }
            else if (imageNumber == 5) { pictureBox5.Image = newImage; }
            else if (imageNumber == 6) { pictureBox6.Image = newImage; }
            else if (imageNumber == 7) { pictureBox7.Image = newImage; }
            else if (imageNumber == 8) { pictureBox8.Image = newImage; }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /* private void radioButton10_CheckedChanged(object sender, EventArgs e)
         {
             scaleMode = 0;
             radioButton12.Checked = true;
             radioButton13.Checked = false;
             applyScaleModeToPicturebox();

             if (radioButton103.Checked == true)
             {
                 cursorMode = 1;
             }
         }
         /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
         private void radioButton11_CheckedChanged(object sender, EventArgs e)
         {
             if (radioButton11.Checked == true)
             {
                 cursorMode = 2;
             }
         }
         * */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pictureBox01_MouseDown(object sender, MouseEventArgs e)
        {
            if (cursorMode == 1) { downPoint = new Point(e.X, e.Y); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            relayout();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void relayout()
        {
            //this.panel1.Size = new Size(this.Size.Width - 160, this.Size.Height - 150);
            this.pictureBox01.Size = new Size(this.panel1.Size.Width - 44, this.panel1.Size.Height - 36);
            //this.panel1.Size = new Size(this.Size.Width - 160, this.Size.Height - 150);
            //this.pictureBox01.Size = new Size(800, 600);

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton12_CheckedChanged(object sender, EventArgs e)    // Реальный
        {
            scaleMode = 0;
            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton13_CheckedChanged(object sender, EventArgs e)     // Подогнанный (все изображение в окне)
        {
            scaleMode = 1;
            applyScaleModeToPicturebox();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void applyScaleModeToPicturebox()
        {
            if (scaleMode == 0) { pictureBox01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize; }
            else
                  if (scaleMode == 1) { pictureBox01.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; }

            relayout();
            pictureBox01.Invalidate();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void restoderImageReceived(Bitmap newBitmap, double ratio)
        {
            pictureBox10.Image = newBitmap;
            pictureBox10.Invalidate();
            pictureBox10.Update();

            afterRemovingScaleRatio = ratio;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void phaseImageReceived(Bitmap newBitmap, double ratio)
        {
            pictureBox9.Image = newBitmap;
            pictureBox9.Invalidate();
            pictureBox9.Update();

            initialScaleRatio = ratio;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Фильтрация нескольких кадров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void фильтрацияНесколькихКадровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filtr FLT = new Form_Filtr();

            FLT.Show();
        }

        // ------------------------------  Сглаживание
        private void сглаживаниеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Click_SM);
        }
        private void filt_Click_SM(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_smothingSM(amp, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }
        // ------------------------------  Удаление низких частот
        private void сглаживаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Click);
        }
        private void filt_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_smothing(amp, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }
        // -------------------------------  Медианный
        private void медианныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_median_Click);
        }
        private void filt_median_Click(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_Mediana(amp, k_filt);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }
        // -------------------------------  Рамка
        private void рамкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_Ramka);
        }
        private void filt_Ramka(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_Ramka(amp, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }

        // -------------------------------  Взвешенный фильтр для голографии
        private void взвешенныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_Hologramm(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        // -------------------------------  Усреднение 2 точек с уменьшением размера файла
        private void усреднениеДвухТочекПоСтрокеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_2(amp, 1);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        // -------------------------------  Усреднение 2х2 точек с уменьшением размера файла
        private void усреднение2х2ТочекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_2х2(amp, 0, 0);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        // -------------------------------  Уменьшением размера файла в два раза
        private void уменьшениеМассиваВ2РазаToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Filt_1_2(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
        // -------------------------------  Сложение всех строк
        private void сложениеВсехСтрокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.Sum_Line(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        // -------------------------------  Если выше или ниже границы => ноль
        private void неВГраницах0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double s1 = Convert.ToDouble(textBox1.Text);
            double s2 = Convert.ToDouble(textBox2.Text);
            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height; 

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {
                    if (zArrayPicture.array[i, j] >= s1 || zArrayPicture.array[i, j] <= s2) zArrayPicture.array[i, j]=0;
                }
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);        

        }

        // -------------------------------  
        // -------------------------------  Сверхразрешение
        /// <summary>
        /// Разряжение массива комплексных чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void разряжениеКомплексногоМассиваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Super SuperResoluton = new Super();

            //FrForm.OnFrenel += FrComplex;



            //FrForm.InversComplex += InversComplexM;
            SuperResoluton.Show();
        }



        // ----- Разряжение массива нулями 101010
        private void разряжениеМассиваНулямиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.Decim10);
        }     
        private void Decim10(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            zArrayPicture = FiltrClass.Decim10(zArrayPicture, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }


        private void разряжениеМассиваНулями10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.Decim01);
        }

        // -------------------------------  
        // -------------------------------  
        // ----- Разряжение массива нулями 010101
        private void Decim01(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);


            zArrayPicture = FiltrClass.Decim10(zArrayPicture, k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }

        private void добвлениеМассиваПовторениемЗначенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.Decim1);
        }

        private void Decim1(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);


            zArrayPicture = FiltrClass.Decim1(zArrayDescriptor[regImage], k_filt);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }


        // ----- 4 новых файла в два раза меньше в 1,2,3,4 
        private void массиваВ1234СоСдвигомНа12ПикселяToolStripMenuItem_Click(object sender, EventArgs e)
        // со cдвигом на половину пикселя
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);                                   // Из главного окна
            
            zArrayDescriptor[0] = FiltrClass.Filt_2х2(amp, 0, 0);
            zArrayDescriptor[1] = FiltrClass.Filt_2х2(amp, 1, 0);
            zArrayDescriptor[2] = FiltrClass.Filt_2х2(amp, 0, 1);
            zArrayDescriptor[3] = FiltrClass.Filt_2х2(amp, 1, 1);
            Vizual_regImage(0); Vizual_regImage(1); Vizual_regImage(2); Vizual_regImage(3);              // Отображение
        }

        // ----- 4 новых файла в четыре раза меньше в 1,2,3,4 
       
        // ----- Из  zArrayDescriptor[0] 1,2,3 в основное окно файл в 2 раза больший 
        private void из1234НовыйМассивToolStripMenuItem_Click(object sender, EventArgs e)               // 
        {
            int w2 = zArrayDescriptor[0].width;
            int h2 = zArrayDescriptor[0].height;
            int w1 = w2 * 2;
            int h1 = h2 * 2;
            //ZArrayDescriptor tmp = new ZArrayDescriptor(w2, h2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            /*
            tmp = zArrayDescriptor[0];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[1];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2 + 1, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[2];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2, j * 2 + 1] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[3];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2 + 1, j * 2 + 1] = tmp.array[i, j]; }
              */

            for (int i = 0; i < w1 ; i+=2)    for (int j = 0; j < h1 ; j+=2)
                {
                    res_array.array[i,     j]     = zArrayDescriptor[0].array[i / 2, j / 2];
                    res_array.array[i + 1, j]     = zArrayDescriptor[1].array[i / 2, j / 2];
                    res_array.array[i,     j + 1] = zArrayDescriptor[2].array[i / 2, j / 2];
                    res_array.array[i + 1, j + 1] = zArrayDescriptor[3].array[i / 2, j / 2];
                }
        
            zArrayPicture = res_array;

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void массиваВ1234СоСдвигомНа12ПикселяToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);                                   // Из главного окна
            int w1 = amp.width;
            int h1 = amp.height;
            int w2 = w1 / 4;
            int h2 = h1 / 4;
            //ZArrayDescriptor tmp = new ZArrayDescriptor(w2, h2);
            //tmp = FiltrClass.Filt_2х2(amp, 0, 0); zArrayDescriptor[0]=tmp;
            zArrayDescriptor[0] = FiltrClass.Filt_4х4(amp, 0, 0);
            zArrayDescriptor[1] = FiltrClass.Filt_4х4(amp, 1, 0);
            zArrayDescriptor[2] = FiltrClass.Filt_4х4(amp, 0, 1);
            zArrayDescriptor[3] = FiltrClass.Filt_4х4(amp, 1, 1);
            Vizual_regImage(0); Vizual_regImage(1); Vizual_regImage(2); Vizual_regImage(3);
        }


        private void из12НовыйМассивToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w2 = zArrayDescriptor[0].width;
            int h2 = zArrayDescriptor[0].height;
            int w1 = w2 * 2;
            int h1 = h2 * 2;
            //ZArrayDescriptor tmp = new ZArrayDescriptor(w2, h2);
            ZArrayDescriptor res_array = new ZArrayDescriptor(w1, h1);
            /*
            tmp = zArrayDescriptor[0];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[1];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2; j++) { res_array.array[i * 2 + 1, j * 2] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[2];  for (int i = 0; i < w2; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2, j * 2 + 1] = tmp.array[i, j]; }
            tmp = zArrayDescriptor[3];  for (int i = 0; i < w2 - 1; i++) for (int j = 0; j < h2 - 1; j++) { res_array.array[i * 2 + 1, j * 2 + 1] = tmp.array[i, j]; }
              */

            double [] tmp = new double[h1];
            for (int j = 0; j < h2 - 1; j++)
            {
                for (int i = 0; i < w2;     i++) { tmp[i * 2]                    = zArrayDescriptor[0].array[i, j]; }
                for (int i = 0; i < w2 - 1; i++) { tmp[i * 2 + 1]                = zArrayDescriptor[1].array[i, j]; }
                for (int i = 0; i < w1;     i++) { res_array.array[i, j * 2]     = tmp[i]; }
                for (int i = 0; i < w1;     i++) { res_array.array[i, j * 2 + 1] = tmp[i]; }
            }
          

            zArrayPicture = res_array;

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
           
        }
/// <summary>
///   Массив каждая точка которого 2х2 (Центральное окно)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void массивСУсреднением2х2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w2 = zArrayPicture.width;
            int h2 = zArrayPicture.height;

            zArrayPicture = FiltrClass.Filt1_2х2(zArrayPicture);

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
 /// <summary>
///   Массив каждая точка которого NхN (Центральное окно)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void массивСУсреднениемNxNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FiltDialog(this.filt_NxN);
        }
        private void filt_NxN(object sender, EventArgs e)
        {
            if (tb1_filt.Text != "") k_filt = Convert.ToInt32(tb1_filt.Text);
            
            zArrayPicture = FiltrClass.Filt1_NхN(zArrayPicture, k_filt);

            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            f_filt.Close();
        }
//------------------------------------------------------------------------------------------
        private void генерацияМассиваСоСдвигомНаПловинуПикселяToolStripMenuItem_Click(object sender, EventArgs e) // -----  Генерация файла со сдвигом на половину пикселя
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = FiltrClass.ADD_razresh_2х2(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        // -------------------------------  Увеличение массива в два раза простым повторением
        //       private void увеличениеМассиваВ2РазаПростымПовторениемToolStripMenuItem_Click(object sender, EventArgs e)
        //       {
        //           ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
        //zArrayPicture = FiltrClass.Filt_2_1(amp);
        //           zArrayPicture = FiltrClass.Filt_2_1_s(amp);
        //            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        //        }
        // -------------------------------  Увеличение массива в два раза со сдвигом на пловину пикселя
        private void увеличениеМассиваВ2РазаПростымПовторениемToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);

            zArrayPicture = FiltrClass.Filt_2_1_2(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        // -------------------------------  Увеличение массива в два раза со сдвигом на пловину пикселя

        private void увеличениеМассиваСРамкойToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);

            zArrayPicture = FiltrClass.Filt_2_Ramka(amp);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        //-------------------------------------------------------------------------------
        public void FiltDialog(EventHandler functionPointer)
        {
            int max_x = 120, max_y = 100;

            f_filt = new Form();
            f_filt.Size = new Size(max_x, max_y + 36);
            f_filt.StartPosition = FormStartPosition.Manual;
            Point p = this.Location;
            p.Offset(40, 165);
            f_filt.Location = p;

            Label label1 = new Label();
            label1.Location = new System.Drawing.Point(8, 10);
            label1.Text = "k = 1,2,3 ... :";

            tb1_filt = new TextBox();
            tb1_filt.Location = new System.Drawing.Point(8, 30);
            tb1_filt.Size = new System.Drawing.Size(60, 20);
            tb1_filt.Text = k_filt.ToString();

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(8, 60);
            b1.Text = "ok";
            b1.Size = new System.Drawing.Size(100, 30);
            b1.Click += new System.EventHandler(functionPointer);

            f_filt.Controls.Add(label1);
            f_filt.Controls.Add(tb1_filt);
            f_filt.Controls.Add(b1);

            f_filt.Show();

        }

        //                              Инверсия цветов

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zArrayPicture = SumClass.Inversia(zArrayPicture);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     ->    Complex ->  Re, Im. Am, Ph
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void button1_Click(object sender, EventArgs e)
        {
            Complex_pictureBox(regComplex);
        }



       


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //              Заполнение массива комплексных чисел  ( файл - Complex_form.cs)
        //   
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)  // Complex_form.cs
        {
            Complex_form BoxForm = new Complex_form();
            BoxForm.OnComplex += FormComplex;                           // фазу по центру амплитуда прежняя
            BoxForm.OnComplex0 += FormComplex0;
            BoxForm.OnComplexPhase += FormComplexPhase;
            BoxForm.OnComplexPhase_Random += FormComplexPhase_Random;  // Добавление к фазе случайных значений
            BoxForm.OnNull += FormNull;
            BoxForm.OnRe += FormRe;
            BoxForm.OnIm += FormIm;
            BoxForm.OnReIm0 += FormReIm0;
            BoxForm.OnCicle += FormCicle;
            BoxForm.OnAmpl += FormAmpl;                                // Меняем амплитуду (фаза прежняя)
            BoxForm.OnComplexNew += FormComplexNew;
            BoxForm.Show();
        }


        private void FormComplexNew()   // Создание нового Complex[regComplex]   Амплитуду из zArrayPicture (фаза нулевая) 
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL  (FormComplexNew)"); return; }

            int Nx = zArrayPicture.width;
            int Ny = zArrayPicture.height;

            zComplex[regComplex] = new ZComplexDescriptor(Nx, Ny);

            zComplex[regComplex] = new ZComplexDescriptor(zComplex[regComplex], zArrayPicture); // амплитуда = zComplex[regComplex], фаза - (прежняя) нулевая
            Complex_pictureBox(regComplex);
        }

        private void FormAmpl(int Nx, int Ny)   // В Complex[regComplex]   Амплитуду (фаза прежняя) ---------------zComplex------------------
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }

            Nx = zComplex[regComplex].width;
            Ny = zComplex[regComplex].height;

            ZArrayDescriptor ampl = new ZArrayDescriptor(Nx, Ny, zArrayPicture);                      //  в центре по краям 127, eсли не хватает    
            zComplex[regComplex] = new ZComplexDescriptor(zComplex[regComplex], ampl);
            Complex_pictureBox(regComplex);
        }



        private void FormComplex(double am, int Nx, int Ny)   // В Complex[regComplex]   фазу  по центру амплитуда прежняя
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL (Main фазу по центру амплитуда прежняя)"); return; }
            if (zComplex[regComplex] == null) { MessageBox.Show("zArrayPicture == NULL (Main фазу по центру амплитуда прежняя)"); return; }

            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);
            Nx = zComplex[regComplex].width;
            Ny = zComplex[regComplex].height;

            ZArrayDescriptor fz = new ZArrayDescriptor(Nx, Ny, zArrayPicture);                      // Фаза в центре по краям 127
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[regComplex * 4 + 2]);      // Амплитуда


            double max = SumClass.getMax(fz);
            double min = SumClass.getMin(fz);

            for (int i=0; i< Nx; i++)
                for (int j = 0; j < Ny; j++)
                {
                    fz.array[i, j] = (fz.array[i, j]-min) *2* Math.PI /(max-min) - Math.PI; 
                }




                    //MessageBox.Show("regComplex " + Convert.ToString(regComplex));
                    zComplex[regComplex] = new ZComplexDescriptor(fz, amp);                                // Конструктор

            Complex_pictureBox(regComplex);
        }
        //
        private void FormComplexPhase_Random()    //  В Complex[regComplex]  к существующей фазе добавляются случайные значения 
                                                  //  Амплитуда прежняя.     Шаблон в zArrayPicture.
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL  FormComplexPhase_Random"); return; }

            int Nx = zArrayDescriptor[regComplex * 4 + 2].width;
            int Ny = zArrayDescriptor[regComplex * 4 + 2].height;
            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny);                     // Фаза в центре (шаблон)
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[regComplex * 4 + 2]);      // Амплитуда прежняя

            zComplex[regComplex] = new ZComplexDescriptor(fz, amp, 1);

            Complex_pictureBox(regComplex);
        }


        private void FormComplex0(double am, int Nx, int Ny)   // В Complex[regComplex]   фазу  в левый угол
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }

            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);

            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny, 1);
            //MessageBox.Show("regComplex " + Convert.ToString(regComplex));

            zComplex[regComplex] = new ZComplexDescriptor(fz, am);
            //Vizual_regImage(regComplex);
            Complex_pictureBox(regComplex);
        }

        private void FormComplexPhase()   // В Complex[regComplex]   фазу  в левый угол амплитуда прежняя
        {

            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            if (zComplex[regComplex] == null) { MessageBox.Show("Complex[regComplex] == NULL"); return; }

            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);

            ZArrayDescriptor fz  = new ZArrayDescriptor(zArrayPicture);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[regComplex * 4 + 2]);      // Амплитуда

            zComplex[regComplex] = new ZComplexDescriptor(fz, amp);
            Complex_pictureBox(regComplex);
            MessageBox.Show("regComplex " + Convert.ToString(regComplex));
        }


        private void FormNull(int Nx, int Ny)                             // В Complex[regComplex] заносим нулевые значения
        {
            zComplex[regComplex] = new ZComplexDescriptor(Nx, Ny);
            Complex_pictureBox(regComplex);
        }
        /// <summary>
        ///  Меняем реальную часть комплексного массива zComplex[regComplex] значениями из центрального окнаz ArrayPicture
        /// </summary>
        /// <param name="Nx"></param>
        /// <param name="Ny"></param>
        private void FormRe(int Nx, int Ny)                               // В Complex[regComplex] заносим Re
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            if (zComplex[regComplex] == null) { MessageBox.Show("zComplex[" + regComplex + "] по центру == NULL"); return; }

            zComplex[regComplex] = new ZComplexDescriptor(zArrayPicture, zComplex[regComplex], 0);

            Complex_pictureBox(regComplex);
        }

        private void FormIm(int Nx, int Ny)                               // В Complex[regComplex] заносим Im
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);
            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny);
            zComplex[regComplex] = new ZComplexDescriptor(fz, zComplex[regComplex], 1);
            Complex_pictureBox(regComplex);
        }

        private void FormReIm0(int Nx, int Ny)                 // В Complex[regComplex] заносим Re Im=0 (В левый угол)
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            //ZArrayDescriptor fz = new ZArrayDescriptor(pictureBox01, Nx, Ny);
            ZArrayDescriptor fz = new ZArrayDescriptor(zArrayPicture, Nx, Ny, 1);
            zComplex[regComplex] = new ZComplexDescriptor(fz);
            Complex_pictureBox(regComplex);
        }

        //  Циклический сдвиг ---------------------------------------------------------------------------

        private void FormCicle()   // В Complex[regComplex]   циклический сдвиг
        {

            if (zComplex[regComplex] == null) { MessageBox.Show("Complex[regComplex] == NULL"); return; }

            zComplex[regComplex] = Furie.Invers(zComplex[regComplex]);

            Complex_pictureBox(regComplex);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //              Арифметические операции над массивами  ADD_Cmplx.cs
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ADD_Cmplx ADDPLUS = new ADD_Cmplx();
          
            ADDPLUS.On_Ampl   += Ampl_C;        // Амплитуда суммы двух волновых полей
          
            ADDPLUS.Show();
        }
        // Сами программы формы ADD_Cmplx находятся в  ADD_Math.cs - Арифметические операции над массивами
           
        private void Ampl_C(int k11, int k12)       { ADD_Math.Ampl_C(k11, k12); Vizual.Vizual_Picture(zArrayPicture, pictureBox01); }   // Амплитуда суммы двух комплексных массивов
       
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //        Двухмерное преобразование Фурье и Френеля
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void фурьеToolStripMenuItem1_Click(object sender, EventArgs e)
        {   
            FrenelForm FrForm = new FrenelForm();

            FrForm.OnFrenel         += FrComplex;
            FrForm.OnFrenelN        += FrComplexN;              // Френель с четным количеством точек
            FrForm.OnFrenelN_CUDA   += FrComplexN_CUDA;         // Френель CUDA

            FrForm.OnFurie               += FurComplex;
            //FrForm.OnFurieM              += FurComplexM;
            FrForm.OnFurie_N             += FurComplex_N;
            FrForm.OnFurie_2Line         += FurComplex_2Line;   // Из k1 => k2 (Complex) по строкам
            FrForm.OnFurie_CUDA          += FurComplex_CUDA;
            FrForm.OnFurie_CUDA_CMPLX    += FurComplex_CUDA1;   // Фурье CUDA из к1 в k2
            FrForm.OnFurie_NXY           += FurComplex_NXY;     // Фурье   с задаваемым количеством точек из k1 => k2
            FrForm.OnFrenel_NXY          += FrenelComplex_NXY;  // Френель с задаваемым количеством точек из k1 => k2
            FrForm.OnInterf_XY           += Frenel_XY;          // Интерферометрия двух сдвинутых фронтов

            FrForm.OnPSI_fast += FrPSI;                         // PSI + Фурье => Complex(k2) ------------------------
            FrForm.OnADD_PHASE += FrADD_PHASE;  // Фаза по пиле


            //FrForm.InversComplex += InversComplexM;
            FrForm.Show();
        }


        private void FrADD_PHASE(double step)   //  Фаза - пила
        {

            if (zComplex[regComplex] == null) { MessageBox.Show("zComplex[regComplex] == NULL  FrADD_PHASE"); return; }
           
          
          
            zComplex[regComplex] = Model_interf.ADD_PHASE(zComplex[regComplex], step);
          
            Complex_pictureBox(regComplex);
        }

        private void FrPSI(double[] fz, double xmax, double lambda, double d, int k2)    // быстрое PSI 8,9,10,11 -> zComplex[0]
        {
             if ( zArrayDescriptor[8]  == null)  { MessageBox.Show("FrPSI zComplex[8] == NULL");  return; }
             if ( zArrayDescriptor[9]  == null)  { MessageBox.Show("FrPSI zComplex[9] == NULL");  return; }
             if ( zArrayDescriptor[10] == null) { MessageBox.Show("FrPSI zComplex[10] == NULL"); return; }
             if ( zArrayDescriptor[11] == null) { MessageBox.Show("FrPSI zComplex[11] == NULL"); return; }
            
            double amplit = 160000;
            //zArrayPicture = ATAN_PSI.ATAN_quick(zArrayDescriptor,  progressBar1, fz, xmax, lambda, d, amplit);
            //zComplex[1] = ATAN_PSI.ATAN_891011(zArrayDescriptor, progressBar1, fz, amplit);     // Параметр amplit  не используется
            //FrComplexN(xmax, lambda, d, 1, 0);
           
            
            zComplex[k2] = FurieN.FrenelTransformN(ATAN_PSI.ATAN_8_11(8, 9, 10, 11, zArrayDescriptor,  fz, amplit), lambda, d, xmax);
            
            //MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);
            Complex_pictureBox(k2);
           
           
        }



        private void FrComplex(double xmax, double lambda, double d, int k1, int k2)
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int m = 1;
            int n = zComplex[k1].width;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > n) { n = nn / 2; m = i; break; } }

            MessageBox.Show("n = " + Convert.ToString(n) + " m = " + Convert.ToString(m));

            zComplex[k2] = new ZComplexDescriptor(n, n);
            //MessageBox.Show("regComplex " + Convert.ToString(regComplex));
            zComplex[k2] = Furie.FrenelTransform(zComplex[k1], m, lambda, d, xmax);
            MessageBox.Show(" m= " + m + " Lambda= " + lambda + " d= " + d + " dx= " + xmax);
            Complex_pictureBox(k2);
        }

        private void FrComplexN(double xmax, double lambda, double d, int k1, int k2)           //----------- Френель с четным количеством точек
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int nx = zComplex[k1].width;
            int ny = zComplex[k1].height;

            zComplex[k2] = new ZComplexDescriptor(nx, ny);

            //ZComplexDescriptor zComplex_Picture = new ZComplexDescriptor(zArrayPicture);        // Re=zArrayPicture Im=0
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
                  zComplex[k2] = FurieN.FrenelTransformN(zComplex[k1], lambda, d, xmax);
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            //MessageBox.Show(" Lambda= " + lambda + " d= " + d + " dx= " + xmax);
            Complex_pictureBox(k2);
        }


        private void FrComplexN_CUDA(double xmax, double lambda, double d, int k1, int k2)       //---------- Френель CUDA
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int nx = zComplex[k1].width;
            int ny = zComplex[k1].height;

            zComplex[k2] = new ZComplexDescriptor(nx, ny);

           // ZComplexDescriptor zComplex_Picture = new ZComplexDescriptor(zArrayPicture);        // Re=zArrayPicture Im=0
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            zComplex[k2] = CUDA_FFT.Fr_CUDA(zComplex[k1], lambda, d, xmax); 
            
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Френель CUDA  Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);

            //MessageBox.Show(" m= " + m + " Lambda= " + lambda + " d= " + d + " dx= " + xmax);
            Complex_pictureBox(k2);
        }


        private void FurComplex_NXY(int k1, int k2, int X, int Y, int N)     // Прямое BPF с задаваемым количеством точек
        {
            if (zComplex[k1] == null) { MessageBox.Show("zComplex[" + k1 + "] == NULL (FurComplex_NXY)"); return; }
            if (zComplex[k1].width < N)  { MessageBox.Show(" zComplex.width[" + k1 + "] < " + N + "] (FurComplex_NXY)"); return; }
            if (zComplex[k1].height < N) { MessageBox.Show(" zComplex.height[" + k1 + "] < " + N + "] (FurComplex_NXY)"); return; }

            zComplex[k2] = new ZComplexDescriptor(N, N);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    zComplex[k2].array[i, j] = zComplex[k1].array[i+X, j+Y];
           
            zComplex[k2] = FurieN.BPF2(zComplex[k2]);                    // Фурье преобразование для произвольного количества точек
           
            Complex_pictureBox(k2);
        }

        private void FrenelComplex_NXY(int k1, int k2, int X, int Y, int N, double xmax, double lambda, double d)     // Френель с задаваемым количеством точек
        {
            if (zComplex[k1] == null)    { MessageBox.Show("zComplex[" + k1 + "] == NULL (FrenelComplex_NXY)"); return; }
            if (zComplex[k1].width < N)  { MessageBox.Show(" zComplex.width[" + k1 + "] < " + N + "] (FrenelComplex_NXY)"); return; }
            if (zComplex[k1].height < N) { MessageBox.Show(" zComplex.height[" + k1 + "] < " + N + "] (FrenelComplex_NXY)"); return; }

            ZComplexDescriptor z = new ZComplexDescriptor(N, N);
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    z.array[i, j] = zComplex[k1].array[i + X, j + Y];

            //zComplex[k2] = FurieN.BPF2(zComplex[k2]);                    // Фурье преобразование для произвольного количества точек
            zComplex[k2] = FurieN.FrenelTransformN(z, lambda, d, xmax);

            Complex_pictureBox(k2);
        }



        private void Frenel_XY(double[] fz, double xmax, double lambda, double d, int X, int Y, int X1, int Y1, int N)     // Интерферометрия сдвинутых фронтов
        {
            Model_object.Interf_XY(zComplex, zArrayDescriptor, fz,  xmax,  lambda,  d, X,  Y,  X1,  Y1, N);
            Complex_pictureBox(0); 
            //Complex_pictureBox(1); Complex_pictureBox(2);
            //Vizual_regImage(4); Vizual_regImage(5); Vizual_regImage(6); Vizual_regImage(7);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
        }



        private void FurComplex_2Line(int k1, int k2)                  // Прямое Фурье преобразование для произвольного количества точек по строкам
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[" + k1 + "] == NULL (FurComplex)"); return; }
      
            zComplex[k2] = FurieN.BPF2_Line(zComplex[k1]);                 // Фурье преобразование для произвольного количества точек         
          
            Complex_pictureBox(k2);
        }

        private void FurComplex(int k1, int k2)                                           // Прямое Фурье преобразование для произвольного количества точек
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex["+ k1 + "] == NULL (FurComplex)"); return; }
            //int m = 1;
            //int n = zComplex[k1].width;
            //int nn = 2;
            //for (int i = 1; ; i++) { nn = nn * 2; if (nn > n) { n = nn / 2; m = i; break; } }
            //MessageBox.Show("n = " + Convert.ToString(n) + " m = " + Convert.ToString(m));

            //zComplex[k2] = new ZComplexDescriptor(n, n);

            //zComplex[k2] = Furie.FourierTransform(zComplex[k1], m);                        // Старое Фурье преобразование
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
                        zComplex[k2] = FurieN.BPF2(zComplex[k1]);                            // Фурье преобразование для произвольного количества точек
            sw.Stop();              
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);
            Complex_pictureBox(k2);
        }

 /*       private void FurComplexM(int k1, int k2)  // Обратное преобразование
        {

            if (zComplex[k1] == null) { MessageBox.Show("zComplex[0] == NULL"); return; }
            int m = 1;
            int n = zComplex[k1].width;
            int nn = 2;
            for (int i = 1; ; i++) { nn = nn * 2; if (nn > n) { n = nn / 2; m = i; break; } }

            MessageBox.Show("n = " + Convert.ToString(n) + " m = " + Convert.ToString(m));

            //zComplex[k2] = new ZComplexDescriptor(n, n);

            zComplex[k2] = Furie.InverseFourierTransform(zComplex[k1], m);

            Complex_pictureBox(k2);
        }
*/
        private void FurComplex_N()                                            // Прямое преобразование Фурье с четным числом точек
        {
            //MessageBox.Show("Прямое преобразование Фурье с произвольным числом точек");
            if (zArrayPicture == null) { MessageBox.Show("Главное окно пустое == NULL"); return; }

            int nx = zArrayPicture.width;
            int ny = zArrayPicture.height;
       
            ZComplexDescriptor zComplex_Picture = new ZComplexDescriptor(zArrayPicture);  // Re=zArrayPicture Im=0
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
                 zComplex_Picture = FurieN.BPF2(zComplex_Picture);              
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            MessageBox.Show("Время Минут: " + ts.Minutes + "   Время сек: " + ts.Seconds + "   Время миллисек: " + ts.Milliseconds);
          
           
            zArrayPicture = Furie.zAmplituda(zComplex_Picture);        // Выводится только амплитуда
            zArrayPicture = Furie.Invers_Double(zArrayPicture);        // Инверсия
           
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        private void FurComplex_CUDA()                                    // --------------------------------------------------Прямое преобразование Фурье (CUDA) из главного окна
        {
            //MessageBox.Show("Прямое преобразование Фурье с произвольным числом точек");
            if (zArrayPicture == null) { MessageBox.Show("Главное окно пустое == NULL"); return; }

            CUDA_FFT.Fur_CUDA(zArrayPicture);                             // FFT из главного окна

            zArrayPicture = Furie.Invers_Double(zArrayPicture);           // Инверсия
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);           // Визуализация

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }


        private void FurComplex_CUDA1(int k1, int k2)                                            // Прямое преобразование Фурье (CUDA) из zComplex[k1] -> zComplex[k2]
        {
            //MessageBox.Show("Прямое преобразование Фурье с произвольным числом точек");
            if (zComplex[k1] == null) { MessageBox.Show("zComplex[k1] == NULL"); return; }

            zComplex[k2] = CUDA_FFT.Fur_CUDA1(zComplex[k1]); 

            Complex_pictureBox(k2);
           
        }


        //   ---------------------------------------------------------------------------------------------------------------------
        /*      private void InversComplexM(int k1, int k2)      // Инверсия в центр zComplex[regComplex]
                {

                    if (zComplex[regComplex] == null) { MessageBox.Show("zComplex[regComplex] == NULL"); return; }

                    zComplex[regComplex] =  Furie.Invers(zComplex[regComplex]);

                    Complex_pictureBox(regComplex);
                }
         * */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                   ->    Инверсия в pictureBox01
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button6_Click(object sender, EventArgs e)
        {
            if (zArrayDescriptor[regImage] == null) { MessageBox.Show("zArrayDescriptor[" +regImage+"] == NULL"); return; }
            zArrayPicture = Furie.Invers_Double(zArrayDescriptor[regImage]);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            //zArrayPicture.Double_Picture(pictureBox01);                     // В основное окно zArrayPicture
            //currentScaleRatio = 1;
            applyScaleModeToPicturebox();
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Интерференция плоских волн     (Сложение с опорной волной)     InterForm.cs
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////      
        private void интерференцияПлоскихВолнToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            InterForm BoxForm = new InterForm();
            BoxForm.OnBox       += FormOn;
            BoxForm.OnBoxADD    += FormOnADD;        // Сложить с плоской волной
            BoxForm.OnBoxADD_I  += FormOnADD_I;      // Сложить с плоской волной и поместить интенсивность в центральное окно
            BoxForm.OnBoxSUB    += FormOnSUB;
            // BoxForm.OnBoxADD_Random += FormOnADD_Random;
            BoxForm.OnBoxMUL    += FormOnMUL;
            BoxForm.OnBoxPSI    += FormOnPSI;      // Сложение с фазовым сдвигом => 8,9,10,11
            BoxForm.OnBoxSUB1   += FormOnSUB1;
            BoxForm.OnBoxNoise  += FormOnNoise;
            BoxForm.OnBoxMove   += FormOnMove;    // <=  Из текущего окна в главное с мастабированием
            BoxForm.Show();

        }

        private void FormOnMove(double am)
        {
            if (zArrayDescriptor[regImage] == null) return;
            applyScaleModeToPicturebox();

            int nx = zArrayDescriptor[regImage].width;
            int ny = zArrayDescriptor[regImage].height;
            zArrayPicture = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    zArrayPicture.array[i, j] = zArrayDescriptor[regImage].array[i, j];
            zArrayPicture = SumClass.Range_Array1(zArrayPicture, am);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        // Сложить с плоской волной => интенсивность в центральное окно

        private void FormOnADD_I(double am, double AngleX, double AngleY, double Lambda, double dx, double noise, double fz) // Сложить с плоской волной + fz[0]
        {
            //int k1 = regComplex;
            if (zComplex[1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[k1] == NULL   FormOnADD_I"); return; }

            zArrayPicture = Model_interf.Model_pl_ADD_I(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormOnADD(double am, double AngleX, double AngleY, double Lambda, double dx, double noise, double fz) // Сложить с плоской волной + fz[0]
        {
            //int k1 = regComplex;
            if (zComplex[1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[k1] == NULL   FormOnADD"); return; }

            zComplex[2] = Model_interf.Model_pl_ADD_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz);
            Complex_pictureBox(2);

        }
        private void FormOnPSI(double am, double AngleX, double AngleY, double Lambda, double dx, double noise, double[] fz)
        {

            if (zComplex[1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[1] == NULL"); return; }
            zArrayDescriptor[8]  = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[0]);
            zArrayDescriptor[9]  = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[1]);
            zArrayDescriptor[10] = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[2]);
            zArrayDescriptor[11] = Model_interf.Model_pl_PSI(am, zComplex[1], AngleX, AngleY, Lambda, dx, noise, fz[3]);

         
            Vizual_regImage(8); Vizual_regImage(9); Vizual_regImage(10); Vizual_regImage(11);


            // PSI
            //zComplex[1] = ATAN_PSI.ATAN_891011(zArrayDescriptor,  progressBar1, fz, am);


            //int i_Complex = 1;
            //ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[i_Complex * 4 + 2]);      // Амплитуда из regComplex = 1
            //zComplex[1] = Furie.ATAN_891011(zArrayDescriptor, fz);
            //zComplex[1] = Furie.ATAN_OLD4(zArrayDescriptor, fz);  // Формула из книги
            //Complex_pictureBox(1);


        }

        private void FormOnNoise(double noise)      // Добавление шума к фазе
        {

            if (zComplex[regComplex] == null) { MessageBox.Show("Ошибка добавления шума к фазе: zComplex[k1] == NULL"); return; }
            zComplex[regComplex] = Model_interf.Model_Noise(noise, zComplex[regComplex]);
            Complex_pictureBox(regComplex);

        }

        private void FormOn(double am, double AngleX, double AngleY, double Lambda, int NX, int NY, double dx, int k1)
        {

            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_pl(am, AngleX, AngleY, Lambda, NX, NY, dx);
            Complex_pictureBox(k1);

        }

      
 /*       private void FormOnADD_Random(double am, double AngleX, double AngleY, double Lambda, double dx, int k1)
        {

            if (zComplex[k1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[k1] == NULL"); return; }
            zComplex[k1] = Model_interf.Model_pl_ADD_Random(am, zComplex[k1], AngleX, AngleY, Lambda, dx);
            Complex_pictureBox(k1);

        }
  * */
      
        private void FormOnMUL(double am, double AngleX, double AngleY, double Lambda, double dx, int k1)
        {

            if (zComplex[k1] == null) { MessageBox.Show("Сложение с плоской волной zComplex[k1] == NULL"); return; }
            zComplex[k1] = Model_interf.Model_pl_MUL(am, zComplex[k1], AngleX, AngleY, Lambda, dx);
            Complex_pictureBox(k1);

        }
       
        //   Устранение наклона
        private void FormOnSUB1(double AngleX, double AngleY)
        {
            if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL");  return; }
            if (zArrayDescriptor[regImage] == null) { MessageBox.Show("Вычитание наклона zArrayDescriptor[regImage] == NULL"); return; }

            zArrayPicture = Model_interf.Model_pl_SUB1(zArrayPicture, AngleX, AngleY);
            //Complex_pictureBox(k1);
            zArrayPicture.Double_Picture(pictureBox01);
        }
        // Вычитание фазы
        private void FormOnSUB(double am, double AngleX, double AngleY, double Lambda, double dx, int k1)
        {

            if (zComplex[k1] == null) { MessageBox.Show("Вычитание плоской волны zComplex[k1] == NULL"); return; }
            zComplex[k1] = Model_interf.Model_pl_SUB(am, zComplex[k1], AngleX, AngleY, Lambda, dx);
            Complex_pictureBox(k1);

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Интерференция сферических волн  
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void интерференцияСферическихВолнToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InterFormSph SpForm = new InterFormSph();
            SpForm.SpBox += Sphera;
            SpForm.SpBox1 += Sphera1;
            SpForm.SpBox2 += Sphera_SUB;
            //BoxForm.OnBoxADD += FormOnADD;
            //BoxForm.OnBoxMUL += FormOnMUL;
            SpForm.Show();
        }
        private void Sphera(double am, double Lambda, int NX, int NY, double dx, double dl, int k1)
        {
            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_Sp(am, Lambda, NX, NY, dx, dl);
            Complex_pictureBox(k1);

        }
        private void Sphera1(double am, double Lambda, int NX, int NY, double dx, double dl, int k1)
        {
            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_Sp1(am, Lambda, NX, NY, dx, dl);
            Complex_pictureBox(k1);

        }
        private void Sphera_SUB(double am, double Lambda, int NX, int NY, double dx, double dl, int k1)
        {
            zComplex[k1] = new ZComplexDescriptor(NX, NY);
            zComplex[k1] = Model_interf.Model_SpSUB(am, Lambda, NX, NY, dx, dl);
            Complex_pictureBox(k1);

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //           
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)  // -------------------------   Нижний движок
        {
            //vScrollBar2.Maximum = 255;
            //vScrollBar2.Value=50;
            //label33.Text = e.NewValue.ToString();
            //vScrollBar2.Update();
            //vScrollBar2.Show();

           // double max = Convert.ToDouble(textBox1.Text);    // Отображение pictureBox01
           // double min = Convert.ToDouble(textBox2.Text);
           // vScrollBar1.Maximum = Convert.ToInt32(max);
           // vScrollBar1.Minimum = Convert.ToInt32(min);

            int k = vScrollBar2.Maximum - e.NewValue;
           
            int k1 = Convert.ToInt32(textBox1.Text);   // Max
            if (k < k1) textBox2.Text = k.ToString();

           // double max = Convert.ToDouble(textBox1.Text);    // Отображение pictureBox01
           // double min = Convert.ToDouble(textBox2.Text);
           // SumClass.Range_Picture(pictureBox01, min, max);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)  // ----------------------- Верхний движок
        {
            //double max = Convert.ToDouble(textBox1.Text);    // Отображение pictureBox01
            //double min = Convert.ToDouble(textBox2.Text);
            //vScrollBar1.Maximum = Convert.ToInt32(max);
            //vScrollBar1.Minimum = Convert.ToInt32(min);
            

            int k = vScrollBar1.Maximum - e.NewValue;
           
            int k1 = Convert.ToInt32(textBox2.Text);  // Min
            if (k > k1) textBox1.Text = k.ToString();
           // textBox2.Text = e.NewValue.ToString();
            
            //SumClass.Range_Picture(pictureBox01, min, max);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //        Определение  Max Min zArrayPicture (основное окно)
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button5_Click(object sender, EventArgs e)
        {
            if (zArrayPicture == null)     { MessageBox.Show("zArrayPicture == NULL"); return; }
            if (zArrayPicture.width  == 0) { MessageBox.Show("zArrayPicture.width  = 0"); return; }
            if (zArrayPicture.height == 0) { MessageBox.Show("zArrayPicture.height = 0"); return; }
            double max, min;
            max = SumClass.getMax(zArrayPicture); textBox1.Text = Convert.ToString(max);
            min = SumClass.getMin(zArrayPicture); textBox2.Text = Convert.ToString(min);
            SumClass.Range_Picture(pictureBox01, zArrayPicture, min, max);
            //MessageBox.Show("min = " + Convert.ToString(min) + "max = " + Convert.ToString(max));

            //int BarMax = Convert.ToInt32(max - min);
            try
            {
                vScrollBar1.Maximum = Convert.ToInt32(max);
            }
            catch (System.OverflowException)
            {
                System.Console.WriteLine("Overflow in double to int conversion."); return;
            }


            try
            {
                vScrollBar1.Minimum = Convert.ToInt32(min);
            }
            catch (System.OverflowException)
            {
                System.Console.WriteLine("Overflow in double to int conversion."); return;
            }


           
            vScrollBar1.Value = Convert.ToInt32(min);
            vScrollBar1.Update();
            vScrollBar1.Show();

            vScrollBar2.Maximum = Convert.ToInt32(max);
            vScrollBar2.Minimum = Convert.ToInt32(min);
            vScrollBar2.Value = Convert.ToInt32(max);
            vScrollBar2.Update();
            vScrollBar2.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Range  (основное окно)  Приведение изображения к диапазону
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button4_Click(object sender, EventArgs e)
        {
            double max = Convert.ToDouble(textBox1.Text);
            double min = Convert.ToDouble(textBox2.Text);
            SumClass.Range_Picture(pictureBox01, zArrayPicture, min, max);
            //SumClass.Range_Picture(pictureBox01,  min, max);
            
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Shift  (основное окно)  Сдвиг по интенсивности
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void button7_Click(object sender, EventArgs e)
        {
            double s_ogr = Convert.ToDouble(textBox1.Text);
            double s_min = Convert.ToDouble(textBox2.Text);
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayPicture);
            zArrayPicture = SumClass.Shift_Picture(amp, s_ogr, s_min);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            Save  (основное окно)  Сдвиг по интенсивности
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       
        private void button8_Click(object sender, EventArgs e)
        {
            double max = Convert.ToDouble(textBox1.Text);
            double min = Convert.ToDouble(textBox2.Text);
            zArrayPicture = SumClass.Range_Array(zArrayPicture, min, max);
            SumClass.Range_Picture(pictureBox01, zArrayPicture, min, max);
            //zArrayPicture.Double_Picture(pictureBox01);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        


//----------------------------------------------------------------------------------------------------------------------------------
        private void Фильтр_Click(object sender, EventArgs e)
        {

        }
        // -------------------------------------------------------------------------------------------------------------------------
        //                     Модель объекта
        // 
        // -------------------------------------------------------------------------------------------------------------------------
        private void модельОбъектаToolStripMenuItem_Click(object sender, EventArgs e)  // Модель объекта
        {
            Model ModelForm = new Model();                   // Model.cs

            ModelForm.OnModel += FormModel;                  // Записать в zComplex[regComplex] модель после деформации

            ModelForm.OnInterf_Fr += FormModel_fr;           // Записать в zComplex[1] модель в области Френеля до    деформации,
                                                             // записать в zComplex[2] модель в области Френеля после деформации,
                                                             // сложить с плоским фронтом под углом ax,ay
           // ModelForm.OnInterf        += FormInterf;
            ModelForm.OnInterf2         += FormInterf2;        // Двойная экспозиция
           // ModelForm.OnInterf3       += FormInterf3;
            ModelForm.OnInterfPSI_Fr    += FormInterfPSI_Fr;   // 4 интерферограммы в 8,9,10,11
            ModelForm.OnInterf8PSI_Fr   += FormInterf8PSI_Fr;
            ModelForm.OnInterf_Cos      += FormInterf_Cos;     // Cos (k1-k2) => Главное окно
            ModelForm.OnInterf_Balka    += FormInterf_Balka;   // Моделирование прогиба балки

            ModelForm.Show(); 
        }



        private void FormInterf_Balka(double L, double Y, int N) // Cos (k1-k2) => Главное окно
        {
            Model_object.Model_Balka(zArrayDescriptor, L, Y, N);
            Vizual_regImage(0); Vizual_regImage(1); Vizual_regImage(2);
            Vizual_regImage(3); Vizual_regImage(4); Vizual_regImage(5);
            Vizual_regImage(6);


            Vizual_regImage(8); Vizual_regImage(9); Vizual_regImage(10); Vizual_regImage(11);
            //Complex_pictureBox(1); Complex_pictureBox(2);
        }
        private void FormInterf_Cos(double[] fz) // Cos (k1-k2) => Главное окно
        {
            Model_object.Model_Cos(zArrayDescriptor,  fz);
            Vizual_regImage(8);   Vizual_regImage(9);  Vizual_regImage(10);   Vizual_regImage(11);

            //Vizual_regImage(0); Vizual_regImage(1);
            //Complex_pictureBox(1); Complex_pictureBox(2);
        }


            private void FormModel_fr(double sdvg0, double sdvg, double noise, double lambda, double lm, double dx, double ax, double ay)
        {
            
           
            zComplex[1] = Model_object.Model_2(sdvg0, noise, lambda);           // До деформации
            zComplex[1] = FurieN.FrenelTransformN(zComplex[1], lambda, lm, dx);
            zComplex[2] = Model_object.Model_2(sdvg, noise, lambda);            // После деформации
            zComplex[2] = FurieN.FrenelTransformN(zComplex[2], lambda, lm, dx);
            Complex_pictureBox(1); Complex_pictureBox(2);

        }
 /*       private void FormInterf(double sdvg, double noise, double Lambda)  // Модель объекта (пластинки) Сложение волновых фронтов
        {
            //if (zArrayPicture == null) { MessageBox.Show("zArrayPicture == NULL"); return; }
            //if (zComplex[regComplex] == null) { MessageBox.Show("Моделирование объекта: zComplex[regComplex] == NULL"); return; }

            Model_object.Glgr_Interf(zComplex, zArrayDescriptor, sdvg, noise, Lambda);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            Complex_pictureBox(2);
            //zArrayPicture.Double_Picture(pictureBox01);
        }
*/       
        // Двойная экспозиция
        private void FormInterf2(double sdvg0 ,double sdvg, double noise, double Lambda, double lm, double dx, double AngleX, double AngleY)  
        {
 
            Model_object.Glgr_Interf2(zComplex, zArrayDescriptor, sdvg0, sdvg, noise, Lambda, lm, dx, AngleX, AngleY);

            //Complex_pictureBox(0);
            Complex_pictureBox(1);   // Отображение комплекного массива
            Complex_pictureBox(2);

            Vizual_regImage(0);      // Отображение реального массива
            Vizual_regImage(1);
            Vizual_regImage(2);

        }
/*        private void FormInterf3(double sdvg, double noise, double Lambda)  // Модель объекта (пластинки) Две голограммы
        {

            Model_object.Glgr_Interf3(zComplex, zArrayDescriptor, sdvg, noise, Lambda);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            zArrayDescriptor[8].Double_Picture(pictureBox9);
            zArrayDescriptor[9].Double_Picture(pictureBox10);
            //Complex_pictureBox(2);

        }
*/
        private void FormModel(double sdvg0, double sdvg, double noise, double Lambda)  // Модель объекта (пластинки)
        {
            MessageBox.Show("noise = " + noise);
            zComplex[0] = Model_object.Model_2(sdvg0, noise, Lambda);
            zComplex[1] = Model_object.Model_2(sdvg, noise, Lambda);
            Complex_pictureBox(0); Complex_pictureBox(1);            
        }
        /*
                private void FormInterfPSI(double sdvg0, double sdvg1, double noise, double Lambda, double dx, double[] fz)  // Модель объекта (пластинки) Двойная экспозиция Расшифровка PSI
                {

                    zArrayPicture = Model_object.Glgr_Interf_PSI(zComplex, zArrayDescriptor, sdvg0, sdvg1, noise, Lambda, dx, fz);
                    Complex_pictureBox(0);
                    Complex_pictureBox(1);
                    //Complex_pictureBox(2);
                    zArrayDescriptor[8].Double_Picture(pictureBox9);
                    zArrayDescriptor[9].Double_Picture(pictureBox10);
                    zArrayDescriptor[10].Double_Picture(pictureBox11);
                    zArrayDescriptor[11].Double_Picture(pictureBox12);

                    //Complex_pictureBox(regComplex);
                    zArrayPicture.Double_Picture(pictureBox01);
                }
         */
        // Модель объекта (пластинки) Расшифровка PSI
        private void FormInterfPSI_Fr(double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d, 
                                      double[] fz, double Ax, double Ay)  
        {

            Model_object.Glgr_Interf_PSI_Fr(zComplex, zArrayDescriptor, progressBar1, sdvg0, sdvg1, noise, Lambda, dx, d, fz, Ax, Ay);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            //Complex_pictureBox(2);

            Vizual_regImage(8); Vizual_regImage(9); Vizual_regImage(10); Vizual_regImage(11);
            
        }

        // Расшифровка PSI по новому алгоритму

        private void FormInterf8PSI_Fr(double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d, double[] fz, double Ax, double Ay) 
        {

            Model_object.Glgr_Interf8_PSI_Fr(zComplex, zArrayDescriptor, sdvg0, sdvg1, noise, Lambda, dx, d, fz, Ax, Ay);
            Complex_pictureBox(0);
            Complex_pictureBox(1);
            Complex_pictureBox(2);
           // Vizual_regImage(8);            Vizual_regImage(9);            Vizual_regImage(10);            Vizual_regImage(11);
        }
        // -------------------------------------------------------------------------------------------------------------------------
 /*       private void aDDToolStripMenuItem_Click(object sender, EventArgs e)  // zArrayPicture + zComplex[1] => zComplex[1]
        {

        }

        private void zArrayPictureZComplex1ToolStripMenuItem_Click(object sender, EventArgs e)  // zArrayPicture + zArrayDescriptor[regImage] => zArrayPicture
        {

        }
*/
      // private void aTANToolStripMenuItem_Click(object sender, EventArgs e)
      // {
           //
      // }

        private void PRMTRToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // -------------------------------------------------------------------------------------------------------------------------
        //  Моделирование интерференционных полос  и ATAN2
        // -------------------------------------------------------------------------------------------------------------------------
        private void моделированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Model_Sin ModelForm = new Model_Sin();
            //ModelForm.OnModelSin          += FormModel_Sin;             // Задается число полос
            //ModelForm.OnModelSin1         += FormModel_Sin1;            // Задается число точек в периоде
            //ModelForm.OnModelSin8         += FormModel_Sin8;
            ModelForm.OnModelWB           += FormModel_WB;
            ModelForm.OnModel_Dithering   += FormModel_Dithering;
            ModelForm.OnModel_DitheringVZ += FormModel_DitheringVZ;
            //ModelForm.OnModelAtan2        += FormModel_Atan2;
            //ModelForm.OnModel_Intensity    += FormModel_Intensity;             // Клин интенсивности
            //ModelForm.OnModel_Intensity_Line += FormModel_Intensity_Line;      // Вертикальные полосы

            ModelForm.OnModelExp           += FormModel_Exp;
            ModelForm.Show();

        }
        /// <summary>
        ///  Клин интенсивности 4096 х 2048
        /// </summary>
        /// <param name="nu"></param> Число градаций
        /// <param name="Nx"></param> Не используется
        /// <param name="Ny"></param> Не используется
        /// 
       // private void FormModel_Intensity(double nu, int Nx, int Ny, double gamma)       // Клин интенсивности
       // {
       //     zArrayDescriptor[regComplex * 4 + 0] = Model_Sinus.Intensity1(nu, Nx, Ny, gamma);
       //     zArrayDescriptor[regComplex * 4 + 1] = Model_Sinus.Intensity2(nu, Nx, Ny, gamma);
       //     zArrayDescriptor[regComplex * 4 + 2] = Model_Sinus.Intensity3(nu, Nx, Ny);
       //     zArrayDescriptor[regComplex * 4 + 3] = Model_Sinus.Intensity4(nu, Nx, Ny);

       //     for (int i = 0; i < 4; i++) Vizual_regImage(regComplex * 4 + i);
           
        //}
       // private void FormModel_Intensity_Line(int nl)                    // Вертикальные полосы
       // {
       //     zArrayDescriptor[regComplex * 4 + 0] = Model_Sinus.Intensity_Line(nl, zArrayDescriptor[regComplex * 4 + 0]);
       //     zArrayDescriptor[regComplex * 4 + 1] = Model_Sinus.Intensity_Line(nl, zArrayDescriptor[regComplex * 4 + 1]);
       //     zArrayDescriptor[regComplex * 4 + 2] = Model_Sinus.Intensity_Line1(nl, zArrayDescriptor[regComplex * 4 + 2]);
       //     zArrayDescriptor[regComplex * 4 + 3] = Model_Sinus.Intensity_Line1(nl, zArrayDescriptor[regComplex * 4 + 3]);

       //     for (int i = 0; i < 4; i++) Vizual_regImage(regComplex * 4 + i);
       // }



        private void FormModel_Exp(double g, int N)       // Модель exponent
        {
            zComplex[regComplex] = Model_Sinus.Exponenta(g,N);
            Complex_pictureBox(regComplex);
        }
       // private void FormModel_Sin(double[] fz, double amp, double gamma, double n_pol, int kr, int Nx, int Ny, double noise)       // Модель sin c фазовым сдвигом   kr - разреживание нулями
       // {
       //     for (int i = 0; i < 4; i++)
       //     {
       //         zArrayDescriptor[regComplex*4+i] = Model_Sinus.Sinus(fz[i], amp, n_pol, gamma, kr, Nx, Ny, noise);
       //         Vizual_regImage(regComplex * 4 + i);
       //     }         
       // }

        //private void FormModel_Sin1(double[] fz, int N_sdv, double amp, double gamma, double n_pol, int kr, int Nx, int Ny, double noise, double[] clin = null)       // Модель sin c фазовым сдвигом
        //{
         //   for (int i = 0; i < N_sdv; i++)
        //    {
        //        zArrayDescriptor[regComplex * 4 + i] = Model_Sinus.Sinus1(fz[i], amp, n_pol, gamma, kr, Nx, Ny, noise, clin);
        //        Vizual_regImage(regComplex * 4 + i);
        //    }
       // }

        //private void FormModel_Sin8(double[] fz, double amp, double gamma, double n_pol, int kr, int Nx, int Ny, double noise)       // Модель sin c фазовым сдвигом
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        zArray8[i] = Model_Sinus.Sinus1(fz[i], amp, n_pol, gamma, kr, Nx, Ny, noise);
         //   }
        //}

        private void FormModel_Dithering(double[] fz, double n_pol, int n_kvant, int n_urovn)       // Dithering
        {

            zArrayDescriptor[0] = Model_Sinus.Dithering(fz[0], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[0].Double_Picture(pictureBox1);


            zArrayDescriptor[1] = Model_Sinus.Dithering(fz[1], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[1].Double_Picture(pictureBox2);

            zArrayDescriptor[2] = Model_Sinus.Dithering(fz[2], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[2].Double_Picture(pictureBox3);

            zArrayDescriptor[3] = Model_Sinus.Dithering(fz[3], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[3].Double_Picture(pictureBox4);
        }

        private void FormModel_DitheringVZ(double[] fz, double n_pol, int n_kvant, int n_urovn)       // Dithering (Матрица возбуждения)
        {

            zArrayDescriptor[0] = Model_Sinus.DitheringVZ(fz[0], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[0].Double_Picture(pictureBox1);


            zArrayDescriptor[1] = Model_Sinus.DitheringVZ(fz[1], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[1].Double_Picture(pictureBox2);

            zArrayDescriptor[2] = Model_Sinus.DitheringVZ(fz[2], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[2].Double_Picture(pictureBox3);

            zArrayDescriptor[3] = Model_Sinus.DitheringVZ(fz[3], n_pol, n_kvant, n_urovn);
            zArrayDescriptor[3].Double_Picture(pictureBox4);
        }



        private void FormModel_WB(double[] fz, double gamma, double n_pol)       // Модель WB полос
        {

            zArrayDescriptor[0] = Model_Sinus.WB(fz[0], n_pol);
            zArrayDescriptor[0].Double_Picture(pictureBox1);


            zArrayDescriptor[1] = Model_Sinus.WB(fz[1], n_pol);
            zArrayDescriptor[1].Double_Picture(pictureBox2);

            zArrayDescriptor[2] = Model_Sinus.WB(fz[2], n_pol);
            zArrayDescriptor[2].Double_Picture(pictureBox3);

            zArrayDescriptor[3] = Model_Sinus.WB(fz[3], n_pol);
            zArrayDescriptor[3].Double_Picture(pictureBox4);
        }

      //  private void FormModel_Atan2(double[] fz)  // Atan2 bp 1,2,3,4 => zArrayPicture
      //  {
      //      zArrayPicture = FazaClass.ATAN_N(zArrayDescriptor, fz);
      //      Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            //zArrayPicture.Double_Picture(pictureBox01);
     //   }
        
        



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //           Моделирование фазы
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            

        private void моделированиеФазыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Model_Faza BoxFZ = new Model_Faza();
            BoxFZ.OnModelFz += ModelFaza;
            BoxFZ.OnModel_Sin_Fz += Model_Sin_Fz;   // c n1 и n2
            BoxFZ.OnModel_Sin_Fz3 += Model_Sin_Fz3; // c n1, n2 и n3
            BoxFZ.OnModelFzSub += ModelFazaSub;
            BoxFZ.OnModelFzSubA += ModelFazaSubA;
            BoxFZ.Show();
        }


        private void Model_Sin_Fz3(double n1, double n2, double n3, double n12, double n23, double noise, double minIntensity)  // 4 sin c фазовым сдвигом => atan
        {
            int Nx = 1024;
            int Ny = 1024;

            double[] fz = new double[4];
            fz[0] = Math.PI * 0 / 180.0; fz[1] = Math.PI * 90 / 180.0; fz[2] = Math.PI * 180 / 180.0; fz[3] = Math.PI * 270 / 180.0;  // Фазовый сдвиг в радианах  
                   
            zArrayDescriptor[8] = Model_Sinus.Sinus1(fz[0], 255, n1, 1, 0, Nx,  Ny, noise, minIntensity);   // 4 синусоиды с периодом n1
            zArrayDescriptor[9] = Model_Sinus.Sinus1(fz[1], 255, n1, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n1, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n1, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[0] = ATAN_PSI.ATAN(zArrayDescriptor, 2, fz);   // 8,9,10,11 ATAN2 с периодом n1  => zArrayDescriptor[0]

            zArrayDescriptor[8] = Model_Sinus.Sinus1(fz[0],  255, n2, 1, 0, Nx, Ny, noise, minIntensity);   // 4 синусоиды с периодом n2
            zArrayDescriptor[9] = Model_Sinus.Sinus1(fz[1],  255, n2, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n2, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n2, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[1] = ATAN_PSI.ATAN(zArrayDescriptor, 2, fz);   // 8,9,10,11 ATAN2 с периодом n2  => zArrayDescriptor[1]

            zArrayDescriptor[8]  = Model_Sinus.Sinus1(fz[0], 255, n3, 1, 0, Nx, Ny, noise, minIntensity);   // 4 синусоиды с периодом n2
            zArrayDescriptor[9]  = Model_Sinus.Sinus1(fz[1], 255, n3, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n3, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n3, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[2]  = ATAN_PSI.ATAN(zArrayDescriptor, 2, fz);   //8,9,10,11  ATAN2 с периодом n2  => zArrayDescriptor[2]

            zArrayDescriptor[3] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[0], zArrayDescriptor[1], noise);   // n12
            zArrayDescriptor[4] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[1], zArrayDescriptor[2], noise);   // n23

            zArrayDescriptor[5] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[4], zArrayDescriptor[3], noise);   // n123
            double n123 = n12 * n23 / (Math.Abs(n12 - n23));
            zArrayDescriptor[6] = Model_Sinus.Model_FAZA_T(zArrayDescriptor[5], zArrayDescriptor[4], n123, n23);  // Ступеньки
            //zArrayDescriptor[6] = Model_Sinus.Model_FAZA_T(zArrayDescriptor[5], zArrayDescriptor[0], n123, n1);

            zArrayDescriptor[7] = Model_Sinus.Model_FAZA_SUM(zArrayDescriptor[6], zArrayDescriptor[4], n23);      // Восстановленный профиль
            //zArrayDescriptor[7] = Model_Sinus.Model_FAZA_SUM(zArrayDescriptor[6], zArrayDescriptor[0], n1);

            zArrayDescriptor[8] = Model_Sinus.Model_FAZA_T(zArrayDescriptor[7], zArrayDescriptor[0], 1, n1);  // Ступеньки
            zArrayDescriptor[9] = Model_Sinus.Model_FAZA_SUM(zArrayDescriptor[8], zArrayDescriptor[0], n1);


            for (int i = 0; i < 12; i++) Vizual_regImage(i);
            

        }



        private void Model_Sin_Fz(double n1, double n2, double noise, double minIntensity)      // 4 sin c фазовым сдвигом => atan
        {
            int Nx = 1024;
            int Ny = 1024;
            double[] fz = new double[4];
            fz[0] = Math.PI *   0 / 180.0;                                // Фазовый сдвиг в радианах  
            fz[1] = Math.PI *  90 / 180.0;
            fz[2] = Math.PI * 180 / 180.0;
            fz[3] = Math.PI * 270 / 180.0;
            zArrayDescriptor[4]  = Model_Sinus.Sinus1(fz[0], 255, n1, 1, 0, Nx, Ny, noise, minIntensity);   // 4 синусоиды с периодом n1
            zArrayDescriptor[5]  = Model_Sinus.Sinus1(fz[1], 255, n1, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[6]  = Model_Sinus.Sinus1(fz[2], 255, n1, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[7]  = Model_Sinus.Sinus1(fz[3], 255, n1, 1, 0, Nx, Ny, noise, minIntensity);

           // for (int i = 0; i < 4; i++) fz[i] = fz[i] + Math.PI * 20 / 180.0;  // Сдвиг

            zArrayDescriptor[8]  = Model_Sinus.Sinus1(fz[0], 255, n2, 1, 0, Nx, Ny, noise, minIntensity);   // 4 синусоиды с периодом n2
            zArrayDescriptor[9]  = Model_Sinus.Sinus1(fz[1], 255, n2, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[10] = Model_Sinus.Sinus1(fz[2], 255, n2, 1, 0, Nx, Ny, noise, minIntensity);
            zArrayDescriptor[11] = Model_Sinus.Sinus1(fz[3], 255, n2, 1, 0, Nx, Ny, noise, minIntensity);
    

            zArrayDescriptor[0] = ATAN_PSI.ATAN(zArrayDescriptor, 1, fz); // 4,5,6,7
            zArrayDescriptor[1] = ATAN_PSI.ATAN(zArrayDescriptor, 2, fz); // 8,9,10,11

            zArrayDescriptor[2] = Model_Sinus.Model_FAZA_SUB(zArrayDescriptor[0], zArrayDescriptor[1]);
            zArrayDescriptor[3] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[0], zArrayDescriptor[1], noise);
            for (int i = 0; i < 1024; i++) for (int j = 0; j < 100; j++)   zArrayDescriptor[3].array[i, j] = zArrayDescriptor[0].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 100; j < 200; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[1].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 200; j < 300; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[2].array[i, j];

             zArrayDescriptor[4]= Model_Sinus.Model_FAZA_T1(zArrayDescriptor[3], n1, n2);
            
            for (int i = 0; i < 12; i++) Vizual_regImage(i);
           
        }

        private void ModelFaza(double n1, double n2, double noise, double minIntensity)      // Фаза пила 1024/n
        {
            zArrayDescriptor[0] = Model_Sinus.Model_FAZA(n1, noise);
            zArrayDescriptor[1] = Model_Sinus.Model_FAZA(n2, noise);
            zArrayDescriptor[2] = Model_Sinus.Model_FAZA_SUB(zArrayDescriptor[0], zArrayDescriptor[1]);
            zArrayDescriptor[3] = Model_Sinus.Model_FAZA_SUBN(zArrayDescriptor[0], zArrayDescriptor[1], noise);
            for (int i = 0; i < 1024; i++) for (int j =   0; j < 128; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[0].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 128; j < 256; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[1].array[i, j];
            for (int i = 0; i < 1024; i++) for (int j = 256; j < 380; j++) zArrayDescriptor[3].array[i, j] = zArrayDescriptor[2].array[i, j];

            Vizual_regImage(0);
            Vizual_regImage(1);
            Vizual_regImage(2);
            Vizual_regImage(3);
        }



        private void ModelFazaSub(int k1,int k2, int k3)      // Вычитание фаз
        {
            k1--; k2--; k3--;
            
            //MessageBox.Show("Эквивалентная длина волны " + k1 + " " + k2 + " " + k3);
            zArrayDescriptor[k1] = Model_Sinus.Model_FAZA_SUB(zArrayDescriptor[k2], zArrayDescriptor[k3]);
            Vizual_regImage(k1);
        }
        private void ModelFazaSubA(int k1, int k2, int k3)      // Вычитание фаз абсолютная
        {
            k1--; k2--; k3--;
            MessageBox.Show("Вычитание " + k1 + " " + k2 + " " + k3);

            zArrayDescriptor[k1] = Model_Sinus.Model_FAZA_SUBA(zArrayDescriptor[k2], zArrayDescriptor[k3]);
            Vizual_regImage( k1);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                       Фазовая развертка
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  

        private void разверткаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // MessageBox.Show("Фазовая развертка" );
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)   // Развертка по строкам
        {
            UnRupLine UnrupLine = new UnRupLine();
            UnrupLine.OnUnrupLine     += RupLine;
            UnrupLine.OnUnrupLinePlus += RupLinePluss;
            UnrupLine.OnUnrupLine2pi  += RupLine2pi;
            UnrupLine.OnUnrupLine2pi_L += RupLine2pi_L;
            UnrupLine.Show();
        }

        private void RupLine(int x0, double gr)
        {
            // MessageBox.Show("Фазовая развертка по строкам " +gr);
            zArrayPicture = Unrup.Unrup_LineSub(zArrayPicture, x0, gr);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void RupLinePluss(int x0, double gr)
        {
            // MessageBox.Show("Фазовая развертка по строкам " +gr);
            zArrayPicture = Unrup.Unrup_LinePluss(zArrayPicture,x0,  gr);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void RupLine2pi()
        {
            // MessageBox.Show("Фазовая развертка по строкам " +gr);
            zArrayPicture = Unrup.Unrup_Line2pi(zArrayPicture);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        //---
        private void RupLine2pi_L()
        {
            // MessageBox.Show("Фазовая развертка по строкам " +gr);
            zArrayPicture = Unrup.Unrup_Line2pi_L(zArrayPicture);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        //---------------------------------------------------------------------------------------
        private void разверткаToolStripMenuItem_Click(object sender, EventArgs e)     // Построение таблицы
        {
            UnrupForm UnrupForm = new UnrupForm();
            UnrupForm.OnUnrup              += FormUnrup;                     // Таблица со сдвигом
            UnrupForm.OnUnrup_Diag         += FormUnrup_Diag;
            UnrupForm.OnUnrup_Diag_Tab     += FormUnrup_Diag_Tab;
            UnrupForm.OnUnrup_Diag_Tab256  += FormUnrup_Diag_Tab256;
            UnrupForm.OnUnrup_Tab256       += FormUnrup_Tab256;
            UnrupForm.OnUnrup_2pi          += FormUnrup_2pi;
            UnrupForm.OnUnrup_Line         += FormUnrup_Line;
            UnrupForm.Show();
        }

        private void FormUnrup(PictureBox pictureBox1, int k1, int k2, int N1_sin, int N2_sin, int scale, int d, int sdvig)      
        {
            Unrup.Tabl_int(zArrayDescriptor, pictureBox1, k1, k2, N1_sin, N2_sin, scale, d, sdvig);
        }

        private void FormUnrup_Diag(PictureBox pictureBox1, int N1_sin, int N2_sin, int scale, int d) // Диагонали
        {
            Unrup.Tabl_Diag(zArrayDescriptor, pictureBox1,  N1_sin, N2_sin, scale, d);
        }

        private void FormUnrup_Diag_Tab(PictureBox pictureBox1, int N1_sin, int N2_sin, int scale, int d) // Данные по диагоналям
        {
            Unrup.Tabl_Diag_Tab(zArrayDescriptor, pictureBox1, N1_sin, N2_sin, scale, d);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------
        private void FormUnrup_Diag_Tab256(int N1_sin, int N2_sin,  int d)      // Данные (от 0 до 255) по диагоналям
        {
            zArrayPicture = Unrup.Tabl_Diag_Tab256(N1_sin, N2_sin, d);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormUnrup_Tab256(int b)
        {
            zArrayPicture = Unrup.Tabl256(zArrayDescriptor[2], zArrayDescriptor[0], zArrayDescriptor[1], b);                // Из 1 и 2 =>  zArrayPicture
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        private void FormUnrup_2pi(int b)  // Ручное устранение фазовой неоднозначности
        {
            zArrayDescriptor[regImage] = Unrup.Unrup_2pi(zArrayDescriptor[regImage], b);                // Из zArrayDescriptor[regImage]  =>  zArrayDescriptor[regImage]
            Vizual_regImage(regImage);
            //Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
      
        private void FormUnrup_Line()  // Вычитание тренда по двум точкам
        {

            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

         
            zArrayDescriptor[regImage] = Unrup.Unrup_Line(zArrayDescriptor[regImage], X1, Y1, X2, Y2);    // Из zArrayDescriptor[regImage]  =>  zArrayDescriptor[regImage]
            Vizual_regImage(regImage);
            //Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
        //-----------------------------------------------------------------------------------------------------------------
        private void разверткаToolStripMenuItem1_Click(object sender, EventArgs e)  // Развертка
        {
            UnrupForm1 UnrupForm1 = new UnrupForm1();
            UnrupForm1.OnUnrup1 += FormUnrup1;

            UnrupForm1.Show();

        }
        private void FormUnrup1(int k1, int k2, int k3, int N1_sin, int N2_sin, int N_diag, int sdvig, int Mngtl)
        {
            zArrayDescriptor[k3] = Unrup.Unrup_array(zArrayDescriptor, k1, k2, N1_sin, N2_sin, N_diag, sdvig, Mngtl);
            Vizual_regImage(k3);
        }
       
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //           Исправление интенсивности по фигуре лиссажу
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void лиссажуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lissagu LsgForm = new Lissagu();
            //LsgForm.On_Liss3D += FormLiss3D;
            LsgForm.Show();
        }
       // private void FormLiss3D(int N_line, int k1, int k2, int k3)
       // {
       //     //zArrayPicture = FazaClass.Lissagu3D(zArrayDescriptor, regComplex, N_line, k1, k2, k3);
       //     Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        //}
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //           PSI  из 8,9,10,11 в zArrayPicture fz[4]         Амплитуда Фаза Квантование
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void pSI4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PSI PSIForm = new PSI();
            PSIForm.OnPSI   += FormPSI;              // PSI амплитуда и фаза =>  (8,9,10,11)
            PSIForm.OnPSI1  += FormPSI1;             // PSI  фазы (regComplex) -> в Главное  окно (Обощенная формула)
            PSIForm.OnPSI_Carre += FormPSI_Carre;    // PSI Carre фазы (1,2,3,4) -> в Главное  окно

            PSIForm.OnPSI3 += FormPSI3;             // PSI  фазы (regComplex) -> в Главное  окно  
            PSIForm.OnPSI4 += FormPSI4;
            PSIForm.OnPSI5 += FormPSI5;                
            PSIForm.OnPSI6 += FormPSI6;
            PSIForm.OnPSI7 += FormPSI7;
            PSIForm.OnPSI8 += FormPSI8;

            PSIForm.OnIMAX  += FormIMAX;             // Квантование (8,9,10,11)
            PSIForm.OnIMAX1 += FormIMAX1;            // Квантование одного кадра
            //PSIForm.OnMaska += FormMaska;            // Наложение маски
            PSIForm.OnSdvg   += FormSdvg;              // Carre угол

            PSIForm.Show();
        }


        private void FormPSI1(double[] fz)
        {

            zArrayPicture = ATAN_PSI.ATAN(zArrayDescriptor, regComplex, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormPSI3(double[] fz)
        {

            zArrayPicture = ATAN_PSI.ATAN_3(zArrayDescriptor, regComplex, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormPSI4(double[] fz)
        {

            zArrayPicture = ATAN_PSI.ATAN_4(zArrayDescriptor, regComplex, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormPSI5(double[] fz)
        {

            zArrayPicture = ATAN_PSI.ATAN5(zArrayDescriptor, regComplex, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormPSI6(double[] fz)
        {

            zArrayPicture = ATAN_PSI.ATAN6(zArrayDescriptor, regComplex, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }
        private void FormPSI7(double[] fz)
        {
            zArrayPicture = ATAN_PSI.ATAN7(zArrayDescriptor, regComplex, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void FormPSI8(double[] fz)
        {
            zArrayPicture = ATAN_PSI.ATAN8(zArrayDescriptor, regComplex, fz);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void FormPSI_Carre()
        {
            zArrayPicture = ATAN_PSI.ATAN_Faza_Carre(zArrayDescriptor, regComplex, progressBar1);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void FormSdvg()                                     // Угол по формуле Carre
        {
            zArrayPicture = ATAN_PSI.ATAN_Sdvg(zArrayDescriptor, regComplex);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        private void FormPSI(double[] fz, double am)
        {
            zComplex[1] = ATAN_PSI.ATAN_891011(zArrayDescriptor, progressBar1, fz, am);
            Complex_pictureBox(1);
        }
      

        private void FormIMAX(int imax)
        {

            ATAN_PSI.Diapazon(zArrayDescriptor, imax);

            zArrayDescriptor[8].Double_Picture(pictureBox9);
            zArrayDescriptor[9].Double_Picture(pictureBox10);
            zArrayDescriptor[10].Double_Picture(pictureBox11);
            zArrayDescriptor[11].Double_Picture(pictureBox12);

        }

        private void FormIMAX1(int imax)
        {
            ATAN_PSI.Diapazon1(zArrayDescriptor, regImage, imax);
            Vizual_regImage(regImage);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

       
        private void интерферометрияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

/*		private void FormMaska(int k1, int k2, int k3)
        {
            zArrayDescriptor[k3] = ATAN_PSI.Maska(zArrayDescriptor, k1, k2);
            Vizual_regImage(k3);
        }
*/
//
//                  Устранение фона по трем точкам
//
        private void устраненияФонаПо3ТочкамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int X1 = Convert.ToInt32(textBox3.Text);
            int Y1 = Convert.ToInt32(textBox4.Text);

            int X2 = Convert.ToInt32(textBox5.Text);
            int Y2 = Convert.ToInt32(textBox6.Text);

            int X3 = Convert.ToInt32(textBox7.Text);
            int Y3 = Convert.ToInt32(textBox8.Text);

            int X4 = Convert.ToInt32(textBox9.Text);
            int Y4 = Convert.ToInt32(textBox10.Text);


            zArrayPicture = Rot.Sub_Plane(zArrayPicture, X1, Y1,  X2,  Y2, X3,  Y3);     
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void pictureBox01_Click(object sender, EventArgs e)
        {

        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageCamera();
        }


        private void ManageCamera()
        {
            CameraForm cameraForm = new CameraForm();
            cameraForm.PictureTaken += new PictureTakenHandler(HandleCameraPicture);
            cameraForm.LiveViewUpdated += new LiveViewUpdatedHandler(HandleLiveViewUpdate);
            cameraForm.MainForm = this;
            cameraForm.Show();
        }

        private void SetImage(Image image, PictureBox pictureBox)
        {
            pictureBox.Invoke
            (
                (MethodInvoker)delegate
                {
                    pictureBox.Image = image;
                    pictureBox.Refresh();
                }
            );
        }

        private void HandleCameraPicture(PictureTakenEventArgs eventArgs)
        {
            if (eventArgs == null) return;

            switch (eventArgs.Number)
            {
                case 0:
                    zArrayPicture = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                    SetImage(eventArgs.Image, pictureBox01);
                    break;
                
                default:
                    {
                        int index = GetPictureBoxIndex(eventArgs);
                        zArrayDescriptor[index] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        SetImage(eventArgs.Image, pictureBoxArray[index]);
                        break;
                    }
            }
        }
        
        private int GetPictureBoxIndex(PictureTakenEventArgs args)
        {
            return (args.StartImageNumber - 1) + (args.Number - 1);
            
            /*
            int index = 1;

            if (args.GroupNumber == 0)
            {
                index = args.Number - 1;
            }
            else
            {
                index = (args.GroupNumber - 1) * 4 + (args.Number - 1);
            }

            return index;
            */
        }

        private void HandleLiveViewUpdate(LiveViewUpdatedEventArgs eventArgs)
        {
            switch (eventArgs.PhaseShiftNumber)
            {
                case 1:
                    {
                        zArrayDescriptor[8] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        pictureBox9.Image = eventArgs.Image;
                        break;
                    }
                case 2:
                    {
                        zArrayDescriptor[9] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        pictureBox10.Image = eventArgs.Image;
                        break;
                    }
                case 3:
                    {
                        zArrayDescriptor[10] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        pictureBox11.Image = eventArgs.Image;
                        break;
                    }
                case 4:
                    {
                        zArrayDescriptor[11] = new ZArrayDescriptor(eventArgs.Image, eventArgs.ColorMode);
                        pictureBox12.Image = eventArgs.Image;
                        break;
                    }

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void фурьеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void фазаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void canon500DToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        
        public Image GetImageFromPictureBox(int number)
        {
            Image image = null;

            int index = number - 1;
            PictureBox picBox = pictureBoxArray[index];
            
            if (picBox != null)
            {
                image = picBox.Image;
            }
            return image;
        }

        public Image GetMainImageFromPictureBox()
        {
            Image image = null;
            PictureBox picBox = this.pictureBox01;
            if (picBox != null)
            {
                image = picBox.Image;
            }
            return image;
        }

        public Coords[] GetCoordinates()
        {
            Coords[] X = new Coords[4];

            X[0] = new Coords(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text));
            X[1] = new Coords(Convert.ToDouble(textBox5.Text), Convert.ToDouble(textBox6.Text));
            X[2] = new Coords(Convert.ToDouble(textBox7.Text), Convert.ToDouble(textBox8.Text));
            X[3] = new Coords(Convert.ToDouble(textBox9.Text), Convert.ToDouble(textBox10.Text));

            return X;
        }

        // ----------------------------------------------------- Структурированное освещение
        /// <summary>
        /// Коррекция неравномерности освещения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void коррекцияНеравномерностиОсвещенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CorrectBr CorI = new CorrectBr();
            CorI.MainForm = this; 
            CorI.On_CorrectX += CorX;          // Коррекция размера
            CorI.On_CorrectG += CorG;          // Коррекция клина
           // CorI.On_CorrectClin += CorClin;  // Коррекция клина
            CorI.On_CorrectSumm += CorSumm;    // Суммирование строк
         

            CorI.Show();
        }

       
        private void CorSumm(int k1,  int k2)                    // Сложение строк
        {
            if (zArrayDescriptor[k1] == null) { MessageBox.Show("CorG zArrayDescriptor[k1] == NULL"); return; }
            
            int w1 = zArrayDescriptor[k1].width;
            int h1 = zArrayDescriptor[k1].height;

            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            int y1 = Convert.ToInt32(textBox4.Text);
            int y2 = Convert.ToInt32(textBox6.Text);

            if (y2 < y1 || y2>h1 || y1<0 ) { MessageBox.Show("CorSumm y2< y1 || y2>h1"); return; }

            double[] array_line = new double[w1];

            for (int i = 0; i < w1; i++)
                for (int j = y1; j < y2; j++)
                    array_line[i] += zArrayDescriptor[k1].array[i, j];

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                    faza.array[i, j] = array_line[i] / (y2 - y1);

            zArrayDescriptor[k2] = faza;
            Vizual_regImage(k1);   Vizual_regImage(k2);

        }
/*
        private void CorClin(int I0, int nx, int ny, int k1)                    // Меняем размер массива
        {
            double gamma = 1;
           
            int nu = 255;                                               // Число уровней
            zArrayDescriptor[k1] = Model_Sinus.Intensity1(nu,  nx, ny, dx, gamma);
            Vizual_regImage(k1); 
        }
*/    



        private void CorG(int k1, int k2, int k3)                    // Меняем размер массива
        {
            if (zArrayDescriptor[k1] == null) { MessageBox.Show("CorG zArrayDescriptor[k1] == NULL"); return; }
            if (zArrayDescriptor[k2] == null) { MessageBox.Show("CorG zArrayDescriptor[k2] == NULL"); return; }
            int w1 = zArrayDescriptor[k1].width;
            int h1 = zArrayDescriptor[k1].height;


            ZArrayDescriptor faza = new ZArrayDescriptor(w1, h1);

            double max = double.MinValue;
            double min = double.MaxValue;

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {
                   double y1 = zArrayDescriptor[k1].array[i, j];
                   double y0 = zArrayDescriptor[k2].array[i, j];   // Клин
                   double y2 = 2 * y0 - y1;
                   if (y2 > 255) y2 = 255;
                   if (y2 < 0) y2 = -y2;
                    if (y2 > max) max = y2; if (y2 < min) min = y2;
                   faza.array[i, j]=y2;
                }

            MessageBox.Show("max = " + max+ " min = " + min);


            zArrayDescriptor[k3] = faza;
            Vizual_regImage(k1); Vizual_regImage(k2); Vizual_regImage(k3);
        }
        private void CorX(int k1, int k2, int n)                    // Меняем размер массива
        {
            if (zArrayDescriptor[k1] == null) { MessageBox.Show("zArrayDescriptor[k1] == NULL"); return; }
            int w1 = zArrayDescriptor[k1].width;
            int h1 = zArrayDescriptor[k1].height;

            int x1 = Convert.ToInt32(textBox3.Text);
            int x2 = Convert.ToInt32(textBox5.Text);

            if (x1>=x2 || x2 > w1-1) { MessageBox.Show("x1 >= x2 || x2 > w1-1"); return; }

            ZArrayDescriptor faza = new ZArrayDescriptor(n, h1);

            for (int i=0; i<n; i++)
                for (int j = 0; j < h1; j++)
                {
                    //int i1 = ((i - x1) * n) / (x2 - x1);
                    int i1 = i * (x2 - x1) / (n-1) + x1;
                    faza.array[i, j] = zArrayDescriptor[k1].array[i1,j];
                }
            zArrayDescriptor[k2] = faza;
            Vizual_regImage(k1); Vizual_regImage(k2);
        }

        /// <summary>
        /// Коррекция по 256 сдвигам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            Corr256 STR256 = new Corr256();
            STR256.Show();
        }
        private void моделированиеОстаткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Structur STRUCTUR = new Structur();
            STRUCTUR.On_Corr += Correct;                    // Скорректировать высоты
            STRUCTUR.On_CorrX += CorrectX;
            STRUCTUR.On_Scale += Correct_Scale;
            STRUCTUR.On_Sub += Correct_Sub;                 // Вычесть два массива 1-2 => ArrayPicture
            STRUCTUR.On_Sub_Cos += Correct_Sub_Cos;         // Вычесть два массива
            //STRUCTUR.On_Corr_Sub += Correct_Corr_Sub;       // Вычесть два массива с корректировкой значений по углу
            STRUCTUR.On_Sub_Line += Correct_Line;           // Вычесть линейный тренд 
            STRUCTUR.On_Count_Line += Correct_Count;
            STRUCTUR.On_Null += Correct_Null;              // Убрать нули
            STRUCTUR.Show();
        }

        private void Correct(double L, double d, double d1, double x_max)    // Корректировка высот
        {
            zArrayPicture = Model_object.Correct(zArrayPicture, L, d, d1, x_max);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        private void Correct_Null()                    // Убрать нули
        {
            zArrayPicture = Model_object.Count_Null(zArrayPicture);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        private void Correct_Count(int num)                    // Вычесть два массива
        {
            Model_object.Count_Line(zArrayPicture, num);

        }
        private void Correct_Line(int num)                    // Вычесть линейный тренд
        {
            zArrayPicture = Model_object.Sub_Line(zArrayPicture, num);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


        private void Correct_Corr_Sub(double L, double d, double d1, int x_max)
        {
            //zArrayDescriptor[2] = Model_object.Correct(zArrayDescriptor[0], L, d, d1, x_max);
            //zArrayDescriptor[3] = Model_object.Correct(zArrayDescriptor[1], L, d, d1, x_max);
            //zArrayPicture = SumClass.Sub_zArray(zArrayDescriptor[0], zArrayDescriptor[1]);
            zArrayDescriptor[2] = SumClass.Sub_zArray(zArrayDescriptor[0], zArrayDescriptor[1]);
            zArrayDescriptor[3] = Model_object.Correct(zArrayDescriptor[2], L, d, d1, x_max);
            Vizual_regImage(2); Vizual_regImage(3);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }
     
        private void Correct_Sub()                    // Вычесть два массива
        {
            zArrayPicture = SumClass.Sub_zArray(zArrayDescriptor[0], zArrayDescriptor[1]);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void Correct_Sub_Cos()              // Вычесть два массива с помощью Cos
        {
            zArrayPicture = Model_object.Sub_zArray_Сos(zArrayDescriptor[0], zArrayDescriptor[1]);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }

        private void Correct_Scale(double x, int n)
        {
            zArrayPicture = Model_object.Correct_Scale(zArrayPicture, x, n);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
        }


     

        private void CorrectX(double d1, double x_max)
        {
            zArrayPicture = Model_object.CorrectX(zArrayPicture, d1, x_max);
            Vizual.Vizual_Picture(zArrayPicture, pictureBox01);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.core = new Pain_t_Core(this.pictureBox01);
            this.core.FirstColor = Color.Black;
            this.core.SecondColor = Color.White;
            this.core.BrushColor = this.core.FirstColor;
            this.core.BrushWidth = 1;
            this.core.BindPipetteBufColor(true);
        }

        private void сохранитьТочкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCoordinates();
        }
        
        private void SaveCoordinates()
        {
            string x1 = textBox3.Text;
            string y1 = textBox4.Text;

            string x2 = textBox5.Text;
            string y2 = textBox6.Text;

            string x3 = textBox7.Text;
            string y3 = textBox8.Text;

            string x4 = textBox9.Text;
            string y4 = textBox10.Text;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "(*.txt)|*.txt";
            saveFileDialog.DefaultExt = "txt";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(FormatCoordinate(x1, y1));
                            sw.WriteLine(FormatCoordinate(x2, y2));
                            sw.WriteLine(FormatCoordinate(x3, y3));
                            sw.WriteLine(FormatCoordinate(x4, y4));

                            sw.Flush();
                        }
                    }
                }
            }
        }

        private string FormatCoordinate(string x, string y)
        {
            return string.Format("{0} {1}", x, y);
        }

        private void pSIToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void криваяПерекодированияToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            this.curvesGraph = new CurvesGraph();
            this.curvesGraph.ApplyCurveForRow += CurvesGraph_ApplyCurveForRow;             
            this.curvesGraph.ApplyCurve += CurvesGraph_ApplyCurve;
            this.curvesGraph.ApplyCurveAll += CurvesGraph_ApplyCurveAll;
            this.curvesGraph.ApplyPhaseDifferenceCalculationForRow += CurvesGraph_ApplyPhaseDifferenceCalculationForRow;
            this.curvesGraph.ApplyPhaseDifferenceCalculation += CurvesGraph_ApplyPhaseDifferenceCalculation;

            this.curvesAltGraphicForm = new Form();
            this.curvesGraphFormHost = new GraphFormHost();
            this.curvesAltGraphicForm.Width = 800;
            this.curvesAltGraphicForm.Height = 400;
            this.curvesGraphFormHost.Dock = DockStyle.Fill;
            this.curvesAltGraphicForm.Controls.Add(this.curvesGraphFormHost);

            this.phaseDifferenceAltGraphicForm = new Form();
            this.phaseDifferenceGraphFormHost = new GraphFormHost();
            this.phaseDifferenceAltGraphicForm.Width = 800;
            this.phaseDifferenceAltGraphicForm.Height = 400;
            this.phaseDifferenceGraphFormHost.Dock = DockStyle.Fill;
            this.phaseDifferenceAltGraphicForm.Controls.Add(this.phaseDifferenceGraphFormHost);
            
            this.curvesGraph.Show();

            this.curvesAltGraphicForm.Show();
            this.phaseDifferenceAltGraphicForm.Show();
        }

        private void CurvesGraph_ApplyPhaseDifferenceCalculationForRow(object sender, EventArgs e)
        {
            CurvesGraph curvesGraph = sender as CurvesGraph;
            if (sender != null)
            {
                int row = curvesGraph.RowNumber;
                int[] recodingArray = curvesGraph.GetRecodingArray();
                double[] phaseShifts = curvesGraph.GetPhaseShifts();

                double[][] transformedArray = new double[8][];

                int startImageIndex = 0;
                int endImageIndex = 7;

                int width = zArrayDescriptor[0].width;
                
                for (int k = startImageIndex; k <= endImageIndex; k++)
                {
                    transformedArray[k] = new double[width];

                    ZArrayDescriptor arrayDescr = zArrayDescriptor[k];
                    if (arrayDescr == null) continue;

                    for (int j = 0; j < width; j++)
                    {
                        int oldValue = Convert.ToInt32(arrayDescr.array[j, row]);
                        int newValue = recodingArray[oldValue];
                        transformedArray[k][j] = newValue;
                    }
                }

                //4 изображения - первое состояние (без нагрузки)
                int regComplex = 0;
                double[] array1 = ATAN_PSI.ATAN(transformedArray, regComplex, phaseShifts);

                //4 изображения - второе состояние (с нагрузкой)
                regComplex = 1;
                double[] array2 = ATAN_PSI.ATAN(transformedArray, regComplex, phaseShifts);

                int nx = array1.Length;

                double[][] resArray = new double[4][];

                for (int k = 0, index = 0; k < phaseShifts.Length; k++, index++)
                {
                    double[] result = new double[nx];
                    for (int i = 0; i < nx; i++)
                    {
                        result[i] = Math.Cos(array1[i] - array2[i] + phaseShifts[k]);
                    }

                    resArray[index] = result;
                }

                double[] finalResult = ATAN_PSI.ATAN(resArray, 0, phaseShifts);
                                                
                IList<GraphInfo> graphCollection = new List<GraphInfo>();

                Point2D[] graphPoints = new Point2D[zArrayPicture.width];
                for (int j = 0; j < finalResult.Length; j++)
                {
                    graphPoints[j] = new Point2D(j, finalResult[j]);
                }

                GraphInfo graphInfo = new GraphInfo("Graphic", System.Windows.Media.Colors.Black, graphPoints, true, false);
                graphCollection.Add(graphInfo);

                this.phaseDifferenceGraphFormHost.GraphInfoCollection = graphCollection;
            }
        }

        private void CurvesGraph_ApplyCurveForRow(object sender, EventArgs e)
        {
            CurvesGraph curvesGraph = sender as CurvesGraph;
            if (sender != null)
            {
                int row = curvesGraph.RowNumber;
                int width = zArrayPicture.width;

                int[] recodingArray = curvesGraph.GetRecodingArray();
                double[] resArray = new double[width];             
                
                for (int j = 0; j < width; j++)
                {
                    int oldValue = Convert.ToInt32(zArrayPictureOriginal.array[j, row]);
                    int newValue = recodingArray[oldValue];
                    resArray[j] = newValue;
                }

                IList<GraphInfo> graphCollection = new List<GraphInfo>();

                Point2D[] graphPoints = new Point2D[zArrayPicture.width];
                for (int j = 0; j < resArray.Length; j++)
                {
                    graphPoints[j] = new Point2D(j, resArray[j]);
                }

                GraphInfo graphInfo = new GraphInfo("Graphic", System.Windows.Media.Colors.Black, graphPoints, true, false);
                graphCollection.Add(graphInfo);

                this.curvesGraphFormHost.GraphInfoCollection = graphCollection;
            }
        }

        private void CurvesGraph_ApplyPhaseDifferenceCalculation(object sender, EventArgs e)
        {
            CurvesGraph curvesGraph = sender as CurvesGraph;
            if (sender != null)
            {
                int[] recodingArray = curvesGraph.GetRecodingArray();
                double[] phaseShifts = curvesGraph.GetPhaseShifts();

                ZArrayDescriptor[] transformedArray = new ZArrayDescriptor[12];

                int startImageIndex = 0;
                int endImageIndex = 7;

                int width = zArrayDescriptor[0].width;
                int height = zArrayDescriptor[0].height;
                                             
                for (int k = startImageIndex; k <= endImageIndex; k++)
                {
                    transformedArray[k] = new ZArrayDescriptor(width, height);

                    ZArrayDescriptor arrayDescr = zArrayDescriptor[k];
                    if (arrayDescr == null) continue;
                                        
                    for (int j = 0; j < width; j++)
                    {
                        for (int i = 0; i < height; i++)
                        {
                            int oldValue = Convert.ToInt32(arrayDescr.array[j, i]);
                            int newValue = recodingArray[oldValue];
                            transformedArray[k].array[j, i] = newValue;
                        }
                    }
                }
                

                //4 изображени - первое состояние (без нагрузки)
                int regComplex = 0;
                ZArrayDescriptor zArray1 = ATAN_PSI.ATAN(transformedArray, regComplex, phaseShifts);

                //4 изображения - второе состояние (с нагрузкой)
                regComplex = 1;
                ZArrayDescriptor zArray2 = ATAN_PSI.ATAN(transformedArray, regComplex, phaseShifts);
                
                int nx = zArray1.width;
                int ny = zArray1.height;
                                
                for (int k = 0, imageIndex = 8; k < phaseShifts.Length; k++, imageIndex++)
                {
                    ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);
                    for (int i = 0; i < nx; i++)
                    {
                        for (int j = 0; j < ny; j++)
                        {
                            rez.array[i, j] = Math.Cos(zArray1.array[i, j] - zArray2.array[i, j] + phaseShifts[k]);
                        }
                    }

                    zArrayDescriptor[imageIndex] = rez;
                    Vizual.Vizual_Picture(zArrayDescriptor[imageIndex], pictureBoxArray[imageIndex]);
                }
            }
        }

        private void CurvesGraph_ApplyCurveAll(object sender, EventArgs e)
        {
            CurvesGraph curvesGraph = sender as CurvesGraph;
            if (sender != null)
            {
                int[] recodingArray = curvesGraph.GetRecodingArray();
                int startImageIndex = curvesGraph.GetStartImageNumber() - 1;
                int endImageIndex = curvesGraph.GetEndImageNumber() - 1;

                for (int k = startImageIndex; k <= endImageIndex; k++)
                {
                    ZArrayDescriptor arrayDescr = zArrayDescriptor[k];
                    if (arrayDescr == null) continue;

                    int width = arrayDescr.width;
                    int height = arrayDescr.height;

                    for (int j = 0; j < width; j++)
                    {
                        for (int i = 0; i < height; i++)
                        {
                            int oldValue = Convert.ToInt32(arrayDescr.array[j, i]);
                            int newValue = recodingArray[oldValue];
                            arrayDescr.array[j, i] = newValue;
                        }
                    }

                    Vizual.Vizual_Picture(arrayDescr, pictureBoxArray[k]);
                }
            }
            
        }

        private void CurvesGraph_ApplyCurve(object sender, EventArgs e)
        {
            CurvesGraph curvesGraph = sender as CurvesGraph;
            if (sender != null)
            {
                int[] recodingArray = curvesGraph.GetRecodingArray();

                int width = zArrayPicture.width;
                int height = zArrayPicture.height;

                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        int oldValue = Convert.ToInt32(zArrayPictureOriginal.array[j, i]);
                        int newValue = recodingArray[oldValue];
                        zArrayPicture.array[j, i] = newValue;
                    }
                }

                Vizual.Vizual_Picture(zArrayPicture, pictureBox01);
            }
        }

        private void fileMakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileMaker fileMaker = new FileMaker();
            fileMaker.Show();
        }
/// <summary>
/// Теорема Котельникова
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void теоремаКотельниковаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Teorema1 Teor1 = new Teorema1();
            Teor1.Show();
        }

        private void extensionTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RangeExtensionModelForm form = new RangeExtensionModelForm();
            form.Show();
        }

       

        private void LoadCoordinates()
        {
            string x1, y1;
            string x2, y2;
            string x3, y3;
            string x4, y4;
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.txt)|*.txt";
            openFileDialog.DefaultExt = "txt";

            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            ExtractCoordinates(sr.ReadLine(), out x1, out y1);
                            ExtractCoordinates(sr.ReadLine(), out x2, out y2);
                            ExtractCoordinates(sr.ReadLine(), out x3, out y3);
                            ExtractCoordinates(sr.ReadLine(), out x4, out y4);
                        }
                    }

                    textBox3.Text = x1;
                    textBox4.Text = y1;

                    textBox5.Text = x2;
                    textBox6.Text = y2;

                    textBox7.Text = x3;
                    textBox8.Text = y3;

                    textBox9.Text = x4;
                    textBox10.Text = y4;
                }
            }
        }

        private void убратьГеометрическиеИскаженияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor inputArray = zArrayPicture;
            double angleInDegrees = 85;

            ZArrayDescriptor resArray = EliminateGeometricDistorsion(inputArray, angleInDegrees);
            zArrayPicture = resArray;
            Vizual.Vizual_Picture(resArray, pictureBox01);
        }

      

        private void ExtractCoordinates(string formattedCoordinate, out string x, out string y)
        {
            x = null;
            y = null;

            if (!string.IsNullOrEmpty(formattedCoordinate))
            {
                string[] xyArray = formattedCoordinate.Split(' ', '\t');
                if (xyArray != null && xyArray.Length == 2)
                {
                    x = xyArray[0];
                    y = xyArray[1];
                }
            }
        }

        private void загрузитьТочкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadCoordinates();
        }

        private void коррекцияГаммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int width = zArrayPicture.width;
            int height = zArrayPicture.height;

            ZArrayDescriptor resArrayDescriptor = new ZArrayDescriptor(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    resArrayDescriptor.array[x, y] = GammaCorrection.GetCorrectedValue(zArrayPicture.array[x, y]);
                }
            }

            zArrayPicture = resArrayDescriptor;
            Vizual.Vizual_Picture(resArrayDescriptor, pictureBox01);
        }

        private void fastTakePhotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FastTakePhoto();    
        }

        private void FastTakePhoto()
        {
            CameraController cameraController = new CameraController(CanonSdkProvider.GetSDKHandler(), this);
            cameraController.PictureTaken += new PictureTakenHandler(HandleCameraPicture);

            short fromImageNumber = 1;
            short toImageNumber = 2;

            cameraController.FastTakePhoto(fromImageNumber, toImageNumber);
            cameraController.Dispose();
        }

        private void FastTakePhoto(short fromImageNumber, short toImageNumber)
        {
            CameraController cameraController = new CameraController(CanonSdkProvider.GetSDKHandler(), this);
            cameraController.PictureTaken += new PictureTakenHandler(HandleCameraPicture);

            cameraController.FastTakePhoto(fromImageNumber, toImageNumber);
            cameraController.Dispose();
        }

        private ZArrayDescriptor EliminateGeometricDistorsion(ZArrayDescriptor inputArray, double angleInDegrees)
        {
            int w = inputArray.width;
            int h = inputArray.height;

            double angleInRadians = Math.PI * angleInDegrees / 180;
            int maxs = 0;

            for (int j = w - 1; j >= 0; j--)
            {
                int k = w - 1 - j;
                int s = Convert.ToInt32(Math.Cos(angleInRadians) * k);
                if (s > maxs)
                {
                    maxs = s;
                }
            }

            ZArrayDescriptor resArray = new ZArrayDescriptor(maxs + 1, h);
            
            for (int j = w - 1; j >= 0; j--)
            {
                int k = w - 1 - j;
                int s = Convert.ToInt32(Math.Cos(angleInRadians) * k);
                for(int y = 0; y < h; y++)
                {
                    resArray.array[s, y] = inputArray.array[k, y];
                }
            }

            return resArray; 
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
