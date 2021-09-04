using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Processing.DataProcessors
{
    public static class DataProcessorsProvider
    {
        public static IEnumerable<DataProcessorInfo> GetDataProcessorInfos()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDataProcessor).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .Select(t => new DataProcessorInfo(t));
        }
    }

    public class DataProcessorInfo
    {
        public string Name { get; }
        public Type Type { get; }

        public string DisplayMenuItem { get; set; }
        public string DisplayName { get; }
        public string DisplayGroup { get; }
        public string DisplayTooltip { get; set; }

        public IReadOnlyCollection<InputInfo> Inputs { get; }
        public IReadOnlyCollection<OutputInfo> Outputs { get; }
        public IDataProcessor Instance { get; private set; }

        public DataProcessorInfo(Type type)
        {
            Name = type.Name;
            Type = type;

            var dataProcessorAttribute = type.GetCustomAttribute<DataProcessorAttribute>();
            if (dataProcessorAttribute != null)
            {
                DisplayMenuItem = dataProcessorAttribute.MenuItem;
                DisplayGroup = dataProcessorAttribute.Group;
                DisplayName = dataProcessorAttribute.Name;
                DisplayTooltip = dataProcessorAttribute.Tooltip;
            }
            else
                DisplayName = Name;

            var propertyInfos = type.GetProperties();
            var inputInfos = new List<InputInfo>();
            var outputInfos = new List<OutputInfo>();

            foreach (var propertyInfo in propertyInfos)
            {
                var inputAttribute = propertyInfo.GetCustomAttribute<InputAttribute>();

                if (inputAttribute != null)
                    inputInfos.Add(new InputInfo(inputAttribute, propertyInfo, this));

                var outputAttribute = propertyInfo.GetCustomAttribute<OutputAttribute>();

                if (outputAttribute != null)
                    outputInfos.Add(new OutputInfo(outputAttribute, propertyInfo, this));
            }

            Inputs = inputInfos.ToArray();
            Outputs = outputInfos.ToArray();
        }

        public DataProcessorInfo(IDataProcessor dataProcessor) : this(dataProcessor.GetType())
        {
            Instance = dataProcessor;
        }
        public IDataProcessor CreateNewInstance()
        {
            Instance = (IDataProcessor) Type.GetConstructor(new Type[] { })?.Invoke(null);
            return Instance;
        }

        public InputInfo GetFirstUnsetInputFor(Type type)
        {
            return Inputs.FirstOrDefault(i => i.Type.IsAssignableFrom(type) && i.Get() == null);
        }

        public class InputInfo: DataProcessorPropertyInfoBase
        {
            public bool Required { get; }
            
            internal InputInfo(InputAttribute inputAttribute, PropertyInfo property, DataProcessorInfo dataProcessor): base(inputAttribute, property, dataProcessor)
            {
                Required = inputAttribute.Required;
            }

            public void Set(object value)
            {
                PropertyInfo.SetValue(DataProcessorInfo.Instance, value);
            }
        }

        public class OutputInfo: DataProcessorPropertyInfoBase
        {
            public OutputInfo(OutputAttribute outputAttribute, PropertyInfo property, DataProcessorInfo dataProcessor) : base(outputAttribute, property, dataProcessor)
            {
            }
        }

        public abstract class DataProcessorPropertyInfoBase
        {
            public string Name { get; }
            public string Description { get; }
            public Type Type => PropertyInfo.PropertyType;

            protected readonly DataProcessorInfo DataProcessorInfo;
            protected readonly PropertyInfo PropertyInfo;

            internal DataProcessorPropertyInfoBase(ProcessorPropertyAttributeBase processorPropertyAttribute, PropertyInfo property, DataProcessorInfo dataProcessor)
            {
                Name = processorPropertyAttribute.Name;
                Description = processorPropertyAttribute.Description;
                PropertyInfo = property;
                DataProcessorInfo = dataProcessor;
            }

            public object Get()
            {
                return PropertyInfo.GetValue(DataProcessorInfo.Instance);
            }

            public T Get<T>()
            {
                return (T) Get();
            }
        }
    }
}