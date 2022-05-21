using HoloCommon.Models.General;

namespace HoloCommon.Models.Charting
{
    public class ChartPoint
    {
        public ChartPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
            //this.Color = color;
        }

        public double X { get; set; }
        public double Y { get; set; }

        //public ColorDescriptor Color { get; set; }
    }
}
