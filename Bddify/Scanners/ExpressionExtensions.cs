using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bddify.Scanners
{
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
                return ExtractConstants(constantExpression);

            var newArrayExpression = expression as NewArrayExpression;
            if (newArrayExpression != null)
                return ExtractConstants(newArrayExpression);

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
                return ExtractConstants(unaryExpression);

            throw new InvalidOperationException("Could not fetch the arguments from the provided exception. This may be a bug so please report it");
        }

        private static IEnumerable<object> ExtractConstants(UnaryExpression unaryExpression)
        {
            return ExtractConstants(unaryExpression.Operand);
        }

        private static IEnumerable<object> ExtractConstants(NewArrayExpression newArrayExpression)
        {
            //yield return newArrayExpression.Expressions.SelectMany(x => ExtractConstants((ConstantExpression)x)).ToArray();
            var arrayElements = new ArrayList();
            Type type = newArrayExpression.Type.GetElementType();
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