using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services.Extensions
{
    public static class ExpressionExtensions
    {
        internal class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression parameter;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return base.VisitParameter(parameter);
            }

            internal ParameterReplacer(ParameterExpression parameter)
            {
                this.parameter = parameter;
            }
        }


        public static Func<TParam, TResult> Combine<TParam, TResult>(this Expression<Func<TParam, TResult>> expression,
            Expression<Func<TParam, TResult>> secondExpression)
        {
            var parameterExpression = Expression.Parameter(typeof(TParam));
            var binaryExpression = Expression.And(expression.Body, secondExpression.Body);
            var visitedBinaryExpression =  (BinaryExpression) new ParameterReplacer(parameterExpression).Visit(binaryExpression);
            var newDelegate = Expression.Lambda<Func<TParam, TResult>>(visitedBinaryExpression, parameterExpression);

            return newDelegate.Compile();
        }
    }
}
