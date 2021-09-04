using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void UserChoosedNumber(int number);

namespace rab1.Forms
{
    public partial class ChooseForm : Form
    {
        public int oldValue = 0;
        public event UserChoosedNumber userChoosedNumber;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ChooseForm()
        {
            InitializeComponent();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ChooseForm_Shown(object sender, EventArgs e)
        {
            oldValueTextField.Text = oldValue.ToString();
            newValueTextField.Text = oldValue.ToString();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void okButton_Click(object sender, EventArgs e)
        {
            if (newValueTextField.Text.Equals("") == false)
            {
                int newNumber = Convert.ToInt16(newValueTextField.Text);
                userChoosedNumber(newNumber);
            }

            Close();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void newValueTextField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
