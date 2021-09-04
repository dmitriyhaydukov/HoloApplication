using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;


namespace rab1
{
   [Serializable()]
    public class ZComplexDescriptor
    {

        public Complex[,] array;
        public int width;
        public int height;

        public ZComplexDescriptor()
        {
           
        }

        public ZComplexDescriptor(int width1, int height1)   
        {
            width = width1;
            height = height1;
            array = new Complex[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] =  new Complex(0.0, 0.0);
                }
            }
        }

        public ZComplexDescriptor(Complex[,] descriptorToCopy)
        {
            if (descriptorToCopy == null)
            {
                return;
            }

            width = descriptorToCopy.GetLength(0); 
            height = descriptorToCopy.GetLength(1);;
            array = new Complex[width, height];
           
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] = descriptorToCopy[i, j];
                }
            }
        }



        public ZComplexDescriptor(ZComplexDescriptor descriptorToCopy)
        {
            if (descriptorToCopy == null)
            {
                return;
            }

            array = new Complex[descriptorToCopy.width, descriptorToCopy.height];
            width = descriptorToCopy.width;
            height = descriptorToCopy.height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] = descriptorToCopy.array[i, j];
                }
            }
        }

        // Конструктор для заполнения амплитуды , фаза остается

        public ZComplexDescriptor(ZComplexDescriptor zc, ZArrayDescriptor ampl)
        {
            if (ampl == null) { MessageBox.Show("ZComplexDescriptor == NULL"); return; }

            width  = zc.width;
            height = zc.height;
            array = new Complex[width, height];

            //double max = SumClass.getMax(descriptorToCopy);
            //double min = SumClass.getMin(descriptorToCopy);

            //MessageBox.Show("width =" + Convert.ToString(width) + "height =" + Convert.ToString(height));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                   
                    array[i, j] = Complex.FromPolarCoordinates(ampl.array[i,j], zc.array[i,j].Phase);                   
                }
            }
        }




        // Конструктор для заполнения фазы (Амплитуда постоянная)

        public ZComplexDescriptor(ZArrayDescriptor descriptorToCopy, double am)
        {
            if (descriptorToCopy == null)  { MessageBox.Show("ZComplexDescriptor == NULL"); return; }
           
            width  = descriptorToCopy.width;
            height = descriptorToCopy.height;
            array  = new Complex[width, height];

            double max = SumClass.getMax(descriptorToCopy);
            double min = SumClass.getMin(descriptorToCopy);

            //MessageBox.Show("width =" + Convert.ToString(width) + "height =" + Convert.ToString(height));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    double a = descriptorToCopy.array[i, j];
                    a = (a-min)*2.0*Math.PI / (max - min);
                    a = a - Math.PI;
                    array[i, j] = Complex.FromPolarCoordinates(am, a );

                    //array[i, j] = new Complex(1.0, descriptorToCopy.array[i, j]); 
                }
            }
        }

        // Конструктор для заполнения амплитуды и фазы)

        public ZComplexDescriptor(ZArrayDescriptor phase, ZArrayDescriptor  amp)
        { 
            if (amp == null)   { MessageBox.Show("Ampl  (ZComplexDescriptor)  == NULL"); return; }
            if (phase == null) { MessageBox.Show("Phase (ZComplexDescriptor)  == NULL"); return; }

            width = amp.width;
            height = amp.height;

            int NX = phase.width;
            int NY = phase.height;

            if (NX > width)  NX = width;
            if (NY > height) NY = height;
            if (NX < width)  width=NX;
            if (NY < height) height=NY;

            array = new Complex[width, height];

            //double max = SumClass.getMax(phase);
            //double min = SumClass.getMin(phase);         
            //MessageBox.Show("width =" + Convert.ToString(width) + "height =" + Convert.ToString(height));

            for (int i = 0; i < NX; i++)
            {
                for (int j = 0; j < NY; j++)
                {
                   
                    array[i, j] = Complex.FromPolarCoordinates(amp.array[i, j], phase.array[i, j]);                 
                }
            }
        }

        // Конструктор для случайного заполнения фазы (RANDOM) (Амплитуда из файла, k=1)

        public ZComplexDescriptor(ZArrayDescriptor descriptorToCopy, ZArrayDescriptor amp, int k)
        {
            if (amp == null)                { MessageBox.Show("Ampl == NULL");               return; }
            if (descriptorToCopy == null)   { MessageBox.Show("ZArrayDescriptor   == NULL"); return; }
            if (k != 1)                     { MessageBox.Show(" k!=1 ");                     return; }

            width  = amp.width;
            height = amp.height;
            array  = new Complex[width, height];

            double max = SumClass.getMax(descriptorToCopy);
            double min = SumClass.getMin(descriptorToCopy);

            int NX = descriptorToCopy.width;
            int NY = descriptorToCopy.height;

            if (NX > width) NX = width;
            if (NY > height) NY = height;
            //MessageBox.Show("width =" + Convert.ToString(width) + "height =" + Convert.ToString(height));
            Random rnd = new Random();
            for (int i = 0; i < NX; i++)
            {
                for (int j = 0; j < NY; j++)
                {
                    double am = amp.array[i, j];
                    double fz  = descriptorToCopy.array[i, j];
                    double fa = rnd.NextDouble() * 2.0 * Math.PI - Math.PI;
                    fz = fz + fa;
                    //if (fz > Math.PI) fz = fz - Math.PI;
                    //if (fz < -Math.PI) fz = fz + Math.PI;
                    if (am <= 0) fz = 0;                              // По шаблону
                    //if (a > 0) a = rnd.NextDouble() *  Math.PI ; else a = 0;
                    array[i, j] = Complex.FromPolarCoordinates(am, fz);

                   
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Конструктор для заполения реальной части и мнимой части 
        // k=0   Re
        // k=1   Im
        // k=2   re im=0
        // value.Real, value.Imaginary
        public ZComplexDescriptor(ZArrayDescriptor descriptorToCopy, ZComplexDescriptor a, int k)
        {
            if (descriptorToCopy == null)    { MessageBox.Show("ZArrayDescriptor == NULL"); return; }
            if (a == null) { MessageBox.Show("ZComplexDescriptor == NULL"); return; } 

            width = a.width;
            height = a.height;

            int w1 =  descriptorToCopy.width;
            int h1 = descriptorToCopy.height;
            MessageBox.Show("k= " +k + "width= " + width+ "height = " + height + "w1 = " + w1 + "h1 = " + h1);


            if (w1 > width || h1 > height) { MessageBox.Show("ZComplexDescriptor.cs ZArrayDescriptor > ZComplexDescriptor"); return; }

            int x1 = (width  - w1) / 2;
            int y1 = (height - h1) / 2;

            array = new Complex[width, height];
            //MessageBox.Show("width =" + Convert.ToString(width) + "height =" + Convert.ToString(height));

            for (int i1 = 0; i1 < w1; i1++)
            {
                for (int j1 = 0; j1 < h1; j1++)
                {
                    int i = i1 + x1;
                    int j = j1 + y1;
                    //Double c = descriptorToCopy.array[i, j];
                    if (k == 0) array[i, j] = new Complex(descriptorToCopy.array[i1, j1], a.array[i1, j1].Imaginary);
                    if (k != 0) array[i, j] = new Complex(a.array[i1, j1].Real, descriptorToCopy.array[i1, j1]); 
                }
            }
           
        }

        // Конструктор для заполнения реальной части и мнимой части = 0

        public ZComplexDescriptor(ZArrayDescriptor descriptorToCopy)
        {
            if (descriptorToCopy == null) { MessageBox.Show("ZArrayDescriptor == NULL"); return; }
            width = descriptorToCopy.width;
            height = descriptorToCopy.height;
            array = new Complex[width, height];

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++) { array[i, j] = new Complex(descriptorToCopy.array[i, j], 0.0); }
                }
                
        }

        public static void Complex_ArrayDescriptor(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, int regComplex)     // Из zComplex в zArrayDescriptor
        {
            if (zComplex[regComplex] == null) { MessageBox.Show("ZComplexDescriptor: Complex_ArrayDescriptor:  zComplex[regComplex] == NULL"); return; }
 
            int width  = zComplex[regComplex].width;
            int height = zComplex[regComplex].height;

            double[,] Image_double = new double[width, height];

            Image_double = Furie.Re(zComplex[regComplex].array);
            zArrayDescriptor[regComplex * 4 + 0] = new ZArrayDescriptor(Image_double);

            Image_double = Furie.Im(zComplex[regComplex].array);
            zArrayDescriptor[regComplex * 4 + 1] = new ZArrayDescriptor(Image_double);
           
            Image_double = Furie.Amplituda(zComplex[regComplex].array);
            zArrayDescriptor[regComplex * 4 + 2] = new ZArrayDescriptor(Image_double);          

            Image_double = Furie.Faza(zComplex[regComplex].array);
            zArrayDescriptor[regComplex * 4 + 3] = new ZArrayDescriptor(Image_double);

        }

  /*      public static Complex[,] Complex_Array(ZComplexDescriptor[] zComplex)     // Из zComplex в zArrayDescriptor
        {
            Complex[,] Array1 = new Complex[zComplex.width, zComplex.height]; 


            return Array1;

        }
*/
        }
}
