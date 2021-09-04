using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;
using System.Diagnostics;

/// Программы формы ADD_Cmplx - Арифметические операции над массивами

namespace rab1
{
    public delegate void VisualRegImageDelegate(int k);

    class ADD_Math
    {
        public static VisualRegImageDelegate VisualRegImage      = null;  // Визуализация одного кадра от 0 до 11
        public static VisualRegImageDelegate ComplexPictureImage = null;  // Визуализация одного комплексного массива ( от 0 до 2)


        /// <summary>
        /// Линейный коэффициент корреляции r-Пирсона между двумя массивами
        /// </summary>
        /// <param name="k1"></param> Номер 1 массива (от 1 до 12 перуводится в диапазон от 0 до 11)
        /// <param name="k2"></param> Номер 2 массива

        public static void Pirs_D(int k1, int k2)             // Линейный коэффициент корреляции r-Пирсона между двумя массивами
        {
            k1--; k2--;                                   // Массив 1 ->  0
            //MessageBox.Show("k1=  " + k1 + " k2=  " + k2);
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("Pirs_D zArrayDescriptor[" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("Pirs_D zArrayDescriptor[" + k2 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k2].height;

            int nx1 = nx-1;
            int ny1 = ny - 1;
            // MessageBox.Show("Pirs_D");

            double s1 = 0;
            double s2 = 0;
            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    s1 += Form1.zArrayDescriptor[k1].array[i, j];
                    s2 += Form1.zArrayDescriptor[k2].array[i, j];
                }
            s1 = s1 / (nx1 * ny1); s2 = s2 / (nx1 * ny1);  // Среднее значение
            //MessageBox.Show("s1 = " + s1);

            double r;
            double ch1 = 0;
            double zn1 = 0;
            double zn2 = 0;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double c1 = Form1.zArrayDescriptor[k1].array[i, j] - s1;
                    double c2 = Form1.zArrayDescriptor[k2].array[i, j] - s2;
                    ch1 += c1 * c2;
                    zn1 += c1 * c1; zn2 += c2 * c2;
                }

            r = ch1 / Math.Sqrt(zn1 * zn2);
            double gr = Math.Acos(r) * 180 / Math.PI;
            MessageBox.Show("Pirs_D Среднее значение s1 = " + s1 + " s2 = " + s2 + "\n Коэффициент корреляции r = " + r + "\n В градусах = " + gr);
        }

        public static void ROR_D(int k1)             //  сдвиг вправо zArrayDescriptor[regImage]
        {

            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROR_D  Form1.zComplex[Form1.regImage] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i + k1, j] = Form1.zArrayDescriptor[Form1.regImage].array[i, j];   

            //  for (int i = 0; i < k1; i++)     // Циклический сдвиг
            //      for (int j = 0; j < ny; j++)
            //          zArray.array[i, j] = Form1.zArrayPicture.array[nx + i - k1, j];

              for (int i = 0; i < k1; i++)     // Не циклический сдвиг
                  for (int j = 0; j < ny; j++)
                      zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[ i, j];

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zArrayDescriptor[Form1.regImage].array[i, j] = zArray.array[i, j];

