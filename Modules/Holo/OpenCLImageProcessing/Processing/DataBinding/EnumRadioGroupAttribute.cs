using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.DataBinding
{
    public class EnumRadioGroupAttribute : MemberBindingAttributeBase
    {
        public EnumRadioGroupAttribute(string tooltip, string group = "Default")
        {
            TooltipText = tooltip;
            Group = group;
        }
    }
}
