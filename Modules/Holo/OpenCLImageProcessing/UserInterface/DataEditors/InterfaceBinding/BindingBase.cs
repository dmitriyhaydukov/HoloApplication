using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Common;
using Processing.DataBinding;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public abstract class BindingBase
    {
        private Action _onPropertyChanged;
        protected MemberInfo MemberInfo;
        protected object Target;
        public abstract Control Control { get; }
        public string Group { get; protected set; }
        public event Action PropertyChanged;

        protected BindingBase(MemberBindingAttributeBase bindingAttributeBase, MemberInfo memberInfo, object target)
        {
            MemberInfo = memberInfo;
            Target = target;
            Group = bindingAttributeBase.Group;

            var targetType = target.GetType();
            if (!bindingAttributeBase.OnPropertyChanged.IsNullOrEmpty())
            {
                var methodInfo = targetType.GetMethod(bindingAttributeBase.OnPropertyChanged);
                if (methodInfo != null)
                    _onPropertyChanged = () =>
                    {
                        try
                        {
                            methodInfo.Invoke(target, null);
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.Exception(ex);
                        }
                    };
            }

            if (!bindingAttributeBase.PropertyChangedEventName.IsNullOrEmpty())
            {
                var eventInfo = targetType.GetEvent(bindingAttributeBase.PropertyChangedEventName);
                if (eventInfo != null)
                    eventInfo.AddEventHandler(target, Delegate.CreateDelegate(typeof(Action), this, nameof(OnPropertyChangedByTarget)));
            }
        }

        protected void ThrowCantCreateBindingForMemberTypeException()
        {
            throw new InvalidOperationException($"Can't create {GetType().Name} for {MemberInfo.MemberType} member.");
        }

        protected void OnPropertyChanged()
        {
            PropertyChanged?.Invoke();
            _onPropertyChanged?.Invoke();
        }

        // Метод выполняется при изменении свойства связанным объектом
        protected virtual void OnPropertyChangedByTarget()
        {
            
        }
    }

    public abstract class PropertyBindingBase : BindingBase
    {
        protected PropertyInfo _propertyInfo;

        public Type PropertyType => _propertyInfo.PropertyType;

        protected PropertyBindingBase(MemberBindingAttributeBase bindingAttributeBase, MemberInfo memberInfo, object target) : base(bindingAttributeBase, memberInfo, target)
        {
            if (memberInfo.MemberType != MemberTypes.Property)
                ThrowCantCreateBindingForMemberTypeException();

            _propertyInfo = memberInfo.DeclaringType.GetProperty(memberInfo.Name);
        }

        public object Get()
        {
            return _propertyInfo.GetValue(Target);
        }

        public T Get<T>()
        {
            return (T) Get();
        }

        public virtual void Set(object value)
        {
            throw new NotImplementedException();
        }
    }

    public static class BindingFactory
    {
        public static BindingBase CreateFor(MemberInfo memberInfo, object target)
        {
            var interfaceBindingAttribute = memberInfo.GetCustomAttribute<MemberBindingAttributeBase>();

            if (interfaceBindingAttribute is ActionAttribute actionAttribute)
                return new ActionBinding(actionAttribute, memberInfo, target);

            if (interfaceBindingAttribute is NumberAttribute sliderAttribute)
                return new NumberBinding(sliderAttribute, memberInfo, target);

            if (interfaceBindingAttribute is SubfieldGroupAttribute complexFieldAttribute)
                return new SubfieldGroupBinding(complexFieldAttribute, memberInfo, target);

            if (interfaceBindingAttribute is ImageSlotWithSubfieldsAttribute imageSlotWithSubfieldsAttribute)
                return new ImageSlotWithSubfieldsBinder(imageSlotWithSubfieldsAttribute, memberInfo, target);

            if (interfaceBindingAttribute is ImageSlotAttribute imageSlotAttribute)
                return new ImageSlotBinder(imageSlotAttribute, memberInfo, target);

            if (interfaceBindingAttribute is EnumRadioGroupAttribute enumRadioGroupAttribute)
                return new EnumRadioGroupBinding(enumRadioGroupAttribute, memberInfo, target);

            if (interfaceBindingAttribute is MergeSubfieldsAttribute mergeSubfieldsAttribute)
                return new MergeSubfieldsBinding(mergeSubfieldsAttribute, memberInfo, target);

            if (interfaceBindingAttribute is CheckboxAttribute checkboxAttribute)
                return new CheckboxBinding(checkboxAttribute, memberInfo, target);

            if (interfaceBindingAttribute is DropdownSelectorAttribute dropdownSelectorAttribute)
                return new DropdownSelectorBinding(dropdownSelectorAttribute, memberInfo, target);

            return null;
        }
    }
}