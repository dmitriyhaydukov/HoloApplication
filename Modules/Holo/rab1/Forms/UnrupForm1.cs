using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public delegate void DelegatUnrup1( int k1, int k2, int k3, int N1_sin, int N2_sin, int N_diag, int sdvig, int Mngtl);

namespace rab1.Forms
{
    public partial class UnrupForm1 : Form
    {
        private static int k1 = 1;
        private static int k2 = 2;
        private static int k3 = 3;
        private static int N1_sin = 167;
        private static int N2_sin = 241;
        private static int Mngtl = 1;
        private static int N_diag = 13;
        private static int sdvig = 0;

        public event DelegatUnrup1 OnUnrup1;
        
        public UnrupForm1()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(N1_sin);
            textBox2.Text = Convert.ToString(N2_sin);
            textBox8.Text = Convert.ToString(Mngtl);
          
            textBox3.Text = Convert.ToString(k1);
            textBox4.Text = Convert.ToString(k2);
            textBox5.Text = Convert.ToString(k3);
            textBox6.Text = Convert.ToString(N_diag);
            textBox7.Text = Convert.ToString(sdvig);
        }

        private void button1_Click(object sender, EventArgs e)  // Развертка
        {
            N1_sin = Convert.ToInt32(textBox1.Text);
            N2_sin = Convert.ToInt32(textBox2.Text);
            Mngtl  = Convert.ToInt32(textBox8.Text);
            
            k1     = Convert.ToInt32(textBox3.Text);
            k2     = Convert.ToInt32(textBox4.Text);
            k3     = Convert.ToInt32(textBox5.Text);
            N_diag = Convert.ToInt32(textBox6.Text);
            sdvig  = Convert.ToInt32(textBox7.Text);
            OnUnrup1(k1 - 1, k2 - 1, k3-1, N1_sin, N2_sin, N_diag, sdvig, Mngtl);
            Close();
            
           
        }
    }
}
