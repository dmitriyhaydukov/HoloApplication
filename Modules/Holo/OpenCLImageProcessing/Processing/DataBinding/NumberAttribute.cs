using System;

namespace Processing.DataBinding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NumberAttribute : MemberBindingAttributeBase
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public float Step { get; set; }

        public NumberAttribute(string tooltip, float min, float max, float step, string group = "Default")
        {
            TooltipText = tooltip;
            Min = min;
            Max = max;
            Step = step;
            Group = group;
        }
    }
}