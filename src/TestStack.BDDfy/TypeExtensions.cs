using System;
using System.Reflection;
using System.Linq;

namespace TestStack.BDDfy
{
    public static class TypeExtensions
    {
        public static Assembly Assembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
  
        }

        public static object[] GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo()
                .GetCustomAttributes(attributeType, inherit)
                .Cast<object>()
                .ToArray();
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsInstanceOfType(this Type type, object obj)
        {
            return type.GetTypeInfo().IsInstanceOfType(obj);
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
    }

}
