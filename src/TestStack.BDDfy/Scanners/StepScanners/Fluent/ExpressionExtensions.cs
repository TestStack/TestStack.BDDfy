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
            private List<StepArgument> _arguments = [];
            private object? _value;

            public IEnumerable<StepArgument> ExtractArguments(LambdaExpression methodCallExpression, object? value)
            {
                _arguments = [];
                _value = value;
                Visit(methodCallExpression);
                return _arguments;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Object is MethodCallExpression methodCallExpression) Visit(methodCallExpression);

                var arguments = node.Arguments.Select(ExtractStepArgument);
                _arguments.AddRange(arguments);
                return node;
            }

            private StepArgument ExtractStepArgument(Expression a)
            {
                switch (a.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression) a;
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
                            var propertyInfo = (PropertyInfo) memberExpression.Member;
                            name = propertyInfo.Name;
                            parameterType = propertyInfo.PropertyType;
                            isReadOnly = !propertyInfo.CanWrite;
                        }

                        var getValue = GetValue(memberExpression);
                        var setValue = isReadOnly ? null : SetValue(memberExpression, parameterType);

                        return new StepArgument(name, parameterType, getValue, setValue);

                    case ExpressionType.Convert:
                        return ExtractStepArgument(((UnaryExpression)a).Operand);
                    default:
                        return new StepArgument(GetValue(a));
                }
            }

            private Func<object> GetValue(Expression expression)
            {
                // If the expression is a member access on the lambda parameter (e.g. _ => _.Prop)
                // replace the parameter with the supplied _value so the compiled delegate can be invoked
                if (expression is MemberExpression memberExpression && memberExpression.Expression is ParameterExpression)
                {
                    var replaced = Expression.Convert(Expression.MakeMemberAccess(Expression.Constant(_value), memberExpression.Member), typeof(object));
                    return Expression.Lambda<Func<object>>(replaced).Compile();
                }

                return Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object))).Compile();
            }

            private Action<object?> SetValue(Expression expression, Type parameterType)
            {
                var parameter = Expression.Parameter(typeof(object));
                var unaryExpression = Expression.Convert(parameter, parameterType);

                if (expression is MemberExpression memberExpression && memberExpression.Expression is ParameterExpression)
                {
                    var memberAccess = Expression.MakeMemberAccess(Expression.Constant(_value), memberExpression.Member);
                    var assign = Expression.Assign(memberAccess, unaryExpression);
                    return Expression.Lambda<Action<object?>>(assign, parameter).Compile();
                }

                var assignDefault = Expression.Assign(expression, unaryExpression);
                return Expression.Lambda<Action<object?>>(assignDefault, parameter).Compile();
            }
        }
    }
}