using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace rab1
{
    public class LiveViewUpdatedEventArgs
    {
        public Bitmap Image { get; set; }
        public short PhaseShiftNumber { get; set; }
        public short PhaseShiftValue { get; set; }

        public ColorModeEnum ColorMode { get; set; }
    }

    public delegate void LiveViewUpdatedHandler(LiveViewUpdatedEventArgs eventArgs);
}
