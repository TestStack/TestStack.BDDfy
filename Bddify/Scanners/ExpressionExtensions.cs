
namespace Bddify.Scanners
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ExpressionExtensions
    {
        public static IEnumerable<object> ExtractConstants<T>(this Expression<Action<T>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression == null)
                throw new InvalidOperationException("Please provide a lambda expression.");

            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if(methodCallExpression == null)
                throw new InvalidOperationException("Please provide a *method call* lambda expression.");

            return ExtractConstants(methodCallExpression);
        }

        private static IEnumerable<object> ExtractConstants(MethodCallExpression methodCallExpression)
        {
            foreach (var arg in methodCallExpression.Arguments)
            {
                foreach (var constant in ExtractConstants(arg))
                    yield return constant;
            }

            foreach (var constant in ExtractConstants(methodCallExpression.Object))
                yield return constant;
        }

        private static IEnumerable<object> ExtractConstants(Expression expression)
        {
            if (expression == null || expression is ParameterExpression)
                return new object[0];

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
                return ExtractConstants(memberExpression).ToArray();

            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
                return ExtractConstants(constantExpression).ToArray();

            var newArrayExpression = expression as NewArrayExpression;
            if (newArrayExpression != null)
                return ExtractConstants(newArrayExpression).ToArray();

            var newExpression = expression as NewExpression;
            if (newExpression != null)
                return ExtractConstants(newExpression).ToArray();

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
                return ExtractConstants(unaryExpression).ToArray();

            throw new InvalidOperationException("Could not fetch the arguments from the provided exception. This may be a bug so please report it");
        }

        private static IEnumerable<object> ExtractConstants(UnaryExpression unaryExpression)
        {
            return ExtractConstants(unaryExpression.Operand);
        }

        private static IEnumerable<object> ExtractConstants(NewExpression newExpression)
        {
            throw new NotImplementedException();
            //var arguments = new List<object>();
            //foreach (var argumentExpression in newExpression.Arguments)
            //{
            //    arguments.Add(ExtractConstants(argumentExpression));
            //}

            //yield return newExpression.Constructor.Invoke(arguments.ToArray());
        }

        private static IEnumerable<object> ExtractConstants(NewArrayExpression newArrayExpression)
        {
            Type type = newArrayExpression.Type.GetElementType();
            if (type is IConvertible)
                return ExtractConvertibleTypeArrayConstants(newArrayExpression, type);
            
            return ExtractNonConvertibleArrayConstants(newArrayExpression, type);
        }

        private static IEnumerable<object> ExtractNonConvertibleArrayConstants(NewArrayExpression newArrayExpression, Type type)
        {
            dynamic arrayElements = Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            foreach (var arrayElementExpression in newArrayExpression.Expressions)
            {
                dynamic arrayElement;

                if (arrayElementExpression is ConstantExpression)
                    arrayElement = ((ConstantExpression)arrayElementExpression).Value;
                else
                    arrayElement = ExtractConstants(arrayElementExpression);

                if (arrayElement is object[])
                {
                    foreach (dynamic item in (object[])arrayElement)
                        arrayElements.Add(item);
                }
                else
                    arrayElements.Add(arrayElement);
            }

            yield return arrayElements.ToArray();
        }

        private static IEnumerable<object> ExtractConvertibleTypeArrayConstants(NewArrayExpression newArrayExpression, Type type)
        {
            var arrayElements = new ArrayList();
            foreach (var arrayElementExpression in newArrayExpression.Expressions)
            {
                var arrayElement = ((ConstantExpression)arrayElementExpression).Value;
                arrayElements.Add(Convert.ChangeType(arrayElement, arrayElementExpression.Type));
            }

            yield return arrayElements.ToArray(type);
        }

        private static IEnumerable<object> ExtractConstants(ConstantExpression constantExpression)
        {
            if (constantExpression.Value is Expression)
            {
                foreach (var constant in ExtractConstants((Expression)constantExpression.Value))
                {
                    yield return constant;
                }
            }
            else
            {
                if(constantExpression.Type == typeof(string) || constantExpression.Type.IsPrimitive)
                    yield return constantExpression.Value;
            }
        }

        private static IEnumerable<object> ExtractConstants(MemberExpression memberExpression)
        {
            var constExpression = (ConstantExpression)memberExpression.Expression;
            var type = constExpression.Type;
            var member = type.GetMember(memberExpression.Member.Name, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Single();

            if (member.MemberType == MemberTypes.Field)
                yield return ((FieldInfo)member).GetValue(constExpression.Value);
            else
                yield return ((PropertyInfo)member).GetGetMethod(true).Invoke(constExpression.Value, null);
        }
    }
}