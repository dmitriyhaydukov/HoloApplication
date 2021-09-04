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
    public class NumberBinding : PropertyBindingBase
    {
        private readonly NumberControl _control;
        public override Control Control => _control;

        public NumberBinding(NumberAttribute numberAttribute, MemberInfo memberInfo, object target) : base(numberAttribute, memberInfo, target)
        {
            Group = numberAttribute.Group;

            _control = new NumberControl
            {
                Text = numberAttribute.TooltipText,
                Title = numberAttribute.TooltipText,
                Minimum = numberAttribute.Min,
                Maximum = numberAttribute.Max,
                Step = numberAttribute.Step,
                Value = (float) _propertyInfo.GetValue(Target)
            };

            _control.ValueChanged += () =>
            {
                _propertyInfo.SetValue(Target, _control.Value);
                OnPropertyChanged();
            };
        }
    }
}