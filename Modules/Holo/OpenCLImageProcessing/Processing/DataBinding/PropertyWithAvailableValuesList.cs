using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Processing.DataBinding
{
    
    public interface IPropertyWithAvailableValuesList
    {
        event Action OnAvailableValuesUpdated;
        event Action OnValueCleared;
        event Action<object, object> OnValueSelected;

        object GetValue();
        void SetValue(object value, object sender);
        IEnumerable<object> GetAvailableValues();
    }

    // TODO: ужасное решение, события интерфейса используются со стороны DropdownSelectorBinding в то время как OnValueSelected - со стороны владельца свойства
    // необходимо придумать что-то по-лучше
    public class PropertyWithAvailableValuesList<T>: IPropertyWithAvailableValuesList
    {
        private T _value;
        private readonly IListWithEvents<T> _availableValues;

        public IListWithEvents<T> AvailableValues => _availableValues;

        public event Action OnAvailableValuesUpdated;
        public event Action<object, object> OnValueSelected;
        public event Action OnValueCleared;

        public bool ValueSelected { get; private set; } = false;

        public T Value
        {
            get => _value;
            set => SetValue(value, this);
        }

        public PropertyWithAvailableValuesList() : this(new ListWithEvents<T>())
        {
        }

        public PropertyWithAvailableValuesList(IListWithEvents<T> availableValues)
        {
            _availableValues = availableValues;
            _availableValues.ItemsCleared += AvailableValuesOnItemsCleared;
            _availableValues.ItemAdded += AvailableValuesOnItemAdded;
            _availableValues.ItemsAdded += () => OnAvailableValuesUpdated?.Invoke();
        }

        private void AvailableValuesOnItemAdded(T obj)
        {
            OnAvailableValuesUpdated?.Invoke();
        }

        private void AvailableValuesOnItemsCleared()
        {
            ClearValue();
        }

        public void ClearValue()
        {
            ValueSelected = false;
            OnValueCleared?.Invoke();
        }

        public void SetValue(object value, object sender)
        {
            if (AvailableValues.Contains((T)value))
            {
                _value = (T)value;
                ValueSelected = true;
                OnValueSelected?.Invoke(value, sender);
            }
        }

        public object GetValue()
        {
            return Value;
        }

        public IEnumerable<object> GetAvailableValues() => AvailableValues.Select(v => (object) v);
    }
}
