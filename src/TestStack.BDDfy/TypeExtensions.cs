using System;
using System.Reflection;

namespace TestStack.BDDfy
{
#if NET40
    public static class TypeExtensions
    {
        public static Assembly Assembly(this Type type)
        {
            return type.Assembly;
        }

        public static object[] GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetCustomAttributes(attributeType, inherit);
        }

        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        public static bool IsInstanceOfType(this Type type, object obj)
        {
            return type.IsInstanceOfType(obj);
        }

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }
    }

#else
using System.Linq;
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
#if NETSTANDARD1_3
            return type.GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo());
#else
            return type.GetTypeInfo().IsInstanceOfType(obj);
#endif
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
    }

#endif

}
