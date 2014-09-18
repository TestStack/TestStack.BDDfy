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
        public static IEnumerable<StepArgument> ExtractArguments<T>(this Expression<Action<T>> expression, T value)
        {
            var lambdaExpression = expression as LambdaExpression;
            if (lambdaExpression == null)
                throw new InvalidOperationException("Please provide a lambda expression.");

            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if (methodCallExpression == null)
                throw new InvalidOperationException("Please provide a *method call* lambda expression.");

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
                            if (field != null)
                            {
                                var o = field.IsStatic ? null : GetValue(memberExpression.Expression);
                                return new StepArgument(field, o);
                            }
                            var propertyInfo = (PropertyInfo)memberExpression.Member;
                            var methodInfo = propertyInfo.GetGetMethod(true);
                            var declaringObject = methodInfo == null || methodInfo.IsStatic ? null : GetValue(memberExpression.Expression);
                            return new StepArgument(propertyInfo, declaringObject);
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
        }
    }
}