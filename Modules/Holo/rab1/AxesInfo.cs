using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rab1
{
    public class AxesInfo
    {
        //-------------------------------------------------------------------------
        public string AxisTitleX
        {
            get;
            set;
        }
        //-------------------------------------------------------------------------
        public string AxisTitleY
        {
            get;
            set;
        }
        //-------------------------------------------------------------------------
        public AxesInfo(string axisTitleX, string axisTitleY)
        {
            this.AxisTitleX = axisTitleX;
            this.AxisTitleY = axisTitleY;
        }
        //-------------------------------------------------------------------------
    }
}
