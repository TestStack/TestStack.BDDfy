using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace TestStack.BDDfy
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<StepArgument> ExtractArguments<T>(this LambdaExpression expression, T value)
        {
            return new ArgumentExtractorVisitor().ExtractArguments(expression, value);
        }

        public static IEnumerable<StepArgument> ExtractArguments<T>(this Expression<Func<T, Task>> expression, T value)
        {
            return new ArgumentExtractorVisitor().ExtractArguments(expression, value);
        }

        private class ArgumentExtractorVisitor : ExpressionVisitor
        {
            private List<StepArgument> _arguments;

            public IEnumerable<StepArgument> ExtractArguments(LambdaExpression methodCallExpression, object value)
            {
                _arguments = new List<StepArgument>();
                Visit(methodCallExpression);
                return _arguments;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var arguments = node.Arguments.Select(a =>
                {
                    switch (a.NodeType)
                    {
                        case ExpressionType.MemberAccess:
                            var memberExpression = (MemberExpression)a;
                            var field = memberExpression.Member as FieldInfo;
                            string name;
                            Type parameterType;
                            bool isReadOnly;
                            if (field != null)
                            {
                                name = field.Name;
                                parameterType = field.FieldType;
                                isReadOnly = field.IsInitOnly;
                            }
                            else
                            {
                                var propertyInfo = (PropertyInfo)memberExpression.Member;
                                name = propertyInfo.Name;
                                parameterType = propertyInfo.PropertyType;
                                isReadOnly = !propertyInfo.CanWrite;
                            }

                            var getValue = GetValue(memberExpression);
                            var setValue = isReadOnly ? null : SetValue(memberExpression, parameterType);

                            return new StepArgument(name, parameterType, getValue, setValue);
                        default:
                            return new StepArgument(GetValue(a));
                    }
                });
                _arguments.AddRange(arguments);
                return node;
            }

            private static Func<object> GetValue(Expression a)
            {
                return Expression.Lambda<Func<object>>(Expression.Convert(a, typeof(object))).Compile();
            }

            private static Action<object> SetValue(Expression a, Type parameterType)
            {
                var parameter = Expression.Parameter(typeof(object));
                var unaryExpression = Expression.Convert(parameter, parameterType);
                var assign = Expression.Assign(a, unaryExpression);
                return Expression.Lambda<Action<object>>(assign, parameter).Compile();
            }
        }
    }
}