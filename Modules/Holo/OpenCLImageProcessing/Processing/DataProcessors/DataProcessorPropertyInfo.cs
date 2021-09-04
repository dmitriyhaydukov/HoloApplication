using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Processing.DataProcessors
{
    public enum DataProcessorPropertyDirection
    {
        [Display(Name = "Ввод")]
        Input, 
        [Display(Name = "Вывод")]
        Output
    }
    public class DataProcessorPropertyInfo
    {
        private PropertyInfo _propertyInfo;
        public string Name { get; }
        public string Description { get; }
        public bool Required { get; }
        public DataProcessorPropertyDirection  Direction { get; }
        public Type Type => _propertyInfo.PropertyType;
        public IDataProcessor Owner { get; }

        public DataProcessorPropertyInfo(PropertyInfo propertyInfo, IDataProcessor owner, InputAttribute inputAttribute)
        {
            Owner = owner;
            Name = inputAttribute.Name;
            Description = inputAttribute.Description;
            Required = inputAttribute.Required;
            Direction = DataProcessorPropertyDirection.Input;
            _propertyInfo = propertyInfo;
        }

        public DataProcessorPropertyInfo(PropertyInfo propertyInfo, IDataProcessor owner, OutputAttribute outputAttribute)
        {
            Owner = owner;
            Name = outputAttribute.Name;
            Description = outputAttribute.Description;
            Direction = DataProcessorPropertyDirection.Input;
            _propertyInfo = propertyInfo;
        }

        public object GetValue()
        {
            return _propertyInfo.GetValue(Owner);
        }

        public void SetValue(object value)
        {
            _propertyInfo.SetValue(Owner, value);
        }
    }
}
