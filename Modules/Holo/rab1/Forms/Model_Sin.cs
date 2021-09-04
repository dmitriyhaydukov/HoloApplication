using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//public delegate void ModelSin(double[] fz);
public delegate void ModelSinG(double[] fz, double gamma, double N_pol);
public delegate void ModelSinG_kr(double[] fz, double N_urovn, double gamma, double N_pol, int k, int Nx, int Ny, double noise);
public delegate void ModelSinG_Picture(double[] fz, int N, double N_urovn, double gamma, double N_pol, int k, int Nx, int Ny, double noise, double[] clin = null);
public delegate void ModelSinD(double[] fz, double N_pol, int kvant, int N_urovn);
public delegate void ModelExp(double g, int N);
//public delegate void Model_I(double nu, int nx, int ny, double gamma);
//public delegate void Model_IL(int nl);


namespace rab1.Forms
{

    public partial class Model_Sin : Form
    {
        #region Constants

        private const int WEDGE_MIN_INTENSITY_VALUE = 60;
        private const int WEDGE_MAX_INTENSITY_VALUE = 255;
        
        //M1 < M2 (For calculation wedge ratio)
        private const int WEDGE_M1 = 167;
        private const int WEDGE_M2 = 211;

        private const int WEDGE_WIDTH = 850;

        private const int IMAGE_WIDTH = 4096;
        private const int IMAGE_HEIGHT = 2160;

        private const int BLACK_SIDE_WIDTH = 50;

        private readonly double WEDGE_RATIO = Convert.ToDouble(WEDGE_M1) / Convert.ToDouble(WEDGE_M2);

        #endregion

        public delegate void VisualRegImageDelegate(int k);
        public static VisualRegImageDelegate VisualRegImage = null;    // Визуализация одного кадра от 0 до 11 из main
        public static VisualRegImageDelegate VisualRegImageAsRaw = null;
        
       // public event ModelSinG_kr OnModelSin;
       // public event ModelSinG_Picture OnModelSin1;
       // public event ModelSinG_kr OnModelSin8;
        public event ModelSinG    OnModelWB;
        public event ModelSinD    OnModel_Dithering;
        public event ModelSinD    OnModel_DitheringVZ;
        //public event ModelSin     OnModelAtan2;
        //public event ModelSin     OnModelAtan2_L;
        public event ModelExp     OnModelExp;
        //public event Model_I      OnModel_Intensity;
        //public event Model_IL     OnModel_Intensity_Line;  // Вертикальные полосы
        

        private static double[] fz = { 0.0, 90.0, 180.0, 270.0, 0.0, 90.0, 180.0, 270.0 };
        private static int N_sdv = 4;
        private static double gamma = 1;
        private static double N_pol = 40;                // 40 точек на полосу
        private static int    N_kvant = 2;
        private static double    N_urovn = 255;          // Амплитуда
        private static int    kr = 0;                    // Разрядить нулями (0 - не разряжать, 1 - через 1)
        private static int    Nx = 4096;                 // Размер массива
        private static int    Ny = 2160;                 // Размер массива
        private static double noise = 0;               // Шум от амплитуды

        private double[] clin = { 35, 50, 58, 65, 72, 78, 85, 94, 100, 108, 118, 132, 149, 168, 192, 255 };  // Клин для исправления нелинейности

        public Model_Sin()
        {
            InitializeComponent();
            textBox1.Text     = Convert.ToString(fz[0]); textBox2.Text = Convert.ToString(fz[1]); textBox3.Text = Convert.ToString(fz[2]); textBox4.Text = Convert.ToString(fz[3]);
            textBox5.Text     = Convert.ToString(gamma);
            textBox6.Text     = Convert.ToString(N_pol);
            textBox7.Text     = Convert.ToString(N_kvant);
            textBox8.Text     = Convert.ToString(N_urovn);
            textBox9.Text     = Convert.ToString(kr);
            textBox10.Text    = Convert.ToString(Nx);
            textBox12.Text    = Convert.ToString(Ny);
            textBox11.Text    = Convert.ToString(noise);
            textBox17.Text    = Convert.ToString(N_sdv);
        }

