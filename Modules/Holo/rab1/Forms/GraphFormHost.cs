using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace rab1
{
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]
    public class GraphFormHost : System.Windows.Forms.Integration.ElementHost
    {
        protected SwordfishXYLineChartControl chartControl = new SwordfishXYLineChartControl();

        public GraphFormHost()
        {
            base.Child = chartControl;
        }

        public AxesInfo GraphAxesInfo
        {
            get
            {
                return chartControl.GraphAxesInfo;
            }
            set
            {
                chartControl.GraphAxesInfo = value;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------
        public IList<GraphInfo> GraphInfoCollection
        {
            get
            {
                return chartControl.GraphInfoCollection;
            }
            set
            {
                chartControl.GraphInfoCollection = value;
            }
        }

        public string Title
        {
            get
            {
                return chartControl.Title;
            }
            set
            {
                chartControl.Title = value;
            }
        }
    }
}