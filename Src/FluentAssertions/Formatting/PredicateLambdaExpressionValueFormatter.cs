using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Formatting
{
    /// <summary>
    /// The <see cref="PredicateLambdaExpressionValueFormatter" /> is responsible for formatting
    /// boolean lambda expressions.
    /// </summary>
    public class PredicateLambdaExpressionValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => value is LambdaExpression lambdaExpression;

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            var lambdaExpression = (LambdaExpression)value;

            var reducedExpression = ReduceConstantSubExpressions(lambdaExpression.Body);

            if (reducedExpression is BinaryExpression binaryExpression && binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                var subExpressions = ExtractChainOfExpressionsJoinedWithAndOperator(binaryExpression);
                formattedGraph.AddFragment(string.Join(" AndAlso ", subExpressions.Select(e => e.ToString())));
            }
            else
            {
                formattedGraph.AddFragment(reducedExpression.ToString());
            }
        }

        /// <summary>
        /// This step simplifies the lambda expression by replacing parts of it which do not depend on the lambda parameters
        /// with the actual values of these sub-expressions. The simplified expression is much easier to read.
        /// E.g. "(_.Text == "two") AndAlso (_.Number == 3)"
        /// Instead of "(_.Text == value(FluentAssertions.Specs.Collections.GenericCollectionAssertionsSpecs+c__DisplayClass122_0).twoText) AndAlso (_.Number == 3)".
        /// </summary>
        private static Expression ReduceConstantSubExpressions(Expression expression)
        {
            return new ConstantSubExpressionReductionVisitor().Visit(expression);
        }

        /// <summary>
        /// This step simplifies the lambda expression by removing unnecessary parentheses for root level chain of AND operators.
        /// E.g. (_.Text == "two") AndAlso (_.Number == 3) AndAlso (_.OtherText == "foo")
        /// Instead of ((_.Text == "two") AndAlso (_.Number == 3)) AndAlso (_.OtherText == "foo")
        /// This simplification is only implemented for the chain of AND operators because this is the most common predicate scenario.
        /// Similar logic can be implemented in the future for other operators.
        /// </summary>
        private static IEnumerable<Expression> ExtractChainOfExpressionsJoinedWithAndOperator(BinaryExpression binaryExpression)
        {
            var visitor = new AndOperatorChainExtractor();
            visitor.Visit(binaryExpression);
            return visitor.AndChain;
        }

        /// <summary>
        /// Expression visitor which can detect whether the expression depends on parameters.
        /// </summary>
        private class ParameterDetector : ExpressionVisitor
        {
            public bool HasParameters { get; private set; }

            public override Expression Visit(Expression node)
            {
                // As soon as at least one parameter was found in the expression tree we should stop iterating (this is achieved by not calling base.Visit).
                return HasParameters ? node : base.Visit(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                HasParameters = true;
                return node;
            }
        }

        /// <summary>
        /// Expression visitor which can replace constant sub-expressions with constant values.
        /// </summary>
        private class ConstantSubExpressionReductionVisitor : ExpressionVisitor
        {
            public override Expression Visit(Expression node)
            {
                if (node is null)
                {
                    return null;
                }

                if (node is ConstantExpression)
                {
                    return node;
                }

                if (!HasLiftedOperator(node) && ExpressionIsConstant(node))
                {
                    return Expression.Constant(Expression.Lambda(node).Compile().DynamicInvoke());
                }

                return base.Visit(node);
            }

            private static bool HasLiftedOperator(Expression expression) =>
                expression is BinaryExpression { IsLifted: true } or UnaryExpression { IsLifted: true };

            private static bool ExpressionIsConstant(Expression expression)
            {
                var visitor = new ParameterDetector();
                visitor.Visit(expression);
                return !visitor.HasParameters;
            }
        }

        /// <summary>
        /// Expression visitor which can extract sub-expressions from an expression which has the following form:
        /// (SubExpression1) AND (SubExpression2) ... AND (SubExpressionN)
        /// </summary>
        private class AndOperatorChainExtractor : ExpressionVisitor
        {
            public List<Expression> AndChain { get; } = new List<Expression>();

            public override Expression Visit(Expression node)
            {
                if (node.NodeType == ExpressionType.AndAlso)
                {
                    var binaryExpression = (BinaryExpression)node;
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
