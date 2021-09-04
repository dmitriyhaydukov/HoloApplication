using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Numerics;
using ClassLibrary;

namespace rab1.Forms
{
    class ZIntDescriptor   // Массив с отрицательными индексами
    {
        public int[] array;
        public int ind1;
        public int ind2;

        public ZIntDescriptor()
        {

        }

        public ZIntDescriptor(int i1, int i2)
        {
            ind1 = i1;
            ind2 = i2;
            array = new int[i1 + i2 + 1];                            // [-i1, i2] Всего элементов -2 -1 0 1 2 3 -  i1 + i2 + 1
            for (int j = 0; j < i1 + i2 + 1; j++) array[j] = -1;     // адресация от  -i1 до i2
        }

        public int GetValue(int i1)                                // Взять из массива 
        {
            //int i=i1+ind1;
            //if ( (i1 >= (-ind1 + 1)) && (i1 < ind2+1)) return (array[i1 + ind1]);
            if ((i1 >= -ind1) && (i1 <= ind2)) return (array[i1 + ind1]);
            else
            {
                MessageBox.Show("Get Индексы не в диапазоне i1 = " + i1 + " ind1 = " + (-ind1 ) + " ind2 = " + (ind2 ));
                return (0);
            }
        }

        public void SetValue(int i1, int a)                     // Поместить в массив
        {
            //int i=i1+ind1;
            //if ((i1 >= (-ind1 + 1)) && (i1 < ind2+1)) array[i1 + ind1] = a;
            if ((i1 >= -ind1) && (i1 <= ind2)) array[i1 + ind1] = a;
            else
            {
                MessageBox.Show("Set Индексы не в диапазоне i1 = " + i1 + " ind1 = " + (-ind1) + " ind2 = " + (ind2));
            }
        }

        //---------------------------------------------------------------

    }



}
