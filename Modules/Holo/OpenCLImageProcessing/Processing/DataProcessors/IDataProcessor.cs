using System;
using System.Reflection;

namespace Processing.DataProcessors
{
    public interface IDataProcessor : IDisposable
    {
        event Action Updated;
        event Action<IImageHandler> OnImageFinalize;
        event Action<IImageHandler> OnImageCreate;
        void Initialize();
        void InputUpdated(PropertyInfo propertyInfo);
        void Awake();
        void FreeResources();
    }
    
    public abstract class ProcessorPropertyAttributeBase : Attribute
    {
        public string Name { get; }
        public string Description { get; }

        protected ProcessorPropertyAttributeBase(string name, string description = null)
        {
            Name = name;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class InputAttribute : ProcessorPropertyAttributeBase
    {
        public bool Required { get; }
        public InputAttribute(string name = null, bool required = true, string description = null) : base(name, description)
        {
            Required = required;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OutputAttribute : ProcessorPropertyAttributeBase
    {
        public OutputAttribute(string name, string description = null) : base(name, description)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DataProcessorAttribute : Attribute
    {
        public string MenuItem { get; set; } = "Processors";
        public string Name { get; set; }
        public string Group { get; set; }
        public string Tooltip { get; set; }
    }
}