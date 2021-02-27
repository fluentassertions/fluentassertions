using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace FluentAssertions.Formatting
{
    public class PredicateLambdaExpressionValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => value is LambdaExpression lambdaExpression && lambdaExpression.ReturnType == typeof(bool);

        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var lambdaExpression = value as LambdaExpression;

            var reducedExpression = ReduceConstantSubExpressions(lambdaExpression.Body);

            if (reducedExpression is BinaryExpression binaryExpression && binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                var subExpressions = ExtractChainOfExpressionsJoinedWithAndOperator(binaryExpression);
                return string.Join(" AndAlso ", subExpressions.Select(_ => _.ToString()));
            }

            return reducedExpression.ToString();
        }

        private static Expression ReduceConstantSubExpressions(Expression expression) => new ConstantSubExpressionReductionVisitor().Visit(expression);

        private static bool ExpressionIsConstant(Expression expression)
        {
            var visitor = new ParameterDetector();
            visitor.Visit(expression);
            return !visitor.HasParameters;
        }

        private static IEnumerable<Expression> ExtractChainOfExpressionsJoinedWithAndOperator(BinaryExpression binaryExpression)
        {
            var visitor = new AndOperatorChainExtractor();
            visitor.Visit(binaryExpression);
            return visitor.AndChain;
        }

        internal class ParameterDetector : ExpressionVisitor
        {
            public bool HasParameters { get; private set; } = false;

            public override Expression Visit(Expression node)
            {
                // As soon as at least one parameter was found in the expression tree we should stop iterating (this is achieved by not callling base.Visit).
                return HasParameters ? node : base.Visit(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                HasParameters = true;
                return node;
            }
        }

        internal class ConstantSubExpressionReductionVisitor : ExpressionVisitor
        {
            public override Expression Visit(Expression node)
            {
                if (ExpressionIsConstant(node))
                {
                    return Expression.Constant(Expression.Lambda(node).Compile().DynamicInvoke());                    
                }

                return base.Visit(node);
            }
        }

        internal class AndOperatorChainExtractor : ExpressionVisitor
        {
            public List<Expression> AndChain { get; }  = new List<Expression>();

            public override Expression Visit(Expression node)
            {
                if (node.NodeType == ExpressionType.AndAlso)
                {
                    var binaryExpression = node as BinaryExpression;
                    Visit(binaryExpression.Left);
                    Visit(binaryExpression.Right);
                }
                else
                {
                    AndChain.Add(node);
                }

                return null;
            }
        }
    }
}
