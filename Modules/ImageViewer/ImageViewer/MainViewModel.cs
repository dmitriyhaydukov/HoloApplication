using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Microsoft.Practices.Prism.ViewModel;

namespace ImageViewer
{
    public class MainViewModel : NotificationObject
    {
        private BitmapSource mainImageSource;

        public BitmapSource MainImageSource
        {
            get
            {
                return this.mainImageSource;
            }
            set
            {
                this.mainImageSource = value;
                this.RaisePropertyChanged(() => this.MainImageSource);
            }
        }
    }
}