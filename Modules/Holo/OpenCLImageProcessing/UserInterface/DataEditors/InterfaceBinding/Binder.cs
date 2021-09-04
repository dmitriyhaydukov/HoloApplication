using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public class Binder
    {
        private object _target;

        public IReadOnlyCollection<BindingBase> Bindings { get; private set; }
        public IEnumerable<BindingBase> AllBindings => Bindings.SelectMany(b => b is SubfieldGroupBinding subfieldGroupBinding ? subfieldGroupBinding.ComplexFieldBinder.Bindings : new[] {b});


        public Binder(object target)
        {
            _target = target;
            if (_target == null)
            {
                Bindings = new BindingBase[0];
                return;
            }

            var targetType = target.GetType();
            var members = targetType.GetMembers();

            Bindings = members.Select(m => BindingFactory.CreateFor(m, target))
                .Where(b => b != null)
                .SelectMany(b => b is MergeSubfieldsBinding mergeSubfieldsBinding ? mergeSubfieldsBinding.ComplexFieldBinder.Bindings : new[] {b})
                .ToArray();
        }

        public void FillControls(Panel container)
        {
            foreach (var binding in Bindings)
            {
                binding.Control.Width = container.ClientRectangle.Width;
                container.Controls.Add(binding.Control);
            }
        }

        public PropertyBindingBase GetPropertyBindingWithEmptyValueForType(Type type)
        {
            return AllBindings.OfType<PropertyBindingBase>().FirstOrDefault(b => b.PropertyType.IsAssignableFrom(type) && b.Get() == null);
        }
    }
}
