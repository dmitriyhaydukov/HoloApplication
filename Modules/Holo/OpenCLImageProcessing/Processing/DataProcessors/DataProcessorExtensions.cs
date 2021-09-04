using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Processing.DataProcessors
{
    public static class DataProcessorExtensions
    {
        public static IEnumerable<DataProcessorPropertyInfo> GetPropertyInfos(this IDataProcessor self)
        {
            var allPropertyInfos = self.GetType().GetProperties();
            var inputOutputPropertyInfos = new List<DataProcessorPropertyInfo>();

            foreach (var propertyInfo in allPropertyInfos)
            {
                var inputAttribute = propertyInfo.GetCustomAttribute<InputAttribute>();
                var outputAttribute = propertyInfo.GetCustomAttribute<OutputAttribute>();

                if (inputAttribute != null)
                    inputOutputPropertyInfos.Add(new DataProcessorPropertyInfo(propertyInfo, self, inputAttribute));

                if (outputAttribute != null)
                    inputOutputPropertyInfos.Add(new DataProcessorPropertyInfo(propertyInfo, self, outputAttribute));
            }

            return inputOutputPropertyInfos;
        }
    }
}
