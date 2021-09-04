using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using ClassLibrary;
using rab1.Forms;

namespace rab1
{
    class Model_object
    {
        public static ZComplexDescriptor Model_1(ZComplexDescriptor cmpl0, double sdvg, double noise, double Lambda)
        {
            int NX, NY;
            if (cmpl0 == null) { NX = 1024; NY = 1024; }
            else { NX = cmpl0.width; NY = cmpl0.height; }
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            //MessageBox.Show("sdvg= " + sdvg);
            int ax = 512;
            int by = 128;
            int i1 = NX / 2 - ax / 2;
            int j1 = NY / 2 - by / 2;

            double max = 2 * Math.PI * sdvg / ax;
            //Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Random rnd = new Random();
            int ii = 0;
            for (int i = i1; i < ax + i1; i++)
                for (int j = j1; j < by + j1; j++)
                {
                    double fa = (0.5 - rnd.NextDouble()) * Math.PI * noise;   //rnd.NextDouble() 0-1
                    double fz = (i - i1) * max - Math.PI;
                    if (j < NX / 2) ii++; else ii--;
                    cmpl.array[i, j] = Complex.FromPolarCoordinates(ii, fz + fa);

                }

            return cmpl;
        }

        public static ZComplexDescriptor Model_0(double sdvg, double noise, double Lambda)
        {
            int NX = 1024, NY = 1024;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            //MessageBox.Show("sdvg= " + sdvg);
            int ax = 512;
            int by = 128;
            int i1 = NX / 2 - ax / 2;
            int j1 = NY / 2 - by / 2;

            double max = 2 * Math.PI * sdvg / ax;
            //Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Random rnd = new Random();
            int ii = 128;
            for (int i = i1; i < ax + i1; i++)
                for (int j = j1; j < by + j1; j++)
                {
                    double fa = (0.5 - rnd.NextDouble()) * Math.PI * noise;   //rnd.NextDouble() 0-1
                    double fz = (i - i1) * max - Math.PI;
                    //if (j < NX / 2) ii++; else ii--;
                    cmpl.array[i, j] = Complex.FromPolarCoordinates(ii, fz + fa);

                }

            return cmpl;
        }


        public static ZComplexDescriptor Model_2(double sdvg, double noise, double Lambda)
        {
            int NX = 2048, NY = 2048;

            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            //MessageBox.Show("sdvg= " + sdvg);
            int ax = 512;
            int by = 128;
            int i1 = NX / 2 - ax / 2;
            int j1 = NY / 2 - by / 2;

            double max = 2 * Math.PI * sdvg / ax;
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            //Random rnd = new Random();
            int ii = 0;
            for (int i = i1; i < ax + i1; i++)
                for (int j = j1; j < by + j1; j++)
                {
                    double fa = (0.5 - rnd.NextDouble()) * Math.PI * noise;   //rnd.NextDouble() 0-1
                    double fz = (i - i1) * max - Math.PI;
                    if (j < NX / 2) ii++; else ii--;
                    cmpl.array[i, j] = Complex.FromPolarCoordinates(ii, fz + fa);
                }

            return cmpl;
        }
        //  Голограмма двух экспозиций
        public static ZComplexDescriptor Model_ADD(ZComplexDescriptor cmpl0, ZArrayDescriptor cmpl1)
        {
            if (cmpl0 == null) { MessageBox.Show("Model_object ZComplexDescriptor == NULL"); return null; }
            if (cmpl1 == null) { MessageBox.Show("Model_object ZArrayDescriptor == NULL"); return null; }
            int NX = cmpl0.width;
            int NY = cmpl0.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double am1 = cmpl0.array[i, j].Magnitude;
                    double am2 = cmpl1.array[i, j];
                    cmpl.array[i, j] = new Complex(am1 + am2, 0.0);
                }

            return cmpl;
        }

        public static ZComplexDescriptor Model_ADD2(ZArrayDescriptor cmpl0, ZArrayDescriptor cmpl1)
        {
            if (cmpl0 == null) { MessageBox.Show("Model_object ZComplexDescriptor == NULL"); return null; }
            if (cmpl1 == null) { MessageBox.Show("Model_object ZArrayDescriptor == NULL"); return null; }
            int NX = cmpl0.width;
            int NY = cmpl0.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = new Complex(cmpl0.array[i, j] + cmpl1.array[i, j], 0.0); }

