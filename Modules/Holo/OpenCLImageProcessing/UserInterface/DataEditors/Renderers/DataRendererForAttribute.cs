using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UserInterface.DataEditors.Renderers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataRendererForAttribute : Attribute
    {
        public Type Type { get; set; }
        public DataRendererForAttribute(Type type)
        {
            Type = type;
        }
    }

    public static class DataRendererUtil
    {
        public static IDataRenderer GetRendererFor(Type type)
        {
            var dataRendererTypes = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => typeof(IDataRenderer).IsAssignableFrom(t)).ToArray();

            var rendererTypesForType = dataRendererTypes.Where(t => t.GetCustomAttribute<DataRendererForAttribute>()?.Type == type);

            if (!rendererTypesForType.Any())
                rendererTypesForType = dataRendererTypes.Where(t => t.GetCustomAttribute<DataRendererForAttribute>()?.Type.IsAssignableFrom(type) ?? false);

            var rendererType = rendererTypesForType.FirstOrDefault();

            if (rendererType == null)
                return null;

            var renderer = (IDataRenderer)rendererType.GetConstructor(new Type[] { })?.Invoke(new object[] { });

            return renderer;
        }
    }
}
