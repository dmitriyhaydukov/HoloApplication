using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace rab1
{
    public class PictureTakenEventArgs
    {
        public Bitmap Image { get; set; }
        public short Number { get; set; }
        public short StartImageNumber { get; set; }
        public short GroupNumber { get; set; }
        public short PhaseShiftValue { get; set; }
        public ColorModeEnum ColorMode { get; set; }
    }

    public delegate void PictureTakenHandler(PictureTakenEventArgs eventArgs);
}
