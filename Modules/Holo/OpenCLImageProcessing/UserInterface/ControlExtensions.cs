using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    public static class ControlExtensions
    {
        // https://social.msdn.microsoft.com/Forums/windows/en-US/80c6894a-e3b7-4376-8020-43d4ed97b1ee/how-to-prevent-designer-from-executing-code-in-a-usercontrols-load-event?forum=winformsdesigner
        public static bool HasDesigner(this Control self)
        {
            /*if (self == null)
                return false;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;
            Control p = self.Parent;
            while (p != null)
            {
                // System.Windows.Forms.Design.DesignerFrame ... an internal type to System.Design
                if (p.GetType().FullName.Contains(".DesignerFrame"))
                    return true;

                p = p.Parent;
            }
            return false;*/
            return IsInDesignMode(self);
        }

        /// <summary>
        /// Method to test if the control or it's parent is in design mode
        /// </summary>
        /// <param name="control">Control to examine</param>
        /// <returns>True if in design mode, otherwise false</returns>
        private static bool IsInDesignMode(System.Windows.Forms.Control control)
        {
            var ctrl = control;
            while (ctrl != null)
            {
                if ((ctrl.Site != null) && ctrl.Site.DesignMode)
                    return true;
                ctrl = ctrl.Parent;
            }
            return false;
        }
    }
}
