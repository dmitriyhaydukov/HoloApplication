using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.DataBinding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MergeSubfieldsAttribute : MemberBindingAttributeBase
    {
    }
}
