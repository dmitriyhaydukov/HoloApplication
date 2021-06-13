using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoloCommon.Models.General;

namespace HoloCommon.Models.Charting
{
    public class ChartSeries
    {
        public string Name { get; set; }

        public ColorDescriptor ColorDescriptor { get; set; }

        public List<ChartPoint> Points { get; set; }
    }
}