        private void button1_Click(object sender, EventArgs e)                             // Смоделировать 4(8) синусоиды с N_pol полосами
        {
            double[] fzrad = new double[8];

           // fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
           // fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
           // fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
           // fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            gamma = Convert.ToDouble(textBox5.Text);
            N_pol = Convert.ToDouble(textBox6.Text);
            N_urovn = Convert.ToDouble(textBox8.Text);   // Амплитуда
            kr = Convert.ToInt32(textBox9.Text);
            Nx = Convert.ToInt32(textBox10.Text);
            Ny = Convert.ToInt32(textBox12.Text);
            noise = Convert.ToDouble(textBox11.Text);
            N_sdv = Convert.ToInt32(textBox17.Text);   // Число сдвигов

            if (N_sdv > 8) MessageBox.Show("Число сдвигов больше 8", "Message", MessageBoxButtons.OK);

            fzrad[0] = Math.PI * Convert.ToDouble(textBox1.Text) / 180.0;   // Фаза в радианах  
            fzrad[1] = Math.PI * Convert.ToDouble(textBox2.Text) / 180.0;
            fzrad[2] = Math.PI * Convert.ToDouble(textBox3.Text) / 180.0;
            fzrad[3] = Math.PI * Convert.ToDouble(textBox4.Text) / 180.0;

            fzrad[4] = Math.PI * Convert.ToDouble(textBox13.Text) / 180.0;
            fzrad[5] = Math.PI * Convert.ToDouble(textBox14.Text) / 180.0;
            fzrad[6] = Math.PI * Convert.ToDouble(textBox15.Text) / 180.0;
            fzrad[7] = Math.PI * Convert.ToDouble(textBox16.Text) / 180.0;

            for (int i = 0; i < N_sdv; i++)
            {
                Form1.zArrayDescriptor[Form1.regComplex * 4 + i] = Model_Sinus.Sinus(fzrad[i], N_urovn, N_pol, gamma, kr, Nx, Ny, noise);
                VisualRegImage(Form1.regComplex * 4 + i);
            }
            Close();
            //OnModelSin(fzrad, N_urovn, gamma, N_pol, kr, Nx, Ny, noise);
           
            Close();
        }

        private void button8_Click(object sender, EventArgs e)                         // Смоделировать 4(8) синусоиды с размером периода N_pol
        {
            double[] fzrad = new double[8]; 

            gamma       = Convert.ToDouble(textBox5.Text);
            N_pol       = Convert.ToDouble(textBox6.Text);   // Число точек на полоссу
            N_urovn     = Convert.ToDouble(textBox8.Text);   // Амплитуда
            kr          = Convert.ToInt32(textBox9.Text);    // Разрядка нулями
            Nx          = Convert.ToInt32(textBox10.Text);
            Ny          = Convert.ToInt32(textBox12.Text);
            noise       = Convert.ToDouble(textBox11.Text);
            N_sdv      = Convert.ToInt32(textBox17.Text);   // Число сдвигов

            double minIntensity = Convert.ToInt32(textBoxMinIntensity.Text);

            if (N_sdv > 8) MessageBox.Show("Число сдвигов больше 8", "Message", MessageBoxButtons.OK);

            
                fzrad = new double[8];

                fzrad[0] = Math.PI * Convert.ToDouble(textBox1.Text) / 180.0;   // Фаза в радианах  
                fzrad[1] = Math.PI * Convert.ToDouble(textBox2.Text) / 180.0;
                fzrad[2] = Math.PI * Convert.ToDouble(textBox3.Text) / 180.0;
                fzrad[3] = Math.PI * Convert.ToDouble(textBox4.Text) / 180.0;

                fzrad[4] = Math.PI * Convert.ToDouble(textBox13.Text) / 180.0;
                fzrad[5] = Math.PI * Convert.ToDouble(textBox14.Text) / 180.0;
                fzrad[6] = Math.PI * Convert.ToDouble(textBox15.Text) / 180.0;
                fzrad[7] = Math.PI * Convert.ToDouble(textBox16.Text) / 180.0;

            //OnModelSin1(fzrad, N_sdv, N_urovn, gamma, N_pol, kr, Nx, Ny, noise, null);
            for (int i = 0; i < N_sdv; i++)
               {
                  Form1.zArrayDescriptor[Form1.regComplex * 4 + i] = Model_Sinus.Sinus1(fzrad[i], N_urovn, N_pol, gamma, kr, Nx, Ny, noise, minIntensity, null);
                  VisualRegImageAsRaw(Form1.regComplex * 4 + i);
               }
            Close();
          
        }
      /// <summary>
      /// Сверхразрешение.
      /// Синусоида в два раза больше. 
      /// 1.Усредняется по двум точкам.
      /// 2.Сдвиг на 1 точку
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
           
