using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace rab1
{
    public class GraphInfo
    {
        //------------------------------------------------------------------------------------------------
        public string GraphName
        {
            get;
            set;
        }
        //------------------------------------------------------------------------------------------------
        public Color GraphColor
        {
            get;
            set;
        }
        //------------------------------------------------------------------------------------------------
        public Point2D[] GraphPoints
        {
            get;
            set;
        }
        //------------------------------------------------------------------------------------------------
        public bool LineVisibility
        {
            get;
            set;
        }
        public bool PointsVisibility
        {
            get;
            set;
        }
        //------------------------------------------------------------------------------------------------
        public GraphInfo(string graphName, Color graphColor, Point2D[] graphPoints, bool lineVisibility, bool pointsVisibility)
        {
            this.GraphName = graphName;
            this.GraphColor = graphColor;
            this.GraphPoints = graphPoints;
            this.LineVisibility = lineVisibility;
            this.PointsVisibility = pointsVisibility;
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
    }
}
