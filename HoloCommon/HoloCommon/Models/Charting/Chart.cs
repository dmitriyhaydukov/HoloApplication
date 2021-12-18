using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoloCommon.Models.Charting
{
    public class Chart
    {
        public List<ChartSeries> SeriesCollection { get; set; }
        public bool InvertAxisX { get; set; }
        public bool InvertAxisY { get; set; }
    }
}
