using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.DataBinding
{
    public class DropdownSelectorAttribute : MemberBindingAttributeBase
    {
        public DropdownSelectorAttribute(string tooltip, string group = "Default")
        {
            TooltipText = tooltip;
            Group = group;
        }
    }
}
