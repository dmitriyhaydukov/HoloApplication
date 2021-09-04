using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;

namespace rab1.Forms
{
    class Graphic_util
    {

        public static double[] Graph_x(ZArrayDescriptor zArrayPicture,  int y)
        {
           if (zArrayPicture == null ) { MessageBox.Show("Graph_x zArrayPicture == null");           return null; }
           int w1 = zArrayPicture.width;
           //int h1 = zArrayPicture.height;
           double[] buf = new double[w1];
            
           for (int i = 0; i < w1; i++) buf[i]=zArrayPicture.array[i,y];
           return buf;

        }

        public static double[] Graph_y(ZArrayDescriptor zArrayPicture, int x)
        {
            if (zArrayPicture == null) { MessageBox.Show("Graph_y zArrayPicture == null"); return null; }
           
            int h1 = zArrayPicture.height;
            double[] buf = new double[h1];

            for (int i = 0; i < h1; i++) buf[i] = zArrayPicture.array[x, i];
            return buf;

        }

    }
}