            N_pol = Convert.ToDouble(textBox6.Text);   // Число точек на полоссу
            N_urovn = Convert.ToDouble(textBox8.Text);   // Амплитуда
          
            Nx = Convert.ToInt32(textBox10.Text);
            Ny = Convert.ToInt32(textBox12.Text);
          

           Form1.zArrayDescriptor[Form1.regComplex] = Model_Sinus.Sinus3(N_pol, Nx, Ny);
           VisualRegImageAsRaw(Form1.regComplex);
            
            Close();

        }


        /// <summary>
        /// Синусоиды с 256 фазовыми сдвигами
        /// Число повторений в "Разрядить нулями" => kr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            
            N_pol = Convert.ToDouble(textBox6.Text);   // Число точек на полоссу
          
            kr = Convert.ToInt32(textBox9.Text);    // Разрядка нулями
            Nx = Convert.ToInt32(textBox10.Text);
          
           
            //if (kr == 0) { MessageBox.Show("Число повторений равно 0"); return; }
         
            
                Form1.zArrayDescriptor[Form1.regComplex * 4 ] = Model_Sinus.Sinus2(N_pol, kr,  Nx);
              
                VisualRegImage(Form1.regComplex * 4 );
           
            Close();
        }
        /// <summary>
        /// Синусоиды с учетом клина
        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelSinByClinButton_Click(object sender, EventArgs e)
        {
            double[] fzrad = new double[8];

            gamma = Convert.ToDouble(textBox5.Text);
            N_pol = Convert.ToDouble(textBox6.Text);     // Число точек на полоссу
            N_urovn = Convert.ToDouble(textBox8.Text);   // Амплитуда
            kr = Convert.ToInt32(textBox9.Text);         // Разрядка нулями
            Nx = Convert.ToInt32(textBox10.Text);
            Ny = Convert.ToInt32(textBox12.Text);
            noise = Convert.ToDouble(textBox11.Text);
            N_sdv = Convert.ToInt32(textBox17.Text);     // Число сдвигов

            double minIntensity = 0;

            if (N_sdv > 8) MessageBox.Show("Число сдвигов больше 8", "Message", MessageBoxButtons.OK);
          

            fzrad = new double[8];

            fzrad[0] = Math.PI * Convert.ToDouble(textBox1.Text) / 180.0;   // Фаза в радианах  
            fzrad[1] = Math.PI * Convert.ToDouble(textBox2.Text) / 180.0;
            fzrad[2] = Math.PI * Convert.ToDouble(textBox3.Text) / 180.0;
            fzrad[3] = Math.PI * Convert.ToDouble(textBox4.Text) / 180.0;

            fzrad[4] = Math.PI * Convert.ToDouble(textBox13.Text) / 180.0;
            fzrad[5] = Math.PI * Convert.ToDouble(textBox14.Text) / 180.0;
            fzrad[6] = Math.PI * Convert.ToDouble(textBox15.Text) / 180.0;
            fzrad[7] = Math.PI * Convert.ToDouble(textBox16.Text) / 180.0;

            CorrectBr correctBr = new CorrectBr();
            double[] interpolatedClin = correctBr.InterpolateClin(clin);

            //OnModelSin1(fzrad, N_sdv, N_urovn, gamma, N_pol, kr, Nx, Ny, noise, interpolatedClin);
            for (int i = 0; i < N_sdv; i++)
            {     
               Form1.zArrayDescriptor[Form1.regComplex * 4 + i] = Model_Sinus.Sinus1(fzrad[i], N_urovn, N_pol, gamma, kr, Nx, Ny, noise, minIntensity, interpolatedClin);
               VisualRegImage(Form1.regComplex * 4 + i);
            }
            Close();
        }
        /// <summary>
        /// Загрузка клина из файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadClinButton_Click(object sender, EventArgs e)
        {
            LoadWedge();
        }

        private void LoadWedge()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.txt)|*.txt";
            openFileDialog.DefaultExt = "txt";

            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    List<double> valuesList = new List<double>();
                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            while (!sr.EndOfStream)
                            {
                                string stringValue = sr.ReadLine();
                                if (!string.IsNullOrEmpty(stringValue))
                                {
                                    valuesList.Add(double.Parse(stringValue));
                                }
                            }
                        }
                    }

                    clin = valuesList.ToArray();
                    MessageBox.Show("Клин загружен (cl)");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)     // В текущий комплексный массив exp(-iw)
        {
            Nx = Convert.ToInt32(textBox10.Text);
            gamma = Convert.ToDouble(textBox5.Text);
            OnModelExp(gamma, Nx);
            Close();
        }
        private void button4_Click(object sender, EventArgs e)    // Смоделировать 4 черно-белые
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;
            gamma = Convert.ToDouble(textBox5.Text);
            N_pol = Convert.ToDouble(textBox6.Text);

            OnModelWB(fzrad, gamma, N_pol);
            Close();
        }

        private void button5_Click(object sender, EventArgs e)  // Смоделировать 4 dithering (Алгоритм Флойда-Стейнберга)
        {
            double[] fzrad = new double[4];
            fz[0]   = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1]   = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2]   = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3]   = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;
          
            N_pol   = Convert.ToDouble(textBox6.Text);
            N_kvant = Convert.ToInt32(textBox7.Text);
            N_urovn = Convert.ToDouble(textBox8.Text);
            int amp = (int)N_urovn;
            OnModel_Dithering(fzrad,  N_pol, N_kvant,  amp);
            Close();
        }

        private void button6_Click(object sender, EventArgs e) // Смоделировать 4 dithering (Матрица возбуждения)
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            N_pol = Convert.ToDouble(textBox6.Text);
            N_kvant = Convert.ToInt32(textBox7.Text);
            N_urovn = Convert.ToDouble(textBox8.Text);
            int amp = (int)N_urovn;
            OnModel_DitheringVZ(fzrad, N_pol, N_kvant, amp);
            Close();
        }

       


 /*       private void button2_Click(object sender, EventArgs e)                       //   ATAN2
        {
            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox1.Text); fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox2.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox3.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox4.Text); fzrad[3] = Math.PI * fz[3] / 180.0;

            OnModelAtan2(fzrad);
            Close();

        }
*/
        private void button3_Click(object sender, EventArgs e)   // Задание сдвигов
        {
            N_sdv = Convert.ToInt32(textBox17.Text);   // Число сдвигов
            double n = 360.0 / N_sdv;

            textBox1.Text = Convert.ToString(0);
            textBox2.Text = Convert.ToString(n);
            textBox3.Text = Convert.ToString(2*n);
            textBox4.Text = Convert.ToString(3*n);

            textBox13.Text = Convert.ToString(4*n);
            textBox14.Text = Convert.ToString(5*n);
            textBox15.Text = Convert.ToString(6*n);
            textBox16.Text = Convert.ToString(7*n); 
        }

        private void button100_Click(object sender, EventArgs e)
        {
            SetCustomShifts();
        }

        private void SetCustomShifts()
        {
            //double[] phaseShiftsInRadians = new double[] 
            //    { 0, Math.PI, Math.PI / 3, -(Math.PI / 3) };
            //double[] phaseShiftsInRadians = new double[] 
            //    { 3 * Math.PI / 2, 5 * Math.PI / 2, 11 * Math.PI / 6, 7 * Math.PI / 6 };

            //double[] phaseShiftsInRadians = new double[] 
            //    { 0, Math.PI, Math.PI / 3, 5 * Math.PI / 3, Math.PI, 2 * Math.PI, 4 * Math.PI / 3, 2 * Math.PI / 3 };

            double[] phaseShiftsInRadians = new double[]
                { Math.PI / 2, 3 * Math.PI / 2, 5 * Math.PI / 6, 13 * Math.PI / 6, 3 * Math.PI / 2, 5 * Math.PI / 2, 11 * Math.PI / 6, 7 * Math.PI / 6 };


            double[] phaseShiftsInDegrees = new double[phaseShiftsInRadians.Length];

            for (int k = 0; k < phaseShiftsInRadians.Length; k++)
            {
                double valueInRadians = phaseShiftsInRadians[k];
                double valueInDegrees = valueInRadians * 180 / Math.PI;
                phaseShiftsInDegrees[k] = valueInDegrees;
            }
            
            if (phaseShiftsInDegrees.Length == 4)
            {
                textBox1.Text = phaseShiftsInDegrees[0].ToString();
                textBox2.Text = phaseShiftsInDegrees[1].ToString();
                textBox3.Text = phaseShiftsInDegrees[2].ToString();
                textBox4.Text = phaseShiftsInDegrees[3].ToString();
            }

            if (phaseShiftsInDegrees.Length == 8)
            {
                textBox1.Text = phaseShiftsInDegrees[0].ToString();
                textBox2.Text = phaseShiftsInDegrees[1].ToString();
                textBox3.Text = phaseShiftsInDegrees[2].ToString();
                textBox4.Text = phaseShiftsInDegrees[3].ToString();

                textBox13.Text = phaseShiftsInDegrees[4].ToString();
                textBox14.Text = phaseShiftsInDegrees[5].ToString();
                textBox15.Text = phaseShiftsInDegrees[6].ToString();
                textBox16.Text = phaseShiftsInDegrees[7].ToString();
            }
        }
                
        private void GenerateWedgeOne()
        {
            int width = WEDGE_WIDTH;
            int imageHeight = IMAGE_HEIGHT;
            int imageWidth = IMAGE_WIDTH;

            ZArrayDescriptor arrayDescriptor = GenerateWedge(WEDGE_M1, width, imageHeight, imageWidth, WEDGE_RATIO);
            Form1.zArrayDescriptor[0] = arrayDescriptor;
            VisualRegImage(0);
        }

        private void GenerateWedgeTwo()
        {
            int width = WEDGE_WIDTH;
            int imageHeight = IMAGE_HEIGHT;
            int imageWidth = IMAGE_WIDTH;

            ZArrayDescriptor arrayDescriptor = GenerateWedge(WEDGE_M2, width, imageHeight, imageWidth, 1);
            Form1.zArrayDescriptor[1] = arrayDescriptor;
            VisualRegImage(1);
        }
        
        private ZArrayDescriptor GenerateWedge(int mValue, int width, int imageHeight, int imageWidth, double? mRatio = null)
        {
            ZArrayDescriptor arrayDescriptor = new ZArrayDescriptor(imageWidth + BLACK_SIDE_WIDTH, imageHeight);

            double maxIntensity = WEDGE_MAX_INTENSITY_VALUE;
            if (mRatio.HasValue)
            {
                maxIntensity = mRatio.Value * (WEDGE_MAX_INTENSITY_VALUE - WEDGE_MIN_INTENSITY_VALUE) + WEDGE_MIN_INTENSITY_VALUE;
            }

            Interval<double> interval1 = new Interval<double>(0, mValue - 1);
            Interval<double> interval2 = new Interval<double>(WEDGE_MIN_INTENSITY_VALUE, maxIntensity);

            RealIntervalTransform intervalTransform = new RealIntervalTransform(interval1, interval2);

            int[] array = new int[width];
            int currentValue = 0;

            //Create wedge 0..m
            for (int j = 0; j < width; j++)
            {
                if (currentValue >= mValue - 1)
                {
                    currentValue = 0;
                }
                array[j] = currentValue;
                currentValue++;
            }

            CorrectBr correctBr = new CorrectBr();
            double[] interpolatedClin = correctBr.InterpolateClin(clin);

            int qq = Convert.ToInt32(maxIntensity);

            //Stretch wedge to image width
            int k = imageWidth / (array.Length) + 1;
            for (int x = 0; x < imageWidth - 1; x++)
            {
                int i = x / k;
                for (int y = 0; y < imageHeight - 1; y++)
                {
                    double value = intervalTransform.TransformToFinishIntervalValue(array[i]);
                    //double correctedValue = CorrectBr.CorrectValueByClin(value, interpolatedClin, qq);
                    double correctedValue = value;
                    arrayDescriptor.array[x, y] = correctedValue;
                }
            }

            return arrayDescriptor;
        }

        private void ShowGraphic(IList<GraphInfo> graphCollection)
        {
            GraphFormHost graphFormHost = new GraphFormHost();
            graphFormHost.GraphInfoCollection = graphCollection;

            Form form = new Form();
            form.Height = 300;
            form.Width = 900;
            graphFormHost.Dock = DockStyle.Fill;
            form.Controls.Add(graphFormHost);
            form.Show();
        }
        
        /*
        private void makeDecisionTableButton_Click(object sender, EventArgs e)
        {
            MakeDecisionTableForWedge();
        }
        */
        private void generateWedgeOneButton_Click(object sender, EventArgs e)
        {
            GenerateWedgeOne();
        }

        private void generateWedgeTwoButton_Click(object sender, EventArgs e)
        {
            GenerateWedgeTwo();
        }

        private void CalcWedgeIntensityDistributionButton_Click(object sender, EventArgs e)
        {
            CalcWedgeIntensityDistribution();
        }

        private void CalcWedgeIntensityDistribution()
        {
            ZArrayDescriptor arrayDescriptor1 = Form1.zArrayDescriptor[0];
            ZArrayDescriptor arrayDescriptor2 = Form1.zArrayDescriptor[1];

            Interval<double> intervalInt = new Interval<double>(0, 225);
            Interval<double> intervalM1 = new Interval<double>(0, WEDGE_M1);
            Interval<double> intervalM2 = new Interval<double>(0, WEDGE_M2);

            RealIntervalTransform intervalTransformM1 = new RealIntervalTransform(intervalInt, intervalM1);
            RealIntervalTransform intervalTransformM2 = new RealIntervalTransform(intervalInt, intervalM2);

            int width = arrayDescriptor1.width;

            int startY = 0;
            int height = 2;

            List<Point2D> pointsList = new List<Point2D>();
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = startY; y < height - 1; y++)
                {
                    double intensity1 = arrayDescriptor1.array[x, y];
                    double intensity2 = arrayDescriptor2.array[x, y];

                    intensity1 = intervalTransformM1.TransformToFinishIntervalValue(intensity1);
                    intensity2 = intervalTransformM2.TransformToFinishIntervalValue(intensity2);

                    int b1 = Convert.ToInt32(intensity1);
                    int b2 = Convert.ToInt32(intensity2);

                    pointsList.Add(new Point2D(b1, b2));
                }
            }

            RangeExtensionModelForm decisionTableForm = new RangeExtensionModelForm();

            IList<Point2D> decisionTablePointsList = decisionTableForm.BuildTable(WEDGE_M1, WEDGE_M2, WEDGE_WIDTH);

            GraphInfo idealGraphInfo =
                new GraphInfo("Ideal graph", System.Windows.Media.Colors.Green, decisionTablePointsList.ToArray(), true, false);
            
            GraphInfo graph = new GraphInfo("Graphic", System.Windows.Media.Colors.Red, pointsList.ToArray(), false, true);
            IList<GraphInfo> graphCollection = new List<GraphInfo>() { idealGraphInfo, graph };

            ShowGraphic(graphCollection);
        }

       
        /*
        private void button2_Click(object sender, EventArgs e)
        {
            N_urovn = Convert.ToDouble(textBox8.Text);   // Число уровней
            Nx = Convert.ToInt32(textBox10.Text);
            Ny = Convert.ToInt32(textBox12.Text);
            gamma = Convert.ToDouble(textBox5.Text);
            OnModel_Intensity(N_urovn, Nx, Ny, gamma);

            Close();
        }
        */

        //       private void button9_Click(object sender, EventArgs e) // Полосы поверх изображения
        //        {
        //            int N_p = Convert.ToInt32(textBox6.Text);
        //           OnModel_Intensity_Line(N_p);

        //            Close();
        //       }
    }
}
