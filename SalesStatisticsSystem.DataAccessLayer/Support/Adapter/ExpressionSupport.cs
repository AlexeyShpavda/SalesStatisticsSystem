using System;
using System.Linq;
using System.Linq.Expressions;

namespace SalesStatisticsSystem.DataAccessLayer.Support.Adapter
{
    public static class ExpressionSupport
    {
        public static Expression<Func<TTargetType, bool>> Project<TSourceType, TTargetType>(
            this Expression<Func<TSourceType, bool>> sourceExpression)
        {
            var sourceParameter = sourceExpression.Parameters.FirstOrDefault();

            var targetParameter = Expression.Parameter(typeof(TTargetType), sourceParameter?.Name);

            var newBody = new TransformVisitor(sourceParameter, targetParameter).Visit(sourceExpression.Body);

            var paramsList = sourceExpression.Parameters.ToList();

            var position = paramsList.IndexOf(sourceParameter);

            paramsList[position] = targetParameter;

            return Expression.Lambda<Func<TTargetType, bool>>(newBody, paramsList);
        }

        private class TransformVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _targetParameter;
            private readonly ParameterExpression _sourceParameter;

            public TransformVisitor(ParameterExpression sourceParameter, ParameterExpression targetParameter)
            {
                _sourceParameter = sourceParameter;
                _targetParameter = targetParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _sourceParameter ? _targetParameter : base.VisitParameter(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if ((node.Member.MemberType & System.Reflection.MemberTypes.Property) != 0)
                {
                    var newExpression = Expression.Property(Visit(node.Expression), node.Member.Name);
                    return newExpression;
                }

                if ((node.Member.MemberType & System.Reflection.MemberTypes.Field) == 0) return base.VisitMember(node);
                {
                    var newExpression = Expression.Field(Visit(node.Expression), node.Member.Name);
                    return newExpression;
                }
            }
        }
    }
}