using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Processing.DataBinding;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class CheckboxBinding : PropertyBindingBase
    {
        private readonly CheckBox _control;
        public override Control Control => _control;
        public CheckboxBinding(CheckboxAttribute checkboxAttribute, MemberInfo memberInfo, object target) : base(checkboxAttribute, memberInfo, target)
        {
            Group = checkboxAttribute.Group;

            _control = new CheckBox
            {
                Checked = (bool) _propertyInfo.GetValue(Target),
                Text = checkboxAttribute.TooltipText
            };

            _control.CheckedChanged += (sender, args) =>
            {
                _propertyInfo.SetValue(Target, _control.Checked);
                OnPropertyChanged();
            };
        }
    }
}
