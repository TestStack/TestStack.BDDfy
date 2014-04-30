using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<object> ExtractArguments<T>(this Expression<Action<T>> expression, T value)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression == null)
                throw new InvalidOperationException("Please provide a lambda expression.");

            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
                throw new InvalidOperationException("Please provide a *method call* lambda expression.");

            return ExtractArguments(methodCallExpression, value);
        }

        public static IEnumerable<object> ExtractArguments<T>(this Expression<Func<T, Task>> expression, T value)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression == null)
                throw new InvalidOperationException("Please provide a lambda expression.");

            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
                throw new InvalidOperationException("Please provide a *method call* lambda expression.");

            return ExtractArguments(methodCallExpression, value);
        }

        private static IEnumerable<object> ExtractArguments<T>(Expression expression, T value)
        {
            if (expression == null || expression is ParameterExpression)
                return new object[0];

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
                return ExtractArguments(memberExpression, value);

            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
                return ExtractArguments(constantExpression, value);

            var newArrayExpression = expression as NewArrayExpression;
            if (newArrayExpression != null)
                return ExtractArguments(newArrayExpression, value);

            var newExpression = expression as NewExpression;
            if (newExpression != null)
                return ExtractArguments(newExpression, value);

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
                return ExtractArguments(unaryExpression, value);

            return new object[0];
        }

        private static IEnumerable<object> ExtractArguments<T>(MethodCallExpression methodCallExpression, T value)
        {
            var constants = new List<object>();
            foreach (var arg in methodCallExpression.Arguments)
            {
                constants.AddRange(ExtractArguments(arg, value));
            }

            constants.AddRange(ExtractArguments(methodCallExpression.Object, value));

            return constants;
        }

        private static IEnumerable<object> ExtractArguments<T>(UnaryExpression unaryExpression, T value)
        {
            return ExtractArguments(unaryExpression.Operand, value);
        }

        private static IEnumerable<object> ExtractArguments<T>(NewExpression newExpression, T value)
        {
            var arguments = new List<object>();
            foreach (var argumentExpression in newExpression.Arguments)
            {
                arguments.AddRange(ExtractArguments(argumentExpression, value));
            }

            return new[] { newExpression.Constructor.Invoke(arguments.ToArray()) };
        }

        private static IEnumerable<object> ExtractArguments<T>(NewArrayExpression newArrayExpression, T value)
        {
            Type type = newArrayExpression.Type.GetElementType();
            if (type is IConvertible)
                return ExtractConvertibleTypeArrayConstants(newArrayExpression, type);

            return ExtractNonConvertibleArrayConstants(newArrayExpression, type, value);
        }

        private static IEnumerable<object> ExtractNonConvertibleArrayConstants<T>(NewArrayExpression newArrayExpression, Type type, T value)
        {
            var arrayElements = CreateList(type);
            foreach (var arrayElementExpression in newArrayExpression.Expressions)
            {
                object arrayElement;

                var constantExpression = arrayElementExpression as ConstantExpression;
                if (constantExpression != null)
                    arrayElement = constantExpression.Value;
                else
                    arrayElement = ExtractArguments(arrayElementExpression, value).ToArray();

                if (arrayElement is object[])
                {
                    foreach (var item in (object[])arrayElement)
                        arrayElements.Add(item);
                }
                else
                    arrayElements.Add(arrayElement);
            }

            return ToArray(arrayElements);
        }

        private static IEnumerable<object> ToArray(IList list)
        {
            var toArrayMethod = list.GetType().GetMethod("ToArray");
            yield return toArrayMethod.Invoke(list, new Type[] { });
        }

        private static IList CreateList(Type type)
        {
            return (IList)typeof(List<>).MakeGenericType(type).GetConstructor(new Type[0]).Invoke(BindingFlags.CreateInstance, null, null, null);
        }

        private static IEnumerable<object> ExtractConvertibleTypeArrayConstants(NewArrayExpression newArrayExpression, Type type)
        {
            var arrayElements = CreateList(type);
            foreach (var arrayElementExpression in newArrayExpression.Expressions)
            {
                var arrayElement = ((ConstantExpression)arrayElementExpression).Value;
                arrayElements.Add(Convert.ChangeType(arrayElement, arrayElementExpression.Type, null));
            }

            return new[] { ToArray(arrayElements) };
        }

        private static IEnumerable<object> ExtractArguments<T>(ConstantExpression constantExpression, T value)
        {
            var expression = constantExpression.Value as Expression;
            if (expression != null)
            {
                return ExtractArguments(expression, value);
            }

            var constants = new List<object>();
            if (constantExpression.Type == typeof(string) ||
                constantExpression.Type == typeof(decimal) ||
                constantExpression.Type.IsPrimitive ||
                constantExpression.Type.IsEnum ||
                constantExpression.Value == null)
                constants.Add(constantExpression.Value);

            return constants;
        }

        private static IEnumerable<object> ExtractArguments<T>(MemberExpression memberExpression, T value)
        {
            var constExpression = memberExpression.Expression as ConstantExpression;
            if (constExpression != null)
                return ExtractConstant(memberExpression, constExpression);

            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo != null)
                return ExtractFieldValue(memberExpression, fieldInfo, value);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
                return ExtractPropertyValue(memberExpression, propertyInfo, value);

            throw new InvalidOperationException("Unknown expression type: " + memberExpression.GetType().Name);
        }

        private static IEnumerable<object> ExtractFieldValue<T>(MemberExpression memberExpression, FieldInfo fieldInfo, T value)
        {
            if (fieldInfo.IsStatic)
                return new[] { fieldInfo.GetValue(null) };

            return new[] { fieldInfo.GetValue(value) };
        }

        private static IEnumerable<object> ExtractConstant(MemberExpression memberExpression, ConstantExpression constExpression)
        {
            var constants = new List<object>();
            var valIsConstant = constExpression != null;
            Type declaringType = memberExpression.Member.DeclaringType;
            object declaringObject = memberExpression.Member.DeclaringType;

            if (valIsConstant)
            {
                declaringType = constExpression.Type;
                declaringObject = constExpression.Value;
            }

            var member = declaringType.GetMember(memberExpression.Member.Name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Single();

            if (member.MemberType == MemberTypes.Field)
                constants.Add(((FieldInfo)member).GetValue(declaringObject));
            else
                constants.Add(((PropertyInfo)member).GetGetMethod(true).Invoke(declaringObject, null));

            return constants;
        }

        private static IEnumerable<object> ExtractPropertyValue<T>(MemberExpression expression, PropertyInfo member, T value)
        {
            var memberExpression = expression.Expression as MemberExpression;
            if (memberExpression != null)
            {
                var extractArguments = ExtractArguments(memberExpression, value);
                return new[]
                       {
                           member.GetValue(extractArguments.Single(), null)
                       };
            }
            return new[]
                       {
                           member.GetValue(value, null)
                       };
        }
    }
}