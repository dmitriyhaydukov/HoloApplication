using System;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;

public delegate void ImageBox(double am, double AngleX, double AngleY, double Lambda, int NX, int NY, double dx, int k1);
public delegate void ImageBoxADD(double am, double AngleX, double AngleY, double Lambda, double dx, double noise, double fz);

public delegate void ImageBoxSUB(double am, double AngleX, double AngleY, double Lambda, double dx, int k1);
public delegate void ImageBoxMUL(double am, double AngleX, double AngleY, double Lambda, double dx, int k1);
public delegate void ImageBoxPSI(double am, double AngleX, double AngleY, double Lambda, double dx, double noise, double [] fz);
public delegate void ImageBoxSUB1(double AngleX, double AngleY);
public delegate void ImageBoxNoise(double noise);

//                                  Сложение с опорной волной 
namespace rab1.Forms
{
      
    public partial class InterForm : Form
    {
        
        public event ImageBox    OnBox ;
        public event ImageBoxADD OnBoxADD;
        public event ImageBoxADD OnBoxADD_I;

        public event ImageBoxSUB OnBoxSUB;
        public event ImageBoxMUL OnBoxMUL;
        public event ImageBoxPSI OnBoxPSI;
        public event ImageBoxSUB1   OnBoxSUB1;
        public event ImageBoxNoise OnBoxNoise;
        public event ImageBoxNoise OnBoxMove;

        private static double AngleX = 0.7;
        private static double AngleY = 0.7;
        private static double Lambda = 0.5;
        private static int NX = 1024;
        private static int NY = 1024;
        private static double dx = 7;
        private static int k1 = 1;
        private static double am = 255;
        private static double[] fz = { 0.0, 90.0, 180.0, 270.0 };
        private static double noise = 0;

        private Button button1;
       // private Button button2;
        private Button button3;
        private Button button2;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
       
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;

        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox textBox10;
        private TextBox textBox11;     
        private TextBox textBox13;
        private Label label11;
        private Button button8;
        private Button button9;
        private TextBox textBox12;

