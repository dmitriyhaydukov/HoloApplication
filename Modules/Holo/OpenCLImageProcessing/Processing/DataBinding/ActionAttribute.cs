using System;

namespace Processing.DataBinding
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : MemberBindingAttributeBase
    {
        public ActionAttribute()
        {
        }

        public ActionAttribute(string tooltip)
        {
            TooltipText = tooltip;
        }
    }
}
