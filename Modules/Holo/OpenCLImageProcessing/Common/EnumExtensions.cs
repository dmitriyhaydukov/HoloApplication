using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum self) where T : Attribute
        {
            var enumType = self.GetType();
            var memberInfo = enumType.GetMember(self.ToString()).Single();
            var customAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(T));
            return (T) customAttribute;
        }

        public static IEnumerable<T> GetValues<T>() where T: struct
        {
            return GetValuesAsObjects(typeof(T)).Select(v => (T) v);
        }

        public static IEnumerable<Enum> GetValues(Type type)
        {
            return GetValuesAsObjects(type).Select(v => (Enum)v);
        }

        private static IEnumerable<object> GetValuesAsObjects(Type type)
        {
            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var enumValue in type.GetEnumValues())
            {
                yield return enumValue;
            }
        }
    }
}