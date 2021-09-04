using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public delegate void DelegatUnrup(PictureBox pictureBox1, int k1, int k2, int N1_sin, int N2_sin, int scale, int max_diap, int Sdvig_b2);
public delegate void DelegatUnrup_Diag(PictureBox pictureBox1,  int N1_sin, int N2_sin, int scale, int N_diag);
public delegate void DelegatUnrup_Diag256(int N1_sin, int N2_sin, int N_diag);
public delegate void DelegatUnrup_256(int b);
public delegate void DelegatUnrup_Line();

namespace rab1.Forms
{
   

    public partial class UnrupForm : Form
    {
        private static int k1 = 1;
        private static int k2 = 2;
        private static int N1_sin = 167;
        private static int N2_sin = 241;
        private static int scale = 2;
        private static int max_diap = N1_sin*10;
        private static int N_diag = 13;
        private static int Sdvig_b2 = 0;

        public event DelegatUnrup OnUnrup;
        public event DelegatUnrup_Diag OnUnrup_Diag;
        public event DelegatUnrup_Diag OnUnrup_Diag_Tab;
        public event DelegatUnrup_Diag256 OnUnrup_Diag_Tab256;

        public event DelegatUnrup_256 OnUnrup_Tab256;
        public event DelegatUnrup_256 OnUnrup_2pi;
        public event DelegatUnrup_Line OnUnrup_Line;

        public UnrupForm()
        {
            InitializeComponent();
            int X = N2_sin * scale + 10;
            int Y = N1_sin * scale + 10;
            this.pictureBox1.Size = new System.Drawing.Size(X, Y);
            //this.UnrupForm.Size = new System.Drawing.Size(X + 100, Y + 100);

            textBox1.Text = Convert.ToString(k1);
            textBox2.Text = Convert.ToString(k2);
            textBox3.Text = Convert.ToString(N1_sin);
            textBox4.Text = Convert.ToString(N2_sin);
            textBox5.Text = Convert.ToString(scale);
            textBox6.Text = Convert.ToString(max_diap);
            textBox7.Text = Convert.ToString(N_diag);
            textBox8.Text = Convert.ToString(Sdvig_b2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            k1     = Convert.ToInt32(textBox1.Text);
            k2     = Convert.ToInt32(textBox2.Text);
            N1_sin = Convert.ToInt32(textBox3.Text);
            N2_sin = Convert.ToInt32(textBox4.Text);
            scale  = Convert.ToInt32(textBox5.Text);
            max_diap = Convert.ToInt32(textBox6.Text);
            Sdvig_b2 = Convert.ToInt32(textBox8.Text);
            int X = N2_sin * scale + 10;
            int Y = N1_sin * scale + 10;
            this.pictureBox1.Size = new System.Drawing.Size(X, Y);

            OnUnrup(pictureBox1, k1 - 1, k2 - 1, N1_sin, N2_sin, scale, max_diap, Sdvig_b2);
            //Close();

        }

        private void button3_Click(object sender, EventArgs e)  // Построение диагоналей
        {
            N1_sin = Convert.ToInt32(textBox3.Text);
            N2_sin = Convert.ToInt32(textBox4.Text);
            scale = Convert.ToInt32(textBox5.Text);
            N_diag = Convert.ToInt32(textBox7.Text);
            int X = N2_sin * scale + 10;
            int Y = N1_sin * scale + 10;
            this.pictureBox1.Size = new System.Drawing.Size(X, Y);

            OnUnrup_Diag(pictureBox1, N1_sin, N2_sin, scale, N_diag);
        }

        private void button4_Click(object sender, EventArgs e)  // По таблице построить результаты
        {
            N1_sin = Convert.ToInt32(textBox3.Text);
            N2_sin = Convert.ToInt32(textBox4.Text);
            scale = Convert.ToInt32(textBox5.Text);
            N_diag = Convert.ToInt32(textBox7.Text);
            int X = N2_sin * scale + 10;
            int Y = N1_sin * scale + 10;
            this.pictureBox1.Size = new System.Drawing.Size(X, Y);

            OnUnrup_Diag_Tab(pictureBox1, N1_sin, N2_sin, scale, N_diag);
        }

        private void button5_Click(object sender, EventArgs e) // Построить таблицу диагоналей 256 Х 256
        {
            N1_sin = Convert.ToInt32(textBox3.Text);
            N2_sin = Convert.ToInt32(textBox4.Text);
            N_diag = Convert.ToInt32(textBox7.Text);
           
            OnUnrup_Diag_Tab256(N1_sin, N2_sin, N_diag);
            Close();
        }

        private void button6_Click(object sender, EventArgs e) // Изображения из 1 и 2 по таблице в zArrayPicture устранить фазовую неоднозначность
        {
            Sdvig_b2 = Convert.ToInt32(textBox8.Text);
            OnUnrup_Tab256(Sdvig_b2);
            Close();
        }

        private void button7_Click(object sender, EventArgs e) // Ручное устранение 2PI неоднозначности
        {
            Sdvig_b2 = Convert.ToInt32(textBox8.Text);
            OnUnrup_2pi(Sdvig_b2);
           // Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            OnUnrup_Line();
            Close();
        }
    }
}
