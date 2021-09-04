using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Processing.DataBinding;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class MergeSubfieldsBinding : PropertyBindingBase
    {
        public Binder ComplexFieldBinder { get; private set; }
        public override Control Control => null;
        public MergeSubfieldsBinding(MergeSubfieldsAttribute mergeSubfieldsAttribute, MemberInfo memberInfo, object target) : base(mergeSubfieldsAttribute, memberInfo, target)
        {
            ComplexFieldBinder = new Binder(_propertyInfo.GetValue(Target));
            foreach (var binding in ComplexFieldBinder.Bindings)
            {
                binding.PropertyChanged += OnPropertyChanged;
            }
        }
    }
}