            VisualRegImage(Form1.regImage); 
           
        }

        public static void ROL_D(int k1)             // Cдвиг влево zArrayDescriptor[regImage]
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROL_D  Form1.zArrayDescriptor[Form1.regImage]== NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;
            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[k1 + i, j];

            //  for (int i = k1; i > 0; i--)               // Циклический сдвиг
            //      for (int j = 0; j < ny; j++)
            //          zArray.array[nx - i, j] = Form1.zArrayPicture.array[k1 - i, j];

            for (int i = k1; i > 0; i--)               // Не циклический сдвиг
                  for (int j = 0; j < ny; j++)
                      zArray.array[nx - i, j] = Form1.zArrayDescriptor[Form1.regImage].array[nx - i, j];

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zArrayDescriptor[Form1.regImage].array[i, j] = zArray.array[i, j];
            VisualRegImage(Form1.regImage);
        }



        public static void ROR_C(int k1)             // Циклический сдвиг вправо  комплексных чисел
        {
            
            if (Form1.zComplex[Form1.regComplex] == null) { MessageBox.Show("ROR_C:  Form1.zComplex[Form1.regImage] == NULL"); return; }

            int nx = Form1.zComplex[Form1.regComplex].width;
            int ny = Form1.zComplex[Form1.regComplex].height;

            ZComplexDescriptor zArray = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i + k1, j] = Form1.zComplex[Form1.regComplex].array[i, j];

              for (int i = 0; i < k1; i++)         // Циклический сдвиг
                  for (int j = 0; j < ny; j++)
                      zArray.array[i, j] = Form1.zComplex[Form1.regComplex].array[nx + i - k1, j];

            for (int i = 0; i < nx; i++)                                        // Переписываем результат
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[Form1.regComplex].array[i, j] = zArray.array[i, j];
            //VisualRegImage(Form1.regImage);
            ComplexPictureImage(Form1.regComplex);
        }


        public static void ROL_C(int k1)             // Циклический сдвиг влево комплексных чисел
        {
            if (Form1.zComplex[Form1.regComplex] == null) { MessageBox.Show("ROR_C:  zComplex[Form1.regImage] == NULL"); return; }

            int nx = Form1.zComplex[Form1.regComplex].width;
            int ny = Form1.zComplex[Form1.regComplex].height;

            ZComplexDescriptor zArray = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx - k1; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zComplex[Form1.regComplex].array[k1 + i, j];

            for (int i = k1; i > 0; i--)
                for (int j = 0; j < ny; j++)
                    zArray.array[nx - i, j] = Form1.zComplex[Form1.regComplex].array[k1 - i, j];

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[Form1.regComplex].array[i, j] = zArray.array[i, j];
            //VisualRegImage(Form1.regImage);
            ComplexPictureImage(Form1.regComplex);
        }

        public static void TRNS_D()             // Транспонирование zArrayDescriptor[regImage]
        {

            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROR_TRNS  zArrayDescriptor [" + Form1.regImage + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(ny, nx);

            for (int i = 0; i < ny; i++)
                for (int j = 0; j < nx; j++)
                    zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[j, i];

            Form1.zArrayDescriptor[Form1.regImage] = zArray;
            VisualRegImage(Form1.regImage);

        }
        public static void TRNS_С()             // Транспонирование zComplex[Form1.regComplex]
        {

            if (Form1.zComplex[Form1.regComplex] == null) { MessageBox.Show(" ROR_TRNS  zComplex [" + Form1.regComplex + "] == NULL"); return; }

            int nx = Form1.zComplex[Form1.regComplex].width;
            int ny = Form1.zComplex[Form1.regComplex].height;

            ZComplexDescriptor zArray = new ZComplexDescriptor(nx, ny);
            for (int i = 0; i < ny; i++)
                for (int j = 0; j < nx; j++)
                    zArray.array[i, j] = Form1.zComplex[Form1.regComplex].array[j, i];

            Form1.zComplex[Form1.regComplex] = zArray;
            ComplexPictureImage(Form1.regComplex);

        }
        public static void ROT180_D()             // Поворот zArrayDescriptor[regImage] на 180 градусов
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show(" ROR_TRNS  zArrayDescriptor [" + Form1.regImage + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    zArray.array[i, j] = Form1.zArrayDescriptor[Form1.regImage].array[i, ny-1-j];
            //MessageBox.Show(" ROR_TRNS 1");
            
                        Form1.zArrayDescriptor[Form1.regImage] = zArray;
            //MessageBox.Show(" ROR_TRNS 2");
            VisualRegImage(Form1.regImage);
        }

        public static void ABS_D()             // Абсолютное значение
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show("ABS_D zArrayDescriptor[" + Form1.regImage + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    Form1.zArrayDescriptor[Form1.regImage].array[i, j] = Math.Abs(Form1.zArrayDescriptor[Form1.regImage].array[i, j]);
                }
            VisualRegImage(Form1.regImage);
        }

        public static void Mirr_D()             // Зеркальное отбражение по x координате
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show("ABS_D zArrayDescriptor[" + Form1.regImage + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    rez.array[nx-i-1, j] = Math.Abs(Form1.zArrayDescriptor[Form1.regImage].array[i, j]);
                }

            Form1.zArrayDescriptor[Form1.regImage] = rez;
            VisualRegImage(Form1.regImage);
        }


        public static void ADD_D(int k1, int k2, int k3)             // Сложить два вещественных массива
        {
            k1--; k2--; k3--;                                   // Массив 1 ->  0
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("zArrayDescriptor [" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("zArrayDescriptor [" + k2 + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k1].height;

            int nx1 = Form1.zArrayDescriptor[k2].width;
            int ny1 = Form1.zArrayDescriptor[k2].height;

            nx = Math.Min(nx, nx1); ny = Math.Min(ny, ny1);


            Form1.zArrayDescriptor[k3] = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zArrayDescriptor[k3].array[i, j] = Form1.zArrayDescriptor[k2].array[i, j] + Form1.zArrayDescriptor[k1].array[i, j];
            VisualRegImage(k3);
        }

        public static void Sub_D1(int k1, int k2, int k3)             // Вычесть два вещественных массива (3 аргумента)
        {
            k1--; k2--; k3--;                                   // Массив 1 ->  0

            //MessageBox.Show(" Main   k1=" + k1 + " k2 =" + k2 + " k3 =" + k3);
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("Sub_D zArrayDescriptor [" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("Sub_D zArrayDescriptor [" + k2 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k1].width; 
            int ny = Form1.zArrayDescriptor[k1].height;

            int nx1 = Form1.zArrayDescriptor[k2].width;
            int ny1 = Form1.zArrayDescriptor[k2].height;

            nx = Math.Min(nx, nx1); ny = Math.Min(ny, ny1);

            Form1.zArrayDescriptor[k3] = new ZArrayDescriptor(nx, ny);
           
          
            for (int j = 0; j < ny; j++)
               for (int i = 0; i < nx; i++)
                    Form1.zArrayDescriptor[k3].array[i, j] = Form1.zArrayDescriptor[k1].array[i, j] - Form1.zArrayDescriptor[k2].array[i, j];
            VisualRegImage(k3);
        }
    
        

        public static void Send_C(int k1, int k2)   // Пересылка комплексных массивов
        {
            k1--; k2--;                                   // Массив 1 ->  0
            //MessageBox.Show("k1= " + k1 + " --> k2= " + k2);
            if (Form1.zComplex[k1] == null) { MessageBox.Show("zComplex[" + k1 + "] == NULL"); return; }
            int nx = Form1.zComplex[k1].width;
            int ny = Form1.zComplex[k1].height;

            Form1.zComplex[k2] = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    Form1.zComplex[k2].array[i, j] = Form1.zComplex[k1].array[i, j];
            ComplexPictureImage(k2);
        }

        public static void Send_C4(int k1, int k2)   // Пересылка 4 вещественных массивов
        {
            k1--;           k2--;                                   // Массив 1 ->  0
            k1 = k1 * 4;    k2 = k2 * 4;

            for (int k = 0; k < 4; k++)
            {

                if (Form1.zArrayDescriptor[k1 + k] == null) continue;

                int nx = Form1.zArrayDescriptor[k1+k].width;
                int ny = Form1.zArrayDescriptor[k1+k].height;
                Form1.zArrayDescriptor[k2+k] = new ZArrayDescriptor(nx, ny);

                for (int i = 0; i < nx; i++)
                    for (int j = 0; j < ny; j++)
                        Form1.zArrayDescriptor[k2+k].array[i, j] = Form1.zArrayDescriptor[k1+k].array[i, j];
                VisualRegImage(k2 + k);
            }

        

        }

        public static void Sub_C(int k3, int k4, int k5)       // Вычесть два комплексных массива
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

           // MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("zComplex[" + k4 + "] == NULL"); return; }
            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Размеры массивов не равны"); return; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    a.array[i, j] = Form1.zComplex[k3].array[i, j] - Form1.zComplex[k4].array[i, j];

            //zComplex[k5] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[k5] = a;
            ComplexPictureImage(k5);
        }

        public static void Add_C(int k3, int k4, int k5)             // Сложить два комплексных массива
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("zComplex[" + k4 + "] == NULL"); return; }
            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Размеры массивов не равны"); return; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    a.array[i, j] = Form1.zComplex[k3].array[i, j] + Form1.zComplex[k4].array[i, j];

            //zComplex[k5] = new ZComplexDescriptor(nx, ny);
            Form1.zComplex[k5] = a;
            ComplexPictureImage(k5);
        }

        public static void Mul_C(int k3, int k4, int k5, ProgressBar progressBar1)             // Умножить два комплексных массива
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("Mul_C zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("Mul_C zComplex[" + k4 + "] == NULL"); return; }

            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Mul_C Размеры массивов не согласованы"); return; }

            //ZComplexDescriptor a = new ZComplexDescriptor(nx, nx);         // Результирующая матрица
            ZComplexDescriptor b = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)                                    // Транспонирование второго массива
                for (int j = 0; j < ny; j++)
                    b.array[i, j] = Form1.zComplex[k3].array[i, j]* Form1.zComplex[k4].array[i, j];