            return cmpl;
        }




        public static ZArrayDescriptor Model_ADD_ZArray(ZArrayDescriptor cmpl0, ZArrayDescriptor cmpl1)
        {
            if (cmpl0 == null) { MessageBox.Show("Model_object ArrayDescriptorr == NULL"); return null; }
            if (cmpl1 == null) { MessageBox.Show("Model_object ArrayDescriptorr == NULL"); return null; }
            int NX = cmpl0.width;
            int NY = cmpl0.height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = cmpl0.array[i, j] + cmpl1.array[i, j]; }

            return cmpl;
        }

        public static ZArrayDescriptor Intens_ADD_Cmplx(ZComplexDescriptor[] zComplex, int k1, int k2)   //  Интенсивность от сложения комплекcных массивов
        {
            if (zComplex[k1] == null) { MessageBox.Show("Model_object ZComplex[k1] == NULL"); return null; }
            if (zComplex[k2] == null) { MessageBox.Show("Model_object ZComplex[k2] == NULL"); return null; }
            int NX = zComplex[k1].width;
            int NY = zComplex[k1].height;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                {
                    double a1 = zComplex[k1].array[i, j].Magnitude;
                    double a2 = zComplex[k2].array[i, j].Magnitude;
                    double f = zComplex[k1].array[i, j].Phase - zComplex[k2].array[i, j].Phase;
                    cmpl.array[i, j] = a1 * a1 + a2 * a2 + 2 * a1 * a2 * Math.Cos(f);
                }

            return cmpl;
        }


        public static ZComplexDescriptor Model_ADD_Cmplx(ZComplexDescriptor[] zComplex, int k1, int k2)   //  Сложение комплекcных массивов
        {
            if (zComplex[k1] == null) { MessageBox.Show("Model_object ZComplex[k1] == NULL"); return null; }
            if (zComplex[k2] == null) { MessageBox.Show("Model_object ZComplex[k2] == NULL"); return null; }
            int NX = zComplex[k1].width;
            int NY = zComplex[k1].height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex[k1].array[i, j] + zComplex[k2].array[i, j]; }

            return cmpl;
        }


        public static ZComplexDescriptor Model_SUB_Cmplx(ZComplexDescriptor[] zComplex, int k1, int k2)   //  Вычитание комплекных массивов
        {
            if (zComplex[k1] == null) { MessageBox.Show("Model_object ZComplex[k1] == NULL"); return null; }
            if (zComplex[k2] == null) { MessageBox.Show("Model_object ZComplex[k2] == NULL"); return null; }
            int NX = zComplex[k1].width;
            int NY = zComplex[k1].height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex[k1].array[i, j] - zComplex[k2].array[i, j]; }

            return cmpl;
        }

        public static ZComplexDescriptor ADD_Cmplx(ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2)   //  Сложение комплекных массивов
        {
            if (zComplex1 == null) { MessageBox.Show("Model_object ZComplex1 == NULL"); return null; }
            if (zComplex2 == null) { MessageBox.Show("Model_object ZComplex2 == NULL"); return null; }
            int NX = zComplex1.width;
            int NY = zComplex1.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex1.array[i, j] + zComplex2.array[i, j]; }

            return cmpl;
        }

        public static ZComplexDescriptor SUB_Cmplx(ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2)   //  Сложение комплекных массивов
        {
            if (zComplex1 == null) { MessageBox.Show("Model_object ZComplex1 == NULL"); return null; }
            if (zComplex2 == null) { MessageBox.Show("Model_object ZComplex2 == NULL"); return null; }
            int NX = zComplex1.width;
            int NY = zComplex1.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex1.array[i, j] - zComplex2.array[i, j]; }

            return cmpl;
        }

        public static ZComplexDescriptor MUL_Cmplx(ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2)   //  Сложение комплекных массивов
        {
            if (zComplex1 == null) { MessageBox.Show("Model_object ZComplex1 == NULL"); return null; }
            if (zComplex2 == null) { MessageBox.Show("Model_object ZComplex2 == NULL"); return null; }
            int NX = zComplex1.width;
            int NY = zComplex1.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex1.array[i, j] * zComplex2.array[i, j]; }

            return cmpl;
        }


        public static ZComplexDescriptor Model_ADD_zComplex(ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2)   //  Сложение комплекных массивов
        {
            if (zComplex1 == null) { MessageBox.Show("Model_object Complex1 == NULL"); return null; }
            if (zComplex2 == null) { MessageBox.Show("Model_object zComplex2 == NULL"); return null; }
            int NX = zComplex1.width;
            int NY = zComplex1.height;
            ZComplexDescriptor cmpl = new ZComplexDescriptor(NX, NY);       // Выходной массив

            for (int i = 0; i < NX; i++)
                for (int j = 0; j < NY; j++)
                { cmpl.array[i, j] = zComplex1.array[i, j] + zComplex2.array[i, j]; }

            return cmpl;
        }



        // Моделирование голографической интерферометрии (сложение волновых фронтов) -> zComplex[2]
        public static void Glgr_Interf(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, double sdvg, double noise, double Lambda)
        {
            int NX = 1024;
            int NY = 1024;
            int m = 10;
            double am = 255;                                                     // Амплитуда опорной волны
            double dx = 10000;                                                   // Размер объекта в мкм
            double AngleY = 0.687;

            zComplex[0] = new ZComplexDescriptor(NX, NY);

            zComplex[0] = Model_1(zComplex[0], 0, noise, Lambda);                               // Модель объекта с нулевым сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[1] = Furie.FourierTransform(zComplex[0], m);                               // Преобразование Фурье
            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], 0, AngleY, Lambda, dx, 0);    // Сложение с опорной волной
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);          // Из zComplex[] ->   zArrayDescriptor[]                                             
            ZArrayDescriptor amp = new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);           // Амплитуда     
            zComplex[1] = new ZComplexDescriptor(amp);                                          // Формирование голограммы (Re=амплитуда, Im=0)
            zComplex[1] = Furie.FourierTransform(zComplex[1], m);                               // Преобразование Фурье


            zComplex[0] = Model_1(zComplex[0], sdvg, noise, Lambda);                             // Модель объекта со сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[2] = Furie.FourierTransform(zComplex[0], m);                               // Преобразование Фурье
            zComplex[2] = Model_interf.Model_pl_ADD(am, zComplex[2], 0, AngleY, Lambda, dx, 0);
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 2);
            amp = new ZArrayDescriptor(zArrayDescriptor[2 * 4 + 2]);                            // Амплитуда     
            zComplex[2] = new ZComplexDescriptor(amp);                                          // Формирование голограммы (Re=амплитуда, Im=0)
            zComplex[2] = Furie.FourierTransform(zComplex[2], m);

            zComplex[2] = Model_ADD_Cmplx(zComplex, 1, 2);                      //  Сложение комплексных массивов


        }

        // Моделирование голографической интерферометрии (Двойная экспозиция) -> zComplex[2]

        public static void Glgr_Interf2(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, double sdvg0, double sdvg, double noise, double Lambda, double lm, double dx, double AngleX, double AngleY)
        {
            //int NX = 1024;
            //int NY = 1024;
            //zComplex[0] = new ZComplexDescriptor(NX, NY);

            //double am = 8000;                                                                   // Амплитуда опорной волны
            //int m = Furie.PowerOfTwo(NX);   

            zComplex[0] = Model_2(sdvg0, noise, Lambda);                                        // Модель объекта до деформации
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            //zComplex[1] = Furie.FourierTransform(zComplex[0], 10);                             // Преобразование Фурье
            //MessageBox.Show("lm= "+lm + "dx= " + dx + " Lambda= " + Lambda + " AngleX= " + AngleX + " AngleY = " + AngleY);
            zComplex[1] = FurieN.FrenelTransformN(zComplex[0], Lambda, lm, dx);                 // Преобразование Френеля
            //zComplex[1] = Furie.FrenelTransform(zComplex[0], m, Lambda, lm, dx);
            //FurieN.FrenelTransformN(zComplex[k1], lambda, d, xmax);
            double am = SumClass.getAverage(zComplex[1]);


            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], AngleX, AngleY, Lambda, dx, 0); // Сложение с опорной волной
            //zComplex[1] = Model_interf.Model_pl_MUL(am, zComplex[1], AngleX, AngleY, Lambda, dx);
            ZArrayDescriptor amp1 = Intens_ADD_Cmplx(zComplex, 0, 1);                                // Из комплексного числа => Интенрферограмма


            //ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);            // Из zComplex[] ->   zArrayDescriptor[]                                             
            //ZArrayDescriptor amp1 = new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);            // Амплитуда     

            // 2 состояние ------------------------------------------------------------------------------------------------------------------
            zComplex[0] = Model_2(sdvg, noise, Lambda);                                         // Модель объекта после деформации
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[2] = FurieN.FrenelTransformN(zComplex[0], Lambda, lm, dx);                 // Преобразование Френеля    
            //zComplex[2] = Furie.FrenelTransform(zComplex[0], m, Lambda, lm, dx);
            zComplex[2] = Model_interf.Model_pl_ADD(am, zComplex[2], AngleX, AngleY, Lambda, dx, 0); // Сложение с опорной волной//zComplex[2] = Model_interf.Model_pl_ADD(am, zComplex[2], AngleX, AngleY, Lambda, dx, 0);
            zComplex[2] = Model_interf.Model_pl_MUL(am, zComplex[2], AngleX, AngleY, Lambda, dx);
            //ZArrayDescriptor amp2 = Furie.zAmplituda(zComplex[2]);                              // Амплитуда   
            ZArrayDescriptor amp2 = Intens_ADD_Cmplx(zComplex, 0, 2);


            //zComplex[2] = Model_ADD(zComplex[2], amp1);
            ZArrayDescriptor amp3 = Model_ADD_ZArray(amp1, amp2);                               // Сложение двух амплитуд

            zArrayDescriptor[0 * 4 + 0] = amp1;                                 // 1-я голограмма для отображения в реальный массив
            zArrayDescriptor[0 * 4 + 1] = amp2;                                 // 2-я голограмма
            zArrayDescriptor[0 * 4 + 2] = amp3;                                 // Сложение голограмм

            //zComplex[1] = new ZComplexDescriptor(NX, NY);                       // Нулевой zComplex[1] 
            zComplex[1] = new ZComplexDescriptor(zComplex[1], amp3);              // амплитуда = amp3, фаза - (прежняя) нулевая

            zComplex[2] = FurieN.FrenelTransformN(zComplex[1], Lambda, lm, dx);
            //zComplex[2] = Furie.FrenelTransform(zComplex[1], m, Lambda, lm, dx);
        }

        // Моделирование голографической интерферометрии (Две голограммы) -> zArrayDescriptor 9,10
        public static void Glgr_Interf3(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, double sdvg, double noise, double Lambda)
        {
            int NX = 1024;
            int NY = 1024;
            int m = Furie.PowerOfTwo(NX);
            double am = 255;                                                     // Амплитуда опорной волны
            double dx = 10000;                                                   // Размер объекта в мкм
            double AngleY = 0.687;

            zComplex[0] = new ZComplexDescriptor(NX, NY);

            zComplex[0] = Model_1(zComplex[0], 0, noise, Lambda);                               // Модель объекта с нулевым сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                            // Циклический сдвиг
            zComplex[1] = Furie.FourierTransform(zComplex[0], m);                               // Преобразование Фурье
            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], 0, AngleY, Lambda, dx, 0); // Сложение с опорной волной
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);          // Из zComplex[1] ->   zArrayDescriptor[1]                                             
            zArrayDescriptor[8] = new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);            // Амплитуда     


            zComplex[0] = Model_1(zComplex[0], sdvg, noise, Lambda);                             // Модель объекта со сдвигом
            zComplex[0] = Furie.Invers(zComplex[0]);                                             // Циклический сдвиг
            zComplex[1] = Furie.FourierTransform(zComplex[0], m);                                // Преобразование Фурье
            zComplex[1] = Model_interf.Model_pl_ADD(am, zComplex[1], 0, AngleY, Lambda, dx, 0);
            ZComplexDescriptor.Complex_ArrayDescriptor(zComplex, zArrayDescriptor, 1);           // Из zComplex[1] ->   zArrayDescriptor[1]   
            zArrayDescriptor[9] = new ZArrayDescriptor(zArrayDescriptor[1 * 4 + 2]);            // Амплитуда     

        }


        //   Получение 4 интерференционных картин со сдвигом k1, k2, k3, k4
        //   Исходный фронт zComplex2 + опора(ki)
        //   Формируется волновой фронт (голограмма) с прежней амплитудой и фазой, определенной пошаговым сдвигом
        //   Складываем с zComplex1 (голографическая интерферограмма)
        //   Добавляется опора и восстанавливается исходное изображение zArray_inter

        public static ZArrayDescriptor PSI(double am, ZComplexDescriptor zComplex1, ZComplexDescriptor zComplex2,
                                           double AngleX, double AngleY, double noise, double Lambda, double dx, double d, double[] fz,
                                           int k1, int k2, int k3, int k4)
        {

            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];
            double[] fz1 = new double[4];
            fz1 = SDVIG(90, 20, 30, 180);                        // Случайные градусы
            zArray[0] = Model_interf.Model_pl_PSI(am, zComplex1, AngleX, AngleY, Lambda, dx, noise, fz1[k1]);   // Сложение с опорой добавление фазового сдвига zComplex2 (второе состояние)
            zArray[1] = Model_interf.Model_pl_PSI(am, zComplex1, AngleX, AngleY, Lambda, dx, noise, fz1[k2]);
            zArray[2] = Model_interf.Model_pl_PSI(am, zComplex1, AngleX, AngleY, Lambda, dx, noise, fz1[k3]);
            zArray[3] = Model_interf.Model_pl_PSI(am, zComplex1, AngleX, AngleY, Lambda, dx, noise, fz1[k4]);

            ZComplexDescriptor zComplex_tmp = ATAN_PSI.ATAN_ar(zArray, fz, am);

            // zComplex_tmp = Model_interf.Model_pl_MUL(1, zComplex_tmp, AngleX, AngleY, Lambda, dx); // Умножение для устранения разности фаз
            zComplex_tmp = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);                   // Преобразование Френеля с четным количеством точек     

            ZArrayDescriptor zArray_tmp = new ZArrayDescriptor(zComplex1.width, zComplex1.height);

            for (int i = 0; i < zComplex1.width; i++)
                for (int j = 0; j < zComplex1.height; j++)
                {
                    double Ar = zComplex_tmp.array[i, j].Magnitude;                                 // амплитуда объектного пучка
                    double Fr = zComplex_tmp.array[i, j].Phase;

                    double Ap = zComplex2.array[i, j].Magnitude;                                     // амплитуда объектного пучка
                    double Fp = zComplex2.array[i, j].Phase;                                         // Фаза объектного пучка
                    zArray_tmp.array[i, j] = Ap * Ap + Ar * Ar + 2 * Ap * Ar * Math.Cos(Fp - Fr);    // Интенсивность
                }

            return zArray_tmp;
        }
        public static double[] SDVIG(double f1, double f2, double f3, double f4)
        {
            double[] fzrad = new double[4];
            fzrad[0] = Math.PI * f1 / 180.0;   // Фаза в радианах  
            fzrad[1] = Math.PI * f2 / 180.0;
            fzrad[2] = Math.PI * f3 / 180.0;
            fzrad[3] = Math.PI * f4 / 180.0;
            return fzrad;
        }
        // ----------------------------------------------------------------------------------------------------------------------------------
        //                  Формирование 4 голографических интерферограмм. Затем расшифровка их методом PSI
        // ----------------------------------------------------------------------------------------------------------------------------------

        public static void Glgr_Interf_PSI_Fr(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor, ProgressBar progressBar1,
                                              double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d,
                                              double[] fz, double Ax, double Ay)
        {

            double AngleX = Ax;
            double AngleY = Ay;
            double noise_add = noise;

            //zComplex[0] = new ZComplexDescriptor(NX, NY);
            //MessageBox.Show(" noise = " + noise );

            // ----------------------------------------------------------------------------------------------------------------------------- 1 голограмма
            ZComplexDescriptor zComplex_tmp = Model_2(sdvg0, noise, Lambda);                       // Модель объекта с нулевым сдвигом                                                                                                                                                 // Модель объекта с нулевым сдвигом
            zComplex_tmp = Furie.Invers(zComplex_tmp);                                             // Циклический сдвиг
            zComplex_tmp = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);                   // Преобразование Френеля с четным количеством точек
            double am = SumClass.getAverage(zComplex_tmp);

            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4]; // Рабочие массивы    
            double[] fz1 = new double[4];
            fz1 = SDVIG(10, 20, 30, 40);                        // Случайные градусы
            for (int i = 0; i < 4; i++) { zArray[i] = Model_interf.Model_pl_PSI(am, zComplex_tmp, AngleX, AngleY, Lambda, dx, noise_add, fz1[i]); }       // Сложение с опорной волной + fz[i]
            zComplex_tmp = ATAN_PSI.ATAN_ar(zArray, fz, am);


            //zComplex_tmp = Model_interf.Model_pl_MUL(1, zComplex_tmp, AngleX, AngleY, Lambda, dx); // Умножение для устранения разности фаз
            zComplex[0] = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);                      // Преобразование Френеля с четным количеством точек

            MessageBox.Show(" 1  состояние -> 0");
            // ----------------------------------------------------------------------------------------------------------------------------- 2 голограмма

            zComplex[1] = Model_2(sdvg1, noise, Lambda);                                             // Модель объекта со сдвигом    
            zComplex_tmp = Furie.Invers(zComplex[1]);                                                // Циклический сдвиг
            zComplex[1] = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);                      // Преобразование Френеля с четным количеством точек
            MessageBox.Show(" 2  голограмма -> 1");

            zArrayDescriptor[8] = PSI(am, zComplex[1], zComplex[0], AngleX, AngleY, noise, Lambda, dx, d, fz, 0, 1, 2, 3);
            zArrayDescriptor[9] = PSI(am, zComplex[1], zComplex[0], AngleX, AngleY, noise, Lambda, dx, d, fz, 3, 0, 1, 2);
            zArrayDescriptor[10] = PSI(am, zComplex[1], zComplex[0], AngleX, AngleY, noise, Lambda, dx, d, fz, 2, 3, 0, 1);
            zArrayDescriptor[11] = PSI(am, zComplex[1], zComplex[0], AngleX, AngleY, noise, Lambda, dx, d, fz, 1, 2, 3, 0);
        }
        // Непосредственное сравнение двух волновых фронтов до и после деформации

        public static void Glgr_Interf8_PSI_Fr(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor,
            double sdvg0, double sdvg1, double noise, double Lambda, double dx, double d, double[] fz, double AngleX, double AngleY)
        {

            double noise_add = 0;

            //zComplex[0] = new ZComplexDescriptor(NX, NY);
            //ZComplexDescriptor zComplex_tmp  = new ZComplexDescriptor(NX, NY);
            //ZComplexDescriptor zComplex_tmp1 = new ZComplexDescriptor(NX, NY);
            //ZComplexDescriptor zComplex_tmp2 = new ZComplexDescriptor(NX, NY);
            // ---------------------------------------------------------------------------------- 1 голограмма

            ZComplexDescriptor zComplex_tmp = Model_2(sdvg0, noise, Lambda);                      // Модель объекта первое состояние
            zComplex_tmp = Furie.Invers(zComplex_tmp);                                            // Циклический сдвиг
            // zComplex[1] = Furie.FourierTransform(zComplex[0], m);                              // Преобразование Фурье
            //zComplex_tmp = Furie.FrenelTransform(zComplex_tmp, m, Lambda, d, dx);               // Преобразование Френеля
            zComplex_tmp = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);
            double am = SumClass.getAverage(zComplex_tmp);                                        // Амплитуда опорной волны (среднее от амплитуды zComplex[1])

            //ZComplexDescriptor zComplex_tmp1 =  Model_interf.Model_pl_ADD_Interf(am, zComplex_tmp, AngleY, AngleY, Lambda, dx, 0);   // Амплитуда интерференция
            //ZComplexDescriptor zComplex_tmp1 = Model_interf.Model_pl_ADD(am, zComplex_tmp, AngleY, AngleY, Lambda, dx, 0);         // Обычное сложение
            //zComplex[2] = Model_interf.Model_pl_MUL(1, zComplex_tmp1, AngleX, AngleY, Lambda, dx); // Умножение для устранения разности фаз
            //zComplex[2] = FurieN.FrenelTransformN(zComplex_tmp1, Lambda, d, dx);


            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];                                  // Вещественные рабочие массивы для 4 голограмм

            for (int i = 0; i < 4; i++)                                                           // Сложение с опорной волной + fz[i]
            {
                zArray[i] = Model_interf.Model_pl_PSI(am, zComplex_tmp, AngleX, AngleY, Lambda, dx, noise_add, fz[i]);     // Амплитуды интерферограммы
            }
            zComplex_tmp = ATAN_PSI.ATAN_ar(zArray, fz, am);
            // zComplex_tmp = ATAN_PSI.ATAN_ar(zArray, fz, am);                                      // 1 комплексная голограмма =>  zComplex_tmp

            zComplex_tmp = Model_interf.Model_pl_MUL(1, zComplex_tmp, AngleX, AngleY, Lambda, dx); // Умножение для устранения разности фаз
            zComplex[0] = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);                     // Восстановленное комплекcное поле для 1 состояния



            // ------------------------------------------------------------------------------------ 2 голограмма
            zComplex_tmp = Model_2(sdvg1, noise, Lambda);                                         // Модель объекта 2 состояние
            zComplex_tmp = Furie.Invers(zComplex_tmp);                                            // Циклический сдвиг   
            zComplex_tmp = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);                  // Преобразование Френеля
            for (int i = 0; i < 4; i++) { zArray[i] = Model_interf.Model_pl_PSI(am, zComplex_tmp, AngleX, AngleY, Lambda, dx, noise_add, fz[i]); }
            zComplex_tmp = ATAN_PSI.ATAN_ar(zArray, fz, am);
            zComplex_tmp = Model_interf.Model_pl_MUL(1, zComplex_tmp, AngleX, AngleY, Lambda, dx); // Умножение для устранения разности фаз
            zComplex[1] = FurieN.FrenelTransformN(zComplex_tmp, Lambda, d, dx);                     // Восстановленное комплекcное поле для 1 состояния

            for (int i = 0; i < zComplex[1].width; i++)
                for (int j = 0; j < zComplex[1].height; j++)
                    zComplex_tmp.array[i, j] = zComplex[1].array[i, j] - zComplex[0].array[i, j];

            zComplex[2] = zComplex_tmp;

        }



        // -------------------------------------------------------------------------------------------------------------------------------
        // Сдвиг голограмм
        //Исходное значение: 4 интерферограммы (PSI) -> 8,9,10,11
        //Задаем X,Y для 1 состояния и X1,Y1 для второго

        //Первое состояние PSI => Область Френеля ->  в zComplex[0]
        //Второе состояние
        //PSI (0,1,2,3) => Область Френеля -> разность фаз ->  zArray(0)
        //PSI (1,2,3,0) => Область Френеля -> разность фаз ->  zArray(1)
        //PSI (2,3,0,1) => Область Френеля -> разность фаз ->  zArray(2)
        public static void Interf_XY(ZComplexDescriptor[] zComplex, ZArrayDescriptor[] zArrayDescriptor,
                                     double[] fz,
                                     double dx, double lambda, double d,
                                     int X, int Y, int X1, int Y1, int N)
        {
            for (int i = 8; i < 12; i++) { if (zArrayDescriptor[8] == null) { MessageBox.Show("zArrayDescriptor[" + i + "] == NULL (Model_object.Interf_XY)"); return; } }
            int w1 = zArrayDescriptor[8].width;
            int h1 = zArrayDescriptor[8].height;

            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];
            for (int k = 0; k < 4; k++) zArray[k] = new ZArrayDescriptor(w1, h1);

            if (X1 + N > w1) { MessageBox.Show(" Размер zArrayDescriptor меньше X(Model_object.Interf_XY)"); return; }
            if (Y1 + N > h1) { MessageBox.Show(" Размер zArrayDescriptor меньше Y(Model_object.Interf_XY)"); return; }

            //MessageBox.Show(" X,Y = " + X+" , "  + Y + " X, Y = " + X1+ " , "  + Y1);



            ZComplexDescriptor zComplex_tmpN = new ZComplexDescriptor(N, N);
            ZComplexDescriptor zComplex_tmp = new ZComplexDescriptor(w1, h1);
            for (int i = 4; i < 8; i++) zArrayDescriptor[i] = new ZArrayDescriptor(N, N);

            zComplex_tmp = ATAN_PSI.ATAN_8_11(8, 9, 10, 11, zArrayDescriptor, fz);                // PSI  0123

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    zComplex_tmpN.array[i, j] = zComplex_tmp.array[i + X, j + Y];
            zComplex[0] = FurieN.FrenelTransformN(zComplex_tmpN, lambda, d, dx);                  // Преобразование Френеля с четным количеством точек 1 состояние => zComplex[0]

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    zComplex_tmpN.array[i, j] = zComplex_tmp.array[i + X1, j + Y1];
            zComplex[1] = FurieN.FrenelTransformN(zComplex_tmpN, lambda, d, dx);                  // Преобразование Френеля с четным количеством точек 0123 => zComplex_tmp
                                                                                                  /* 
                                                                                                              zComplex_tmp = ADD_Math.Div_CMPLX(zComplex[0], zComplex[1]);                         // Разделить два комплексных массива

                                                                                                                        for (int i = 0; i < N; i++)
                                                                                                                            for (int j = 0; j < N; j++)
                                                                                                                                zArrayDescriptor[4].array[i, j] = zComplex_tmp.array[i, j].Phase;

                                                                                                                      zComplex_tmp = ATAN_PSI.ATAN_8_11(9, 10, 11, 8, zArrayDescriptor, fz);                // Второе состояние 1,2,3,0 => zComplex_tmp
                                                                                                                        for (int i = 0; i < N; i++)
                                                                                                                            for (int j = 0; j < N; j++)
                                                                                                                                zComplex_tmpN.array[i, j] = zComplex_tmp.array[i + X1, j + Y1];
                                                                                                                        zComplex_tmp = FurieN.FrenelTransformN(zComplex_tmpN, lambda, d, dx);
                                                                                                                        zComplex_tmp = ADD_Math.Div_CMPLX(zComplex[0], zComplex_tmp);

                                                                                                                        for (int i = 0; i < N; i++)
                                                                                                                            for (int j = 0; j < N; j++)
                                                                                                                                zArrayDescriptor[5].array[i, j] = zComplex_tmp.array[i, j].Phase;

                                                                                                                        zComplex_tmp = ATAN_PSI.ATAN_8_11(10, 11, 8, 9, zArrayDescriptor, fz);                // Второе состояние 2,3,0,1 => zComplex_tmp
                                                                                                                        for (int i = 0; i < N; i++)
                                                                                                                            for (int j = 0; j < N; j++)
                                                                                                                                zComplex_tmpN.array[i, j] = zComplex_tmp.array[i + X1, j + Y1];
                                                                                                                        zComplex_tmp = FurieN.FrenelTransformN(zComplex_tmpN, lambda, d, dx);
                                                                                                                        zComplex_tmp = ADD_Math.Div_CMPLX(zComplex[0], zComplex_tmp);

                                                                                                                        for (int i = 0; i < N; i++)
                                                                                                                            for (int j = 0; j < N; j++)
                                                                                                                                zArrayDescriptor[6].array[i, j] = zComplex_tmp.array[i, j].Phase;

                                                                                                                        zComplex_tmp = ATAN_PSI.ATAN_8_11(11, 8, 9, 10, zArrayDescriptor, fz);                // Второе состояние 3,0,1,2 => zComplex_tmp
                                                                                                                        for (int i = 0; i < N; i++)
                                                                                                                            for (int j = 0; j < N; j++)
                                                                                                                                zComplex_tmpN.array[i, j] = zComplex_tmp.array[i + X1, j + Y1];
                                                                                                                        zComplex_tmp = FurieN.FrenelTransformN(zComplex_tmpN, lambda, d, dx);
                                                                                                                        zComplex_tmp = ADD_Math.Div_CMPLX(zComplex[0], zComplex_tmp);

                                                                                                                        for (int i = 0; i < N; i++)
                                                                                                                            for (int j = 0; j < N; j++)
                                                                                                                                zArrayDescriptor[7].array[i, j] = zComplex_tmp.array[i, j].Phase;

                                                                                                                        zComplex[0] = ATAN_PSI.ATAN_8_11(4, 5, 6, 7, zArrayDescriptor, fz);
                                                                                                                */

        }



        // Разница двух фазовых значений
        public static void Model_Balka(ZArrayDescriptor[] zArrayDescriptor, double L, double Y, int N)
        {

            int LN = 4000;
            int w1 = LN;
            int h1 = 1000;
            double noise = 0;

            ZArrayDescriptor zArray_tmp1 = new ZArrayDescriptor(w1, h1);

            zArrayDescriptor[0] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[1] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[2] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[3] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[4] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[5] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[6] = new ZArrayDescriptor(w1, h1);

            zArrayDescriptor[8] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[9] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[10] = new ZArrayDescriptor(w1, h1);
            zArrayDescriptor[11] = new ZArrayDescriptor(w1, h1);

            // MessageBox.Show(" N = " + N );

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            // Random rnd = new Random();
            // ---------------------------------------------------------------    Фаза от 0 до 2pi без деформаций
            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {

                    double fz1 = (i % N);
                    fz1 = fz1 * 2 * Math.PI / (N);
                    double fa = (0.5 - rnd.NextDouble()) * fz1 * noise;   //rnd.NextDouble() 0-1  
                    fz1 = fz1 + fa;
                    //if (fz1 > 2 * Math.PI) fz1 = 2 * Math.PI;  //fz1 - 2 * Math.PI;
                    //if (fz1 < 0)           fz1 = 0;            // + 2 * Math.PI;
                    zArrayDescriptor[0].array[i, j] = fz1;
                }


            // ---------------------------------------------------------------    Фаза от 0 до max (L) без деформаций
            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {
                    zArrayDescriptor[1].array[i, j] = i * L / w1;
                }

            // ---------------------------------------------------------------    Деформация


            double k = Y / (L * L * L - 3 * L + 2);

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {
                    double e = i * L / LN;                        // X/L

                    double fz1 = e * e * e - 3 * e + 2;

                    zArrayDescriptor[2].array[i, j] = k * fz1;
                }

            // ---------------------------------------------------------------    Деформация с наклоном


            // double k = Y / (L * L * L - 3 * L + 2);

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {


                    zArrayDescriptor[3].array[i, j] = zArrayDescriptor[2].array[i, j] + zArrayDescriptor[1].array[i, j];
                }


            // ---------------------------------------------------------------    Фаза от 0 до 2pi с деформациями
            double XP = N * L / LN;
            ZArrayDescriptor zArray_tmp3 = new ZArrayDescriptor(w1, h1);

            for (int i = 0; i < w1; i++)                                    // Фаза от 0 до 2pi с деформациями
                for (int j = 0; j < h1; j++)
                {
                    double tmp = zArrayDescriptor[3].array[i, j];
                    int ip = Convert.ToInt32(XP * Convert.ToInt32(tmp / XP));
                    double tmp1 = 2 * Math.PI * (tmp - ip) / XP;
                    if (tmp1 < 0) tmp1 = tmp1 + 2 * Math.PI;

                    zArrayDescriptor[4].array[i, j] = tmp1;



                }


            // ---------------------------------------------------------------  Фаза от 0 до 2pi с деформациями
            for (int i = 0; i < w1; i++)                                    // 
                for (int j = 0; j < h1; j++)
                {
                    zArrayDescriptor[5].array[i, j] = zArrayDescriptor[1].array[i, j] - zArrayDescriptor[3].array[i, j];
                }

            // ---------------------------------------------------------------  Производная  => 6

            for (int j = 0; j < h1; j++)
            {
                zArrayDescriptor[6].array[0, j] = zArrayDescriptor[5].array[1, j] - zArrayDescriptor[5].array[0, j];
                for (int i = 1; i < w1 - 1; i++)                                    // 

                {
                    zArrayDescriptor[6].array[i, j] = (zArrayDescriptor[5].array[i - 1, j] - zArrayDescriptor[5].array[i + 1, j]) / 2;
                }
                zArrayDescriptor[6].array[w1 - 1, j] = zArrayDescriptor[6].array[w1 - 2, j];
            }
            // ---------------------------------------------------------------    COS  от разности => 8,9,10,11

            double[] fz = { 0.0, 90.0, 180.0, 270.0 };
            for (int i = 0; i < 4; i++) { fz[i] = fz[i] = Math.PI * fz[i] / 180.0; }  // Фаза в радианах  



            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                {
                    zArrayDescriptor[8].array[i, j] = Math.Cos(zArrayDescriptor[4].array[i, j] - zArrayDescriptor[0].array[i, j] + fz[0]);
                    zArrayDescriptor[9].array[i, j] = Math.Cos(zArrayDescriptor[4].array[i, j] - zArrayDescriptor[0].array[i, j] + fz[1]);
                    zArrayDescriptor[10].array[i, j] = Math.Cos(zArrayDescriptor[4].array[i, j] - zArrayDescriptor[0].array[i, j] + fz[2]);
                    zArrayDescriptor[11].array[i, j] = Math.Cos(zArrayDescriptor[4].array[i, j] - zArrayDescriptor[0].array[i, j] + fz[3]);
                }


        }

        public static ZArrayDescriptor Model_Subr(ZComplexDescriptor zComplex_tmp1, ZComplexDescriptor zComplex_tmp2, double fz)
        {
            int w1 = zComplex_tmp1.width;
            int h1 = zComplex_tmp1.height;
            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);
            for (int i = 0; i < w1; i++) for (int j = 0; j < h1; j++)
                { zArray.array[i, j] = Math.Cos(zComplex_tmp1.array[i, j].Phase - zComplex_tmp2.array[i, j].Phase + fz); }

            return zArray;
        }


        public static void Model_Cos(ZArrayDescriptor[] zArrayDescriptor, double[] fz)
        {

            //double[] fz =  { 0.0, 90.0, 180.0, 270.0 };
            //for (int i = 0; i < 4; i++) { fz[i] = fz[i] = Math.PI * fz[i] / 180.0; }  // Фаза в радианах  

            //double a;
            //double[] fz1 = new double[4];
            //for (int i = 0; i < 4; i++) { fz1[i] = fz[i] ; }  // Фаза в радианах  

            int w1 = zArrayDescriptor[0].width;
            int h1 = zArrayDescriptor[0].height;

            ZComplexDescriptor zComplex_tmp1 = new ZComplexDescriptor(w1, h1);
            ZComplexDescriptor zComplex_tmp2 = new ZComplexDescriptor(w1, h1);
            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);

            zComplex_tmp2 = ATAN_PSI.ATAN_8_11(4, 5, 6, 7, zArrayDescriptor, fz);
            zComplex_tmp1 = ATAN_PSI.ATAN_8_11(0, 1, 2, 3, zArrayDescriptor, fz);                // PSI  0123
            zArrayDescriptor[8] =  Model_Subr(zComplex_tmp1, zComplex_tmp2, Math.PI * 90 / 180.0);
            zArrayDescriptor[9] =  Model_Subr(zComplex_tmp1, zComplex_tmp2, Math.PI * 180 / 180.0);
            zArrayDescriptor[10] = Model_Subr(zComplex_tmp1, zComplex_tmp2, Math.PI * 270 / 180.0);
            zArrayDescriptor[11] = Model_Subr(zComplex_tmp1, zComplex_tmp2, Math.PI * 0 / 180.0);

            /*

                        zComplex_tmp1 = ATAN_PSI.ATAN_8_11(1, 2, 3, 0, zArrayDescriptor, fz1);                // PSI  0123
                        zArrayDescriptor[9] = Model_Subr(zComplex_tmp1, zComplex_tmp2);


                        zComplex_tmp1 = ATAN_PSI.ATAN_8_11(2, 3, 0, 1, zArrayDescriptor, fz1);                // PSI  0123
                        zArrayDescriptor[10] = Model_Subr(zComplex_tmp1, zComplex_tmp2);


                        zComplex_tmp1 = ATAN_PSI.ATAN_8_11(3, 0, 1, 2, zArrayDescriptor, fz1);                // PSI  0123
                        zArrayDescriptor[11] = Model_Subr(zComplex_tmp1, zComplex_tmp2);
            */

        }
        // Вычесть два массива с Cos
        public static ZArrayDescriptor Sub_zArray_Сos(ZArrayDescriptor zArrayDescriptor1, ZArrayDescriptor zArrayDescriptor2)
        {
            double[] fz = { 0.0, 90.0, 180.0, 270.0 };
            for (int i = 0; i < 4; i++) { fz[i] = fz[i] = Math.PI * fz[i] / 180.0; }  // Фаза в радианах  

            int w1 = zArrayDescriptor1.width;
            int h1 = zArrayDescriptor1.height;

            ZArrayDescriptor[] zArray = new ZArrayDescriptor[4];
            for (int i = 0; i < 4; i++) zArray[i] = new ZArrayDescriptor(w1, h1);

            for (int k = 0; k < 4; k++)
                for (int i = 0; i < w1; i++)
                    for (int j = 0; j < h1; j++)
                    { zArray[k].array[i, j] = 0.5 * Math.Cos(zArrayDescriptor1.array[i, j] - zArrayDescriptor2.array[i, j] + fz[k]) + 0.2; }

            double[] fz1 = { 180.0, 270.0, 0.0, 90.0 };
            ZArrayDescriptor Faza = ATAN_PSI.ATAN_Faza(zArray, fz1);

            return Faza;
        }

        //  Скорректировать высоты
        // L         - Расстояние от проектора до объекта
        // d         - Расстояние до камеры от начала объекта
        // d1        - размер объекта
        // x_max     - максимальное смещение

        // Пересечение двух прямых Возвращает y координату пересечения
        private static double line_x(double x11, double y11, double x12, double y12, double x21, double y21, double x22, double y22)
        {

            double a1 = y12 - y11;
            double a2 = y22 - y21;
            double b1 = -(x12 - x11);
            double b2 = -(x22 - x21);
            double c1 = -x11 * (y12 - y11) + y11 * (x12 - x11);
            double c2 = -x21 * (y22 - y21) + y21 * (x22 - x21);

            double z = a1 * b2 - a2 * b1;  // Для x
            //double z = a2 * b1 - a1 * b2;  // Для y
            //double y;              if (z != 0) y = (c2 * a1 - c1 * a2) / (z); else y = 0;
            double x; if (z != 0) x = (c2 * b1 - c1 * b2) / (z); else x = 0;

            return x;
        }

        public static ZArrayDescriptor Sub_Line(ZArrayDescriptor zArrayPicture, int num)  // Вычесть линейный тренд
        {
            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);

            double x1 = 0;
            double x2 = w1 - 1;
            double y1 = zArrayPicture.array[0, num];
            double y2 = zArrayPicture.array[w1 - 1, num];

            for (int j = 0; j < h1; j++)
                for (int i = 0; i < w1; i++)
                {
                    double y = (i - x1) * (y2 - y1) / (x2 - x1) + y1;
                    zArray.array[i, j] = zArrayPicture.array[i, j] - y;
                }


            return zArray;
        }

        // Корректировка по углу
        /// <summary>
        /// num -номер строки
        /// </summary>
        /// <param name="zArrayPicture"></param>
        /// <param name="L"></param>
        /// <param name="d"></param>
        /// <param name="d1"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static ZArrayDescriptor Correct(ZArrayDescriptor zArrayPicture, double L, double d, double d1, double x_max)
        {
            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height;
           
            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);

            //MessageBox.Show(" Расстояние до объекта = " + L + " до камеры от начала объекта = " + d + " размер объекта = " + d1+ " максимальное смещение = " + x_max);

            //double max = double.MinValue;
            //double min = double.MaxValue;

            //double f1 = Math.Atan(L / d);               
            //double f2 = Math.Atan((L-x_max) / (d - d1));
        
            double f1 = Math.Atan((L ) / d);
            double f2 = Math.Atan(L / (d + d1));
            MessageBox.Show(" f1 = " + f1 * 180 / Math.PI + " f2= " + f2 * 180 / Math.PI + " градусов ");

            for (int j = 0; j < h1; j++)                                        // Скорректировать высоты
                for (int i = 0; i < w1; i++)
                 {
                    double ac = zArrayPicture.array[i, j];
                    f2 = Math.Atan((L) / (d + i*d1/w1));
                    //zArray.array[i, j] = ac * Math.Sin(f2 * i / w1 + f1);
                    zArray.array[i, j] = ac*Math.Sin(f2);
                    // zArray.array[i, j] = ac * ( 1 + Math.Sin( (f2-f1)*i/w1) );               
                }

    

            return zArray;
        }

        public static ZArrayDescriptor Count_Null(ZArrayDescriptor zArrayPicture)
        {
            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);

            for (int j = 0; j < h1; j++)
                for (int i = 0; i < w1; i++)
                {
                    if (zArrayPicture.array[i, j] == 0)
                    {
                        int i1 = i;
                        while (i1 >= 1)
                        {
                            i1--;
                            if (zArrayPicture.array[i1, j] != 0) { zArray.array[i, j] = zArrayPicture.array[i1, j]; break; }
                        }
                    }
                    else zArray.array[i, j] = zArrayPicture.array[i, j];

                }


            return zArray;
        }
        // Моделирование теоретического прогиба -----------------------------------------------------------------------------
        /// <summary>
        /// L     - размер балки
        /// x_max - максимальное отклонение
        /// </summary>
        /// <param name="zArrayPicture"></param>
        /// <param name="L"></param>
        /// <param name="X_max"></param>
        /// <returns></returns>
        public static ZArrayDescriptor CorrectX(ZArrayDescriptor zArrayPicture,  double L, double x_max)
        {

            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);

            L = L * 1e-3;
            double L3 = L * L * L;
            double IS = 5.194 * 1e-8;
            double E = 7 * 1e9;
            double P = 4.5;

            double sigma = 0;
            P = (6 * E * IS) * (x_max * 1e-3) /( 2 * L3);  // Определение P, если известен x_max
            MessageBox.Show(" P = " + P);

            double s = (P * L3 )/ (6 * E * IS);

            for (int j = 0; j < h1; j++)
                for (int i = 0; i <w1; i++)
                {
                    double X = (w1-i) * L / (w1-1);
                    sigma = X/L;
                   
                    double Y = s * (sigma*sigma*sigma - 3* sigma + 2);
                  
                    zArray.array[i, j] = Y;
                }


            return zArray;
        }

        public static ZArrayDescriptor Correct_Scale(ZArrayDescriptor zArrayPicture, double x, int n)
        {
            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);
            double max = zArrayPicture.array[0, n];
            double min = zArrayPicture.array[0, n];

            for (int i = 0; i < w1; i++)
            {
                double a = zArrayPicture.array[i, n];
                if (a < min) min = a;
                if (a > max) max = a;
            }

            double x1 = x / (max - min);
            for (int j = 0; j < h1; j++)
                for (int i = 0; i < w1; i++)
                {
                    double a = (zArrayPicture.array[i, j] - min) * x1;
                    if (a < 0) a = 0;
                    if (a > x) a = x;
                    zArray.array[i, j] = a;
                }


            return zArray;
        }
      

        public static void Count_Line(ZArrayDescriptor zArrayPicture, int num)
        {
            int w1 = zArrayPicture.width;
            int h1 = zArrayPicture.height;

            ZArrayDescriptor zArray = new ZArrayDescriptor(w1, h1);

            int j = num;
            int i0 = 0, i1 = 0;
            int i = 0, n=0;

            for (i = 0; i < w1; i++)    { if (zArrayPicture.array[i, j] > 0) { i0 = i; break; } }
            for (i = w1-1; i > i1; i--) { if (zArrayPicture.array[i, j] > 0) { i1 = i; break; } }
            for (i = i0+1; i < i1; i++)  { if (zArrayPicture.array[i, j] > 0) n++;   }

            int t = (i1 - i0) / n;
            MessageBox.Show(" i0= " + i0 +" i1= "+ i1 +" n= "+ n +" Средний период = " + t); 
        }
    }
}
