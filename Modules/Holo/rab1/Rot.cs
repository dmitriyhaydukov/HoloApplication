
using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ClassLibrary;

namespace rab1.Forms
{
    class Rot
    {
        public static ZArrayDescriptor Sub_Plane(ZArrayDescriptor amp, int x1, int y1, int x2, int y2, int x3, int y3)     
        {
            int w = amp.width;
            int h = amp.height;
            ZArrayDescriptor res = new ZArrayDescriptor(w, h);


            double ax = x2 - x1, 
                   ay = y2 - y1,
                   az = amp.array[x2, y2] - amp.array[x1, y1];
            double bx = x3 - x1, 
                   by = y3 - y1,
                   bz = amp.array[x3, y3] - amp.array[x1, y1];
            double A = ay * bz - az * by;
            double B = -(ax * bz - bx * az);
            double C = ax * by - ay * bx;
            double D = -(A * x1 + B * y1 + C * amp.array[x1, y1]);
            
            A = A / C; B = B / C; D = D / C;
            MessageBox.Show(" A " + A.ToString() + " B " + B.ToString() + " C " + C.ToString() + " D " + D.ToString());
          
            for (int i = 0; i < w; i++) for (int j = 0; j < h; j++) res.array[i, j] = amp.array[i,j] + (A * i + B * j + D);
           
            return res;
        }





    }
}
