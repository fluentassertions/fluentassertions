using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace FluentAssertions.Formatting
{
    public static class AdvancedExpressionFormatter 
    {
        public static string FormatExpression(LambdaExpression expression) => FormatExpression(expression.Body);

        public static string FormatExpression(Expression expression)
        {
            var reducedExpression = new ConstantExpressionReductionVisitor().Visit(expression);
            
            if (reducedExpression is BinaryExpression binaryExpression && binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                var subExpressions = AndOperatorChainExtractor.ExtractChainOfExpressionsJoinedWithAndOperator(binaryExpression);
                return string.Join(" AndAlso ", subExpressions.Select(_ => _.ToString()));
            }

            return expression.ToString();
        }

        private static bool IsConstantExpression(Expression expression)
        {
            var visitor = new ConstantVisitor();
            visitor.Visit(expression);
            return !visitor.HasParameters;
        }

        internal class ConstantVisitor : ExpressionVisitor
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

        internal class ConstantExpressionReductionVisitor : ExpressionVisitor
        {
            public override Expression Visit(Expression node)
            {
                if (IsConstantExpression(node))
                {
                    return Expression.Constant(Expression.Lambda(node).Compile().DynamicInvoke());                    
                }

                return base.Visit(node);
            }
        }

        internal class AndOperatorChainExtractor : ExpressionVisitor
        {
            private List<Expression> andChain = new List<Expression>();

            private AndOperatorChainExtractor() { }

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
                    andChain.Add(node);
                }

                return null;
            }

            public static IEnumerable<Expression> ExtractChainOfExpressionsJoinedWithAndOperator(BinaryExpression binaryExpression)
            {
                var visitor = new AndOperatorChainExtractor();
                visitor.Visit(binaryExpression);
                return visitor.andChain;
            }
        }
    }
}
