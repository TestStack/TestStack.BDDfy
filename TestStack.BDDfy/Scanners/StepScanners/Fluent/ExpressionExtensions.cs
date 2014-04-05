using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestStack.BDDfy
{
    public static class ExpressionExtensions
    {
        public static Func<object, IEnumerable<object>> ExtractArguments<T>(this Expression<Action<T>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression == null)
                throw new InvalidOperationException("Please provide a lambda expression.");

            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
                throw new InvalidOperationException("Please provide a *method call* lambda expression.");

            return ExtractArguments(methodCallExpression);
        }

        public static Func<object, IEnumerable<object>> ExtractArguments<T>(this Expression<Func<T, System.Threading.Tasks.Task>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression == null)
                throw new InvalidOperationException("Please provide a lambda expression.");

            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
                throw new InvalidOperationException("Please provide a *method call* lambda expression.");

            return ExtractArguments(methodCallExpression);
        }

        private static Func<object, IEnumerable<object>> ExtractArguments(Expression expression)
        {
            if (expression == null || expression is ParameterExpression)
                return o => new object[0];

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
                return ExtractArguments(memberExpression);

            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
                return ExtractArguments(constantExpression);

            var newArrayExpression = expression as NewArrayExpression;
            if (newArrayExpression != null)
                return ExtractArguments(newArrayExpression);

            var newExpression = expression as NewExpression;
            if (newExpression != null)
                return ExtractArguments(newExpression);

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
                return ExtractArguments(unaryExpression);

            return o => new object[0];
        }

        private static Func<object, IEnumerable<object>> ExtractArguments(MethodCallExpression methodCallExpression)
        {
            return o =>
            {
                var constants = new List<object>();
                foreach (var arg in methodCallExpression.Arguments)
                {
                    constants.AddRange(ExtractArguments(arg)(o));
                }

                constants.AddRange(ExtractArguments(methodCallExpression.Object)(o));

                return constants;
            };
        }

        private static Func<object, IEnumerable<object>> ExtractArguments(UnaryExpression unaryExpression)
        {
            return ExtractArguments(unaryExpression.Operand);
        }

        private static Func<object, IEnumerable<object>> ExtractArguments(NewExpression newExpression)
        {
            return o =>
            {
                var arguments = new List<object>();
                foreach (var argumentExpression in newExpression.Arguments)
                {
                    arguments.AddRange(ExtractArguments(argumentExpression)(o));
                }

                return new[] {newExpression.Constructor.Invoke(arguments.ToArray())};
            };
        }

        private static Func<object, IEnumerable<object>> ExtractArguments(NewArrayExpression newArrayExpression)
        {
            Type type = newArrayExpression.Type.GetElementType();
            if (type is IConvertible)
                return ExtractConvertibleTypeArrayConstants(newArrayExpression, type);

            return ExtractNonConvertibleArrayConstants(newArrayExpression, type);
        }

        private static Func<object, IEnumerable<object>> ExtractNonConvertibleArrayConstants(NewArrayExpression newArrayExpression, Type type)
        {
            return o =>
            {
                var arrayElements = CreateList(type);
                foreach (var arrayElementExpression in newArrayExpression.Expressions)
                {
                    object arrayElement;

                    var constantExpression = arrayElementExpression as ConstantExpression;
                    if (constantExpression != null)
                        arrayElement = constantExpression.Value;
                    else
                        arrayElement = ExtractArguments(arrayElementExpression)(o).ToArray();

                    if (arrayElement is object[])
                    {
                        foreach (var item in (object[]) arrayElement)
                            arrayElements.Add(item);
                    }
                    else
                        arrayElements.Add(arrayElement);
                }

                return ToArray(arrayElements);
            };
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

        private static Func<object, IEnumerable<object>> ExtractConvertibleTypeArrayConstants(NewArrayExpression newArrayExpression, Type type)
        {
            var arrayElements = CreateList(type);
            foreach (var arrayElementExpression in newArrayExpression.Expressions)
            {
                var arrayElement = ((ConstantExpression)arrayElementExpression).Value;
                arrayElements.Add(Convert.ChangeType(arrayElement, arrayElementExpression.Type, null));
            }

            return o => new[] { ToArray(arrayElements) };
        }

        private static Func<object, IEnumerable<object>> ExtractArguments(ConstantExpression constantExpression)
        {

            var expression = constantExpression.Value as Expression;
            if (expression != null)
            {
                return ExtractArguments(expression);
            }

            var constants = new List<object>();
            if (constantExpression.Type == typeof(string) ||
                constantExpression.Type == typeof(decimal) ||
                constantExpression.Type.IsPrimitive ||
                constantExpression.Type.IsEnum ||
                constantExpression.Value == null)
                constants.Add(constantExpression.Value);

            return o => constants;
        }

        private static Func<object, IEnumerable<object>> ExtractArguments(MemberExpression memberExpression)
        {
            var constExpression = memberExpression.Expression as ConstantExpression;
            if (constExpression != null)
                return ExtractConstant(memberExpression, constExpression);
            
            var fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo != null)
                return ExtractFieldValue(memberExpression, fieldInfo);
            
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
                return ExtractPropertyValue(memberExpression, propertyInfo);

            throw new InvalidOperationException("Unknown expression type: " + memberExpression.GetType().Name);
        }

        private static Func<object, IEnumerable<object>> ExtractFieldValue(MemberExpression memberExpression, FieldInfo fieldInfo)
        {
            if (fieldInfo.IsStatic)
                return o => new[] {fieldInfo.GetValue(null)};

            throw new Exception("Currently only static fields supported");
        }

        private static Func<object, IEnumerable<object>> ExtractConstant(MemberExpression memberExpression, ConstantExpression constExpression)
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

            return o => constants;
        }

        private static Func<object, IEnumerable<object>> ExtractPropertyValue(MemberExpression expression, PropertyInfo member)
        {
            return o =>
            {
                var obj = o;
                //if (expression.Expression != null)
                //{
                //    obj = ExtractArguments(expression);
                //}

                return new[] { member.GetValue(o, null) };
            };
        }
    }
}
