using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rab1
{
    public partial class RangeExtensionModelForm : Form
    {
        public RangeExtensionModelForm()
        {
            InitializeComponent();
        }

        private void BuildTableButton_Click(object sender, EventArgs e)
        {
            int m1 = int.Parse(ValueM1TextBox.Text);
            int m2 = int.Parse(ValueM2TextBox.Text);

            int range = int.Parse(RangeTextBox.Text);

            IList<Point2D> pointsList = BuildTable(m1, m2, range);

            GraphInfo graphInfo = new GraphInfo("Graphic", System.Windows.Media.Colors.Green, pointsList.ToArray(), true, false);

            IList<GraphInfo> graphCollection = new List<GraphInfo>() { graphInfo };

            ShowGraphic(graphCollection);
        }

        public List<Point2D> BuildTable(int m1, int m2, int range)
        {
            /*
            ModularArithmeticHelper mah = new ModularArithmeticHelper(m1, m2);

            int b1 = 0;
            int b2 = 16;

            int value = mah.CalculateValue(b1, b2);
            */

            return ModularArithmeticHelper.BuildTable(m1, m2, range);
        }

        private void ValueM1TextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateMaxRange();
        }

        private void ValueM2TextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateMaxRange();
        }
                     
        private void CalculateMaxRange()
        {
            int m1;
            int m2;

            if (int.TryParse(ValueM1TextBox.Text, out m1) && int.TryParse(ValueM2TextBox.Text, out m2))
            {
                int maxRange = m1 * m2;
                MaxRangeValueLabel.Text = maxRange.ToString();
            }
        }

        private void ShowGraphic(IList<GraphInfo> graphCollection)
        {
            GraphFormHost graphFormHost = new GraphFormHost();
            graphFormHost.GraphInfoCollection = graphCollection;

            Form form = new Form();
            form.Height = 300;
            form.Width = 900;
            graphFormHost.Dock = DockStyle.Fill;
            form.Controls.Add(graphFormHost);
            form.Show();
        }
    }
}
