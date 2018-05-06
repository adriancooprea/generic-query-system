using System;
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
    }
}
