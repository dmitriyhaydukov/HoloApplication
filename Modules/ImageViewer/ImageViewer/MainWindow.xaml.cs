using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HoloCommon.Serialization.Imaging;
using HoloCommon.MemoryManagement;

namespace ImageViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WriteableBitmap writeableBitmap = MemoryReader.Read<WriteableBitmap>(new WriteableBitmapSerialization());
            this.MainImage.Source = writeableBitmap;
        }
    }
}
