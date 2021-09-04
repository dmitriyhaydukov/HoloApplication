using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding.Controls
{
    public partial class DropdownWithLabel : UserControl
    {
        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        public object SelectedItem
        {
            get => DropDown.SelectedItem;
            set => DropDown.SelectedItem = value;
        }

        public ComboBox.ObjectCollection Items => DropDown.Items;

        public event Action<object, EventArgs> SelectedIndexChanged;

        public DropdownWithLabel()
        {
            InitializeComponent();
            DropDown.SelectedIndexChanged += (sender, args) => SelectedIndexChanged?.Invoke(sender, args);

        }
    }
}
