using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace GenericQuerySystem.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasProperty(this Type type, string propertyName)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Any(x => x.Name == propertyName.Replace(" ", string.Empty));
        }

        private static bool HasPublicProperty(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName) != null;
        }

        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            if (!type.HasPublicProperty(propertyName))
            {
                return null;
            }

            return type.GetProperty(propertyName);
        }

        public static object GetPropertiesAsObject<T>(this Type type, IEnumerable<string> properties, T sourceObj)
        {
            var expandoObj = new ExpandoObject();
            foreach (var field in properties)
            {
                if (!type.HasProperty(field))
                {
                    continue;
                }

                expandoObj.TryAdd(field, type.GetPropertyValue(field, sourceObj));
            }

            return expandoObj;
        }

        private static object GetFormatedValue(Type propertyType, object value)
        {
            if (propertyType == typeof(DateTime))
            {
                return DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd");
            }

            if (propertyType == typeof(TimeSpan))
            {
                var indexOfDot = value.ToString().LastIndexOf('.');
                if (indexOfDot < 0)
                {
                    return value;
                }
                var result = value.ToString().Substring(0, value.ToString().Length - indexOfDot);

                return result;
            }

            if (propertyType.IsEnum)
            {
                return Enum.GetName(propertyType, value);
            }

            return value;
        }

        public static object GetPropertyValue(this Type type, string propertyName, object sourceObj)
        {
            var propertyNameString = propertyName.Trim().Replace(" ", "");

            if (!type.HasPublicProperty(propertyNameString))
            {
                return null;
            }

            return GetFormatedValue(type.GetProperty(propertyNameString).PropertyType, type.GetPropertyInfo(propertyNameString).GetValue(sourceObj));
        }


    }
}
