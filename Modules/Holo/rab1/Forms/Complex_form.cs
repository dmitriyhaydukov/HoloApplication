using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void InterComplex(double am, int Nx, int Ny);
public delegate void InterComplex0(double am, int Nx, int Ny);
public delegate void InterComplexPhase();
public delegate void InterComplex1(int Nx, int Ny);
public delegate void InterComplex2(int Nx, int Ny);
public delegate void InterComplex3(int Nx, int Ny);
public delegate void InterComplex4(int Nx, int Ny);
public delegate void InterComplexPhase_Random();
//public delegate void InterComplex_Cicle();


namespace rab1.Forms
{
    public partial class Complex_form : Form
    {
        private  static int    Nx = 1024;
        private  static int    Ny = 1024;
        private static double  am = 255.0;

        public event InterComplex OnComplex;
        public event InterComplex0 OnComplex0;
        public event InterComplexPhase OnComplexPhase;
        public event InterComplexPhase_Random OnComplexPhase_Random;
        public event InterComplex1 OnNull;
        public event InterComplex2 OnRe;
        public event InterComplex3 OnIm;
        public event InterComplex4 OnReIm0;
        public event InterComplex4 OnAmpl;
        public event InterComplexPhase OnCicle;
        public event InterComplexPhase_Random OnComplexNew;

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;   // Случайные числа

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
       
        private TextBox textBox1;
        private TextBox textBox3;
        private Button button9;
        private Button button10;
        private Button button11;
        private TextBox textBox2;
       




        public Complex_form()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(Nx);
            textBox2.Text = Convert.ToString(Ny);
            textBox3.Text = Convert.ToString(am);
        }

        private void button1_Click(object sender, EventArgs e)   // Фаза по центру
        {
            Nx = Convert.ToInt32(textBox1.Text);
            Ny = Convert.ToInt32(textBox2.Text);
            am= Convert.ToInt32(textBox3.Text);
            OnComplex(am, Nx, Ny);
            Close();
        }

        private void button6_Click(object sender, EventArgs e)  // Фаза в левый угол
        {
            Nx = Convert.ToInt32(textBox1.Text);
            Ny = Convert.ToInt32(textBox2.Text);
            am = Convert.ToInt32(textBox3.Text);
            OnComplex0(am, Nx, Ny);
            Close();
        }


        private void button7_Click(object sender, EventArgs e)  // Фаза в левый угол Амплитуда прежняя
        {    
            OnComplexPhase();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Nx = Convert.ToInt32(textBox1.Text);
            Ny = Convert.ToInt32(textBox2.Text);
            OnNull(Nx, Ny);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Nx = Convert.ToInt32(textBox1.Text);
            Ny = Convert.ToInt32(textBox2.Text);
            OnRe(Nx, Ny);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Nx = Convert.ToInt32(textBox1.Text);
            Ny = Convert.ToInt32(textBox2.Text);
            OnIm(Nx, Ny);
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Nx = Convert.ToInt32(textBox1.Text);
            Ny = Convert.ToInt32(textBox2.Text);
            OnReIm0(Nx, Ny);
            Close();
        }

        private void button10_Click(object sender, EventArgs e)  // Амплитуда по центру
        {
            Nx = Convert.ToInt32(textBox1.Text);
            Ny = Convert.ToInt32(textBox2.Text);
            OnAmpl(Nx, Ny);
            Close();
        }
        //
        // Шаблон фазы по центру заполняется случайными значениями там, где шаблон отличен от нуля
        //
        private void button8_Click(object sender, EventArgs e)
        {
            OnComplexPhase_Random();
            Close();
        }
        //
        // Циклический сдвиг
        //
        private void button9_Click(object sender, EventArgs e)
        {
            OnCicle();
            Close();
        }

        // Автоматическое создание комплексного массива (амплитуда из центрального окна, фаза нулевая, размер по амплитуде)
        private void button11_Click(object sender, EventArgs e)
        {
            OnComplexNew();
            Close();
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(236, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "фазу (по центру) амплитуда прежняя";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(115, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(78, 20);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(306, 31);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(83, 20);
            this.textBox2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Размер по X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Размер по Y";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(41, 254);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(348, 61);
            this.button2.TabIndex = 5;
            this.button2.Text = " нулевой Complex[regComplex] \r\n";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(41, 153);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(238, 26);
            this.button3.TabIndex = 6;
            this.button3.Text = "Re по центру (Im прежний)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(306, 203);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(248, 26);
            this.button4.TabIndex = 7;
            this.button4.Text = "Im по центру (Re прежний)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(475, 31);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(83, 20);
            this.textBox3.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(407, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Амплитуда";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(560, 203);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(179, 26);
            this.button5.TabIndex = 10;
            this.button5.Text = "Re  (Im=0)";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(41, 114);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(238, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = " фазу (в левый угол) амплитуда из TextBox\r\n";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(306, 153);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(248, 23);
            this.button7.TabIndex = 12;
            this.button7.Text = "фаза в левый угол (амплитуда прежняя)";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = " Из  mainPicture (zarray) в Complex[regComplex] ";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(307, 73);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(473, 51);
            this.button8.TabIndex = 14;
            this.button8.Text = "Шаблон (в zArrayPicture).\r\n if (iшаблон > 0)  к фазе добаляются случайные числа. " +
    "Амплитуда прежняя";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(425, 254);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(355, 61);
            this.button9.TabIndex = 15;
            this.button9.Text = "Циклический сдвиг Complex[regComplex] \r\n";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(43, 203);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(238, 26);
            this.button10.TabIndex = 16;
            this.button10.Text = "Амплитуда по центру (Фаза прежняя)";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(41, 331);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(737, 44);
            this.button11.TabIndex = 17;
            this.button11.Text = "Амплитуда из центрального окна (Фаза нулевая) \r\nРазмер по центральному окну";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // Complex_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 410);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Complex_form";
            this.Text = "Complex_form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     
    }
}