/*
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = nx;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            Complex s0 = new Complex(0, 0);
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    Form1.zComplex[k3].array[i, j]= b.array[j, i]
                }
                progressBar1.PerformStep();
            }
            progressBar1.Value = 1;
            */
            Form1.zComplex[k5] = b;
            ComplexPictureImage(k5);
        }

        public static void Mul_D(int k3, int k4, int k5)             // Умножить два вещественных массива 
        {
            k3--; k4--; k5--;                                        // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zArrayDescriptor[k3] == null) { MessageBox.Show("Mul_C zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k4] == null) { MessageBox.Show("Mul_C zComplex[" + k4 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k3].width;
            int ny = Form1.zArrayDescriptor[k3].height;

            int nx1 = Form1.zArrayDescriptor[k4].width;
            int ny1 = Form1.zArrayDescriptor[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Mul_C Размеры массивов не согласованы"); return; }

            
            ZArrayDescriptor b = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)                                   
                for (int j = 0; j < ny; j++)
                    b.array[i, j] = Form1.zArrayDescriptor[k3].array[i, j] * Form1.zArrayDescriptor[k4].array[i, j];


            Form1.zArrayDescriptor[k5] = b;
            VisualRegImage(k5);
        }

        public static void Conv_D(int k3, int k4, int k5, ProgressBar progressBar1)             // Корреляция двух вещественных массивов
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zArrayDescriptor[k3] == null) { MessageBox.Show("Mul_C zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k4] == null) { MessageBox.Show("Mul_C zComplex[" + k4 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k3].width;
            int ny = Form1.zArrayDescriptor[k3].height;

            int nx1 = Form1.zArrayDescriptor[k4].width;
            int ny1 = Form1.zArrayDescriptor[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Mul_C Размеры массивов не согласованы"); return; }


            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = 3;
            progressBar1.Value = 1;
            progressBar1.Step = 1;


            ZArrayDescriptor b = new ZArrayDescriptor(ny, nx);

            for (int i = 0; i < nx; i++)                                    // Транспонирование первого массива
                for (int j = 0; j < ny; j++)
                    b.array[j, i] = Form1.zArrayDescriptor[k3].array[i, j];
            progressBar1.PerformStep();                                     // ------------------------------------ 1 шаг

            ZComplexDescriptor zCmpl1 = new ZComplexDescriptor(b);  // Конструктор для заполнения реальной части и мнимой части = 0
            ZComplexDescriptor zCmpl2 = new ZComplexDescriptor(Form1.zArrayDescriptor[k4]);

            zCmpl1 = FurieN.BPF2(zCmpl1); progressBar1.PerformStep();       // ------------------------------------ 2 шаг
            zCmpl2 = FurieN.BPF2(zCmpl2); progressBar1.PerformStep();       // ------------------------------------ 3 шаг

            ZComplexDescriptor a = new ZComplexDescriptor(nx, nx);          // Результирующая матрица (ny x nx)*(nx * ny) = (nx x nx)

            progressBar1.Maximum = nx;
            progressBar1.Value = 1;

            Complex s0 = new Complex(0, 0);
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < nx; j++)
                {
                    Complex s = s0;
                    for (int y = 0; y < ny; y++)
                        s += zCmpl1.array[y, j] * zCmpl2.array[i, y];     // Строка на столбец
                    a.array[i, j] = s;
                }
                progressBar1.PerformStep();
            }
            progressBar1.Value = 1;

            a = FurieN.BPF2(a);                                             // ------------------------------------ 5 шаг
            Form1.zArrayDescriptor[k5] = Furie.zAmplituda(a);

            
            VisualRegImage(k5);
        }
        public static void Div_D(int k1, int k2, int k3)             // Разделить два вещественных массива (3 аргумента)
        {
            k1--; k2--; k3--;                                   // Массив 1 ->  0

            //MessageBox.Show(" Main   k1=" + k1 + " k2 =" + k2 + " k3 =" + k3);
            if (Form1.zArrayDescriptor[k1] == null) { MessageBox.Show("zArrayDescriptor [" + k1 + "] == NULL"); return; }
            if (Form1.zArrayDescriptor[k2] == null) { MessageBox.Show("zArrayDescriptor [" + k2 + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[k1].width;
            int ny = Form1.zArrayDescriptor[k1].height;

            ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double b = Form1.zArrayDescriptor[k2].array[i, j];
                    if (b != 0) rez.array[i, j] = Form1.zArrayDescriptor[k1].array[i, j] / b; else rez.array[i, j] = 0;
                }
            Form1.zArrayDescriptor[k3] = rez;
            VisualRegImage(k3);
        }

        public static void Div_C(int k3, int k4, int k5)             // Разделить два комплексных массива поэлементно Form1.zComplex[k]
        {
            k3--; k4--; k5--;                                  // Массив 1 ->  0

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (Form1.zComplex[k3] == null) { MessageBox.Show("zComplex[" + k3 + "] == NULL"); return; }
            if (Form1.zComplex[k4] == null) { MessageBox.Show("zComplex[" + k4 + "] == NULL"); return; }
            int nx = Form1.zComplex[k3].width;
            int ny = Form1.zComplex[k3].height;

            int nx1 = Form1.zComplex[k4].width;
            int ny1 = Form1.zComplex[k4].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Размеры массивов не равны"); return; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                { 
                   a.array[i, j] = Div(Form1.zComplex[k3].array[i, j], Form1.zComplex[k4].array[i, j]);
                  // a.array[i, j] = Complex.Divide( Form1.zComplex[k3].array[i, j] , Form1.zComplex[k4].array[i, j] );
                }
            Form1.zComplex[k5] = a;
            ComplexPictureImage(k5);
           
        }
/*
        public static ZComplexDescriptor Div_CMPLX(ZComplexDescriptor a1, ZComplexDescriptor a2)             // Разделить два комплексных массива поэлементно 
        {
            

            //MessageBox.Show("k3= " + k3 + " - k4= " + k4 + " = k5= " + k5);

            if (a1 == null) { MessageBox.Show("Div_CMPLX a1 == NULL"); return null; }
            if (a2 == null) { MessageBox.Show("Div_CMPLX a2 == NULL"); return null; }
            
            int nx = a1.width;
            int ny = a1.height;

            int nx1 = a2.width;
            int ny1 = a2.height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Div_CMPLX Размеры массивов не равны"); return null; }

            ZComplexDescriptor a = new ZComplexDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                    a.array[i, j] = Div(a1.array[i, j], a2.array[i, j]); 
            return a; 
        }
*/
        private static Complex Div(Complex a, Complex b)  // Деление комплексных чисел 
        {
            Complex res = new Complex(0, 0);
            double D = b.Real * b.Real + b.Imaginary * b.Imaginary;
            if (D != 0)
            //if (Math.Abs(D) > 0.0001 )
            {
                double d = (a.Real * b.Real       + a.Imaginary * b.Imaginary) / D;
                double im = (a.Imaginary * b.Real - a.Real * b.Imaginary) / D;
                res = new Complex(d, im);
            }
           // else
           // { res = new Complex(0, 0); }
           
            return res;
        }

        public static void Ampl_C(int k11, int k12)             // Амплитуда суммы двух комплексных массивов
        {
            k11--; k12--;                                       // Массив 11 +12 -> zArrayPicture

            MessageBox.Show("k11= " + k11 + " + k12= " + k12 );

            if (Form1.zComplex[k11] == null) { MessageBox.Show("zComplex[" + k11 + "] == NULL"); return; }
            if (Form1.zComplex[k12] == null) { MessageBox.Show("zComplex[" + k12 + "] == NULL"); return; }
            int nx = Form1.zComplex[k11].width;
            int ny = Form1.zComplex[k11].height;

            int nx1 = Form1.zComplex[k12].width;
            int ny1 = Form1.zComplex[k12].height;

            if ((nx != nx1) || (ny != ny1)) { MessageBox.Show("Размеры массивов не равны"); return; }

            ZArrayDescriptor a = new ZArrayDescriptor(nx, ny);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double a1 = Form1.zComplex[k11].array[i, j].Magnitude;
                    double a2 = Form1.zComplex[k12].array[i, j].Magnitude;
                    double f1 = Form1.zComplex[k11].array[i, j].Phase;
                    double f2 = Form1.zComplex[k12].array[i, j].Phase;
                    a.array[i, j] =  a1 * a1 + a2 * a2 + 2 * a1 * a2 * Math.Cos(f1 - f2);
                }

            //zComplex[k5] = new ZComplexDescriptor(nx, ny);
            Form1.zArrayPicture = a;

        }
        public static void Add_Double(int k15, double k16)             // Сложить массив с числом
        {
            k15--;                    // Массив 1 ->  0

           
            if (Form1.zArrayDescriptor[k15] == null) { MessageBox.Show(" Add_Double zArrayDescriptor [" + k15 + "] == NULL"); return; }
           
            int nx = Form1.zArrayDescriptor[k15].width;
            int ny = Form1.zArrayDescriptor[k15].height;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    Form1.zArrayDescriptor[k15].array[i, j] = Form1.zArrayDescriptor[k15].array[i, j] + k16;
                   
                }
            VisualRegImage(k15);
        }

        public static void Div_Double(int k15, double k16)             // Сложить массив с числом
        {
            k15--;                    // Массив 1 ->  0


            if (Form1.zArrayDescriptor[k15] == null) { MessageBox.Show(" Add_Double zArrayDescriptor [" + k15 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k15].width;
            int ny = Form1.zArrayDescriptor[k15].height;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    Form1.zArrayDescriptor[k15].array[i, j] = Form1.zArrayDescriptor[k15].array[i, j]/ k16;

                }
            VisualRegImage(k15);
        }
        public static void Mul_Double(int k15, double k16)             // Сложить массив с числом
        {
            k15--;                    // Массив 1 ->  0


            if (Form1.zArrayDescriptor[k15] == null) { MessageBox.Show(" Add_Double zArrayDescriptor [" + k15 + "] == NULL"); return; }

            int nx = Form1.zArrayDescriptor[k15].width;
            int ny = Form1.zArrayDescriptor[k15].height;

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    Form1.zArrayDescriptor[k15].array[i, j] = Form1.zArrayDescriptor[k15].array[i, j] * k16;

                }
            VisualRegImage(k15);
        }

        public static void Diapazon(double a1, double a2)             // Привести к диапазону вещественный кадр
        {
            if (Form1.zArrayDescriptor[Form1.regImage] == null) { MessageBox.Show("Diapazon zArrayDescriptor[" + Form1.regImage + "] == NULL"); return; }
            int nx = Form1.zArrayDescriptor[Form1.regImage].width;
            int ny = Form1.zArrayDescriptor[Form1.regImage].height;

            //ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);

            double min = SumClass.getMin(Form1.zArrayDescriptor[Form1.regImage]);
            double max = SumClass.getMax(Form1.zArrayDescriptor[Form1.regImage]);

            for (int i = 0; i < nx; i++)
              for (int j = 0; j < ny; j++)
                {
                    double d = Form1.zArrayDescriptor[Form1.regImage].array[i, j];
                    Form1.zArrayDescriptor[Form1.regImage].array[i, j] =(d-min)*(a2-a1)/(max-min) + a1 ;
                }


            VisualRegImage(Form1.regImage);
        }

        public static void Diapazon_С(double a1, double a2)             // Привести к диапазону комплексный кадр
        {
            if (Form1.zComplex[Form1.regComplex] == null) { MessageBox.Show("Diapazon zComplex[" + Form1.regComplex + "] == NULL"); return; }
           
            int nx = Form1.zComplex[Form1.regComplex].width;
            int ny = Form1.zComplex[Form1.regComplex].height;

            ZArrayDescriptor rez = new ZArrayDescriptor(nx, ny);
            for (int i = 0; i < nx; i++) for (int j = 0; j < ny; j++) { rez.array[i, j] = Form1.zComplex[Form1.regComplex].array[i, j].Magnitude; }

            double min = SumClass.getMin(rez);
            double max = SumClass.getMax(rez);

            for (int i = 0; i < nx; i++)
                for (int j = 0; j < ny; j++)
                {
                    double d = rez.array[i, j];
                    rez.array[i, j] = (d - min) * (a2 - a1) / (max - min) + a1;
                }

            ZComplexDescriptor rezc = new ZComplexDescriptor(Form1.zComplex[Form1.regComplex], rez);  // Амплитуда в rez, фаза старая
            Form1.zComplex[Form1.regComplex] = rezc;

            ComplexPictureImage(Form1.regComplex);
        }
        //-------------------------------------------------------------------------------------------------------------------------------
    }
}
