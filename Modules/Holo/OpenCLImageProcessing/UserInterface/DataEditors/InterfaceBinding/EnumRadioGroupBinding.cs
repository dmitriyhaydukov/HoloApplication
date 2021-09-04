using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Processing.DataBinding;
using UserInterface.DataEditors.InterfaceBinding.Controls;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class EnumRadioGroupBinding : PropertyBindingBase
    {
        private readonly EnumRadioGroupControl _enumRadioGroupControl;
        public override Control Control => _enumRadioGroupControl;

        public EnumRadioGroupBinding(EnumRadioGroupAttribute enumRadioGroupAttribute, MemberInfo memberInfo, object target) : base(enumRadioGroupAttribute, memberInfo, target)
        {
            Group = enumRadioGroupAttribute.Group;

            _enumRadioGroupControl = new EnumRadioGroupControl(_propertyInfo.PropertyType)
            {
                Title = enumRadioGroupAttribute.TooltipText,
                Value = (Enum)_propertyInfo.GetValue(target)
            };

            _enumRadioGroupControl.OnValueChanged += () =>
            {
                _propertyInfo.SetValue(Target, _enumRadioGroupControl.Value);
                OnPropertyChanged();
            };
        }
    }
}