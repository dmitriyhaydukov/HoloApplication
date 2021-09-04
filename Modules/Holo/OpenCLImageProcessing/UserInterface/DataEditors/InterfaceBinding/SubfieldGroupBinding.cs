using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Common;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class SubfieldGroupBinding : PropertyBindingBase
    {
        private readonly SubfieldGroupControl _control;
        
        public Binder ComplexFieldBinder { get; private set; }
        public override Control Control => _control;

        public SubfieldGroupBinding(SubfieldGroupAttribute attribute, MemberInfo memberInfo, object target) : base(attribute, memberInfo, target)
        {
            ComplexFieldBinder = new Binder(_propertyInfo.GetValue(Target));
            foreach (var binding in ComplexFieldBinder.Bindings)
            {
                binding.PropertyChanged += OnPropertyChanged;
            }

            _control = new SubfieldGroupControl
            {
                Title = !attribute.TooltipText.IsNullOrEmpty() ? attribute.TooltipText : _propertyInfo.Name
            };
            _control.FillControls(ComplexFieldBinder, true);
        }
    }
}