        public InterForm()
        {
            InitializeComponent();
            textBox1.Text = Convert.ToString(AngleX);
            textBox2.Text = Convert.ToString(AngleY);
           
            textBox3.Text = Convert.ToString(NX);
            textBox4.Text = Convert.ToString(NY);
            textBox5.Text = Convert.ToString(Lambda);

            textBox6.Text = Convert.ToString(dx);
            textBox7.Text = Convert.ToString(k1);
            textBox8.Text = Convert.ToString(am);

            textBox9.Text = Convert.ToString(fz[0]);
            textBox10.Text = Convert.ToString(fz[1]);
            textBox11.Text = Convert.ToString(fz[2]);
            textBox12.Text = Convert.ToString(fz[3]);

            textBox13.Text = Convert.ToString(noise);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double AngX, AngY;

            AngleX = Convert.ToDouble(textBox1.Text);
            AngleY = Convert.ToDouble(textBox2.Text);

            AngX = Math.PI * AngleX / 180.0;
            AngY = Math.PI * AngleY / 180.0;

           
           
            NX = Convert.ToInt32(textBox3.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            NY = Convert.ToInt32(textBox4.Text);
            dx = Convert.ToDouble(textBox6.Text);
            k1 = Convert.ToInt32(textBox7.Text);
            am = Convert.ToDouble(textBox8.Text);

            //--------------------------------------------------------------------------------------
            double X = Lambda *NX/ (2 *dx*1000* Math.Sin(AngX / 2));
            double N = 3*2 *dx*1000* Math.Sin(AngX / 2)/Lambda;
            MessageBox.Show("ANx(радианы)= " + Convert.ToString(AngX) + "Nx(между полосами)= " + Convert.ToString(X)+"NMAX= " + Convert.ToString(N));
            OnBox(am, AngX, AngY, Lambda, NX, NY, dx * 1000.0, k1);
            Close();
        }

        private void button3_Click(object sender, EventArgs e)   // ADD + fz[0]
        {
            double AngX, AngY;

            AngleX = Convert.ToDouble(textBox1.Text);
            AngleY = Convert.ToDouble(textBox2.Text);

            AngX =  Math.PI * AngleX / 180.0;
            AngY =  Math.PI * AngleY / 180.0;


            NX = Convert.ToInt32(textBox3.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            NY = Convert.ToInt32(textBox4.Text);
            dx = Convert.ToDouble(textBox6.Text);
            k1 = Convert.ToInt32(textBox7.Text);
            am = Convert.ToDouble(textBox8.Text);
            noise = Convert.ToDouble(textBox13.Text);

            fz[0] = Convert.ToDouble(textBox9.Text);
            double fzr = Math.PI * fz[0] / 180.0;   // Фаза в радианах  

            //MessageBox.Show("Введите порядок фильтрации");
            OnBoxADD(am, AngX, AngY, Lambda, dx * 1000.0, noise/100, fzr);
            Close();

        }


        private void button8_Click(object sender, EventArgs e)   // ADD + fz[0] => в центральное окно
        {
            double AngX, AngY;

            AngleX = Convert.ToDouble(textBox1.Text);
            AngleY = Convert.ToDouble(textBox2.Text);

            AngX = Math.PI * AngleX / 180.0;
            AngY = Math.PI * AngleY / 180.0;


            NX = Convert.ToInt32(textBox3.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            NY = Convert.ToInt32(textBox4.Text);
            dx = Convert.ToDouble(textBox6.Text);
            k1 = Convert.ToInt32(textBox7.Text);
            am = Convert.ToDouble(textBox8.Text);
            noise = Convert.ToDouble(textBox13.Text);

            fz[0] = Convert.ToDouble(textBox9.Text);
            double fzr = Math.PI * fz[0] / 180.0;   // Фаза в радианах  

            //MessageBox.Show("Введите порядок фильтрации");
            OnBoxADD_I(am, AngX, AngY, Lambda, dx * 1000.0, noise / 100, fzr);
            Close();

        }
        private void button5_Click(object sender, EventArgs e) // SUB
        {
            double AngX, AngY;

            AngleX = Convert.ToDouble(textBox1.Text);
            AngleY = Convert.ToDouble(textBox2.Text);

            AngX = Math.PI * AngleX / 180.0;
            AngY = Math.PI * AngleY / 180.0;


            NX = Convert.ToInt32(textBox3.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            NY = Convert.ToInt32(textBox4.Text);
            dx = Convert.ToDouble(textBox6.Text);
            k1 = Convert.ToInt32(textBox7.Text);
            am = Convert.ToDouble(textBox8.Text);


            //MessageBox.Show("Введите порядок фильтрации");
            OnBoxSUB(am, AngX, AngY, Lambda, dx * 1000.0, k1);
            Close();
        }



        private void button2_Click(object sender, EventArgs e)  // MUL
        {
            double AngX, AngY;

            AngleX = Convert.ToDouble(textBox1.Text);
            AngleY = Convert.ToDouble(textBox2.Text);

            AngX = Math.PI * AngleX / 180.0;
            AngY = Math.PI * AngleY / 180.0;


            NX = Convert.ToInt32(textBox3.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            NY = Convert.ToInt32(textBox4.Text);
            dx = Convert.ToDouble(textBox6.Text);
            k1 = Convert.ToInt32(textBox7.Text);
            am = Convert.ToDouble(textBox8.Text);
            
            //MessageBox.Show("Введите порядок фильтрации");
            OnBoxMUL(am, AngX, AngY, Lambda, dx * 1000.0, k1);
            Close();

        }
        
        private void button4_Click(object sender, EventArgs e)  // PSI
        {
            double AngX, AngY;

            AngleX = Convert.ToDouble(textBox1.Text);
            AngleY = Convert.ToDouble(textBox2.Text);

            AngX = Math.PI * AngleX / 180.0;
            AngY = Math.PI * AngleY / 180.0;


            //NX = Convert.ToInt32(textBox3.Text);
            Lambda = Convert.ToDouble(textBox5.Text);
            //NY = Convert.ToInt32(textBox4.Text);
            dx = Convert.ToDouble(textBox6.Text);
            //k1 = Convert.ToInt32(textBox7.Text);
            am = Convert.ToDouble(textBox8.Text);

            double[] fzrad = new double[4];
            fz[0] = Convert.ToDouble(textBox9.Text);  fzrad[0] = Math.PI * fz[0] / 180.0;   // Фаза в радианах  
            fz[1] = Convert.ToDouble(textBox10.Text); fzrad[1] = Math.PI * fz[1] / 180.0;
            fz[2] = Convert.ToDouble(textBox11.Text); fzrad[2] = Math.PI * fz[2] / 180.0;
            fz[3] = Convert.ToDouble(textBox12.Text); fzrad[3] = Math.PI * fz[3] / 180.0;        
           
                             
            noise = Convert.ToDouble(textBox13.Text);   // Шум в процентах

            //MessageBox.Show("Введите порядок фильтрации");
            OnBoxPSI(am, AngX, AngY, Lambda, dx * 1000.0, noise / 100, fzrad);
            Close();

        }

        // Из текущего окна в главное с масштабированием
        private void button9_Click(object sender, EventArgs e) 
        {
            am = Convert.ToDouble(textBox8.Text);
            OnBoxMove(am);
            Close();
        }
        //---------------------------------------------------------------------
        //               Вычесть отрицательный наклон
        //--------------------------------------------------------------------

        private void button6_Click(object sender, EventArgs e)
        {
            double AngX, AngY;

            AngleX = Convert.ToDouble(textBox1.Text);
            AngleY = Convert.ToDouble(textBox2.Text);

            AngX = Math.PI * AngleX / 180.0;
            AngY = Math.PI * AngleY / 180.0;

            //MessageBox.Show("Введите порядок фильтрации");
            OnBoxSUB1(AngX, AngY);
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            noise = Convert.ToDouble(textBox13.Text);   // Шум в процентах
            OnBoxNoise(noise/100);
            Close();
        }
        
        
        
        //-----------------------------------------------------------------------------
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(257, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Поместить  в";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Угол по X (град) -";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Угол по Y (град) -";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(194, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(435, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(94, 20);
            this.textBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(308, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Число пикселов по X -";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(308, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Число пикселей по Y -";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(435, 46);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(94, 20);
            this.textBox3.TabIndex = 7;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(435, 74);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(94, 20);
            this.textBox4.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Длина волны (мкм) -";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(194, 46);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Размер изображения по X (мм) -";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(194, 86);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(382, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Complex";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(456, 210);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(49, 20);
            this.textBox7.TabIndex = 14;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 170);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(257, 53);
            this.button3.TabIndex = 15;
            this.button3.Text = "Сложить zComplex[1] с плоской волной + fz[0]\r\nи поместить в  zComplex[2]  ";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(382, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Амплитуда -";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(456, 141);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(49, 20);
            this.textBox8.TabIndex = 17;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 365);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(260, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Умножить ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(297, 305);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(232, 51);
            this.button4.TabIndex = 19;
            this.button4.Text = "Сложить  с zComplex[1]  (с шумом фазы)\r\n и поместить амплитуды  в  8 9 10 11  ";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(311, 261);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(49, 20);
            this.textBox9.TabIndex = 20;
            this.textBox9.Text = "0";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(363, 261);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(49, 20);
            this.textBox10.TabIndex = 21;
            this.textBox10.Text = "90";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(418, 261);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(49, 20);
            this.textBox11.TabIndex = 22;
            this.textBox11.Text = "180";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(473, 261);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(49, 20);
            this.textBox12.TabIndex = 23;
            this.textBox12.Text = "270";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(15, 333);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(260, 23);
            this.button5.TabIndex = 24;
            this.button5.Text = "Вычесть фазу опоры";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(15, 394);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(260, 23);
            this.button6.TabIndex = 25;
            this.button6.Text = "Вычесть отрицательный наклон";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(382, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Шум -";
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(456, 170);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(49, 20);
            this.textBox13.TabIndex = 27;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(514, 173);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(15, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "%";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(343, 118);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(107, 23);
            this.button7.TabIndex = 29;
            this.button7.Text = "Добавить шум";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(340, 240);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "fz[i]";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(15, 229);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(257, 88);
            this.button8.TabIndex = 31;
            this.button8.Text = "Сложить zComplex[1] с плоской волной\r\n +  шум + fz[0]\r\nи поместить в интенсивност" +
    "ь в центральное окно\r\nПомасштабировать от 0 до Амплитуда";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(297, 366);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(232, 51);
            this.button9.TabIndex = 32;
            this.button9.Text = "Из текущего окна в главное\r\nс масштабированием от 0 до Амплитуда";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // InterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 429);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "InterForm";
            this.Text = "Интерференция двух пучков";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      
    }
}
