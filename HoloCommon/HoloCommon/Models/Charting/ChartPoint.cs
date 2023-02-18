using HoloCommon.Models.General;

namespace HoloCommon.Models.Charting
{
    public class ChartPoint
    {
        public ChartPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
