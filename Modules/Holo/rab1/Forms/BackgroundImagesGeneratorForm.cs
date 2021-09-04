using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

public delegate void OneImageOfSeries(Image newImage, int imageNumber);

namespace rab1
{
    public partial class BackgroundImagesGeneratorForm : Form
    {
        private int stripOrientation = 0;
        private int stripType = 0;
        private int imageWidth = 800;
        private int imageHeight = 600;
        private int imageNumber = 0;

        private double numberOfSin1Value;
        private double numberOfSin2Value;

        private int phaseShift1Value;
        private int phaseShift2Value;
        private int phaseShift3Value;
        private int phaseShift4Value;

        private Form formForStripes;
        private CustomPictureBox pc1;

        public int numberOfImageInSeries = 8;
        public event OneImageOfSeries oneImageOfSeries;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public BackgroundImagesGeneratorForm()
        {
            InitializeComponent();

            numberOfSin1.Text = "167";
            numberOfSin2.Text = "241";

            phaseShift1.Text = "0";
            phaseShift2.Text = "90";
            phaseShift3.Text = "180";
            phaseShift4.Text = "270";

            //ShooterSingleton.init();

            convertValues();

            formForStripes = new Form();
            formForStripes.Size = new Size(800 + 8, 600 + 8);
            formForStripes.StartPosition = FormStartPosition.Manual;

            pc1 = new CustomPictureBox();
            pc1.BackColor = Color.White;
            pc1.Location = new Point(0, 8);
            pc1.Size = new Size(800, 600);
            pc1.SizeMode = PictureBoxSizeMode.StretchImage;
            pc1.BorderStyle = BorderStyle.Fixed3D;

            formForStripes.Controls.Add(pc1);

            updateInitialImage();
            
            pc1.Refresh();
            formForStripes.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void updateInitialImage()
        {
            convertValues();

            if (stripType == 0)
            {
                SinClass1.sin_f(numberOfSin1Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation, pc1);
            }
            else if (stripType == 1)
            {

            }
            else if (stripType == 2)
            {
                SinClass1.drawLines(numberOfSin1Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation, pc1);
            }
            else if (stripType == 3)
            {
                SinClass1.drawDitheredLines(numberOfSin1Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation, pc1);
            }

            pc1.Invalidate();
            pc1.Update();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void convertValues()
        {
            numberOfSin1Value = Convert.ToDouble(numberOfSin1.Text);
            numberOfSin2Value = Convert.ToDouble(numberOfSin2.Text);

            phaseShift1Value = Convert.ToInt16(phaseShift1.Text);
            phaseShift2Value = Convert.ToInt16(phaseShift2.Text);
            phaseShift3Value = Convert.ToInt16(phaseShift3.Text);
            phaseShift4Value = Convert.ToInt16(phaseShift4.Text);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void okClicked(object sender, EventArgs e)
        {
            convertValues();
            imageNumber = 0;

           // ShooterSingleton.imageCaptured += imageTaken;
            //ShooterSingleton.getImage();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void imageTaken(Image newImage)
        {
            Thread.Sleep(1000);

            imageNumber++;

            if (oneImageOfSeries != null)
            {
                oneImageOfSeries(newImage, imageNumber);
            }

            if (imageNumber >= numberOfImageInSeries)
            {
               // ShooterSingleton.imageCaptured -= imageTaken;
            }
            else
            {
                if (imageNumber == 1)
                {
                    if (stripType == 0)
                    {
                        SinClass1.sin_f(numberOfSin1Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 2)
                    {
                        SinClass1.drawLines(numberOfSin1Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 3)
                    {
                        SinClass1.drawDitheredLines(numberOfSin1Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                }
                else if (imageNumber == 2)
                {
                    if (stripType == 0)
                    {
                        SinClass1.sin_f(numberOfSin1Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 2)
                    {
                        SinClass1.drawLines(numberOfSin1Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 3)
                    {
                        SinClass1.drawDitheredLines(numberOfSin1Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                }
                else if (imageNumber == 3)
                {
                    if (stripType == 0)
                    {
                        SinClass1.sin_f(numberOfSin1Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 2)
                    {
                        SinClass1.drawLines(numberOfSin1Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 3)
                    {
                        SinClass1.drawDitheredLines(numberOfSin1Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                }
                else if (imageNumber == 4)
                {
                    if (stripType == 0)
                    {
                        SinClass1.sin_f(numberOfSin2Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 2)
                    {
                        SinClass1.drawLines(numberOfSin2Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 3)
                    {
                        SinClass1.drawDitheredLines(numberOfSin2Value / 10, phaseShift1Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                }
                else if (imageNumber == 5)
                {
                    if (stripType == 0)
                    {
                        SinClass1.sin_f(numberOfSin2Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 2)
                    {
                        SinClass1.drawLines(numberOfSin2Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 3)
                    {
                        SinClass1.drawDitheredLines(numberOfSin2Value / 10, phaseShift2Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                }
                else if (imageNumber == 6)
                {
                    if (stripType == 0)
                    {
                        SinClass1.sin_f(numberOfSin2Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 2)
                    {
                        SinClass1.drawLines(numberOfSin2Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 3)
                    {
                        SinClass1.drawDitheredLines(numberOfSin2Value / 10, phaseShift3Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                }
                else if (imageNumber == 7)
                {
                    if (stripType == 0)
                    {
                        SinClass1.sin_f(numberOfSin2Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 2)
                    {
                        SinClass1.drawLines(numberOfSin2Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                    else if (stripType == 3)
                    {
                        SinClass1.drawDitheredLines(numberOfSin2Value / 10, phaseShift4Value, imageWidth, imageHeight, stripOrientation, pc1);
                    }
                }

                pc1.Invalidate();
                pc1.Update();

                Thread.Sleep(1000);


               // ShooterSingleton.getImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripOrientation = 0;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripOrientation = 1;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripType = 0;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripType = 1;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                stripType = 2;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BackgroundImagesGeneratorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ShooterSingleton.imageCaptured -= imageTaken;
            this.Dispose();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                stripType = 3;
                updateInitialImage();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
