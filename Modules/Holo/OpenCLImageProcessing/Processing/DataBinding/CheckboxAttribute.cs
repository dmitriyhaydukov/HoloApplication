using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.DataBinding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckboxAttribute : MemberBindingAttributeBase
    {
        public CheckboxAttribute(string tooltip, string group = "Default")
        {
            TooltipText = tooltip;
            Group = group;
        }
    }
}
