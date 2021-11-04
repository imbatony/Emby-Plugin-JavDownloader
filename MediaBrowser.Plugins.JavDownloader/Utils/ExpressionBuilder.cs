namespace MediaBrowser.Plugins.JavDownloader.Utils
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="ExpressionBuilder" />.
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// The And.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="first">The first<see cref="Expression"/>.</param>
        /// <param name="second">The second<see cref="Expression"/>.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.AndAlso<T>(second, Expression.AndAlso);
        }

        /// <summary>
        /// The Or.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="first">The first<see cref="Expression"/>.</param>
        /// <param name="second">The second<see cref="Expression"/>.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.AndAlso<T>(second, Expression.OrElse);
        }

        /// <summary>
        /// The AndAlso.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="expr1">The expr1<see cref="Expression"/>.</param>
        /// <param name="expr2">The expr2<see cref="Expression"/>.</param>
        /// <param name="func">The func<see cref="Func{Expression, Expression, BinaryExpression}"/>.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        private static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2,
        Func<Expression, Expression, BinaryExpression> func)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                func(left, right), parameter);
        }

        /// <summary>
        /// Defines the <see cref="ReplaceExpressionVisitor" />.
        /// </summary>
        private class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            /// <summary>
            /// Defines the oldValue.
            /// </summary>
            private readonly Expression oldValue;

            /// <summary>
            /// Defines the newValue.
            /// </summary>
            private readonly Expression newValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReplaceExpressionVisitor"/> class.
            /// </summary>
            /// <param name="oldValue">The oldValue<see cref="Expression"/>.</param>
            /// <param name="newValue">The newValue<see cref="Expression"/>.</param>
            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                this.oldValue = oldValue;
                this.newValue = newValue;
            }

            /// <summary>
            /// The Visit.
            /// </summary>
            /// <param name="node">The node<see cref="Expression"/>.</param>
            /// <returns>The <see cref="Expression"/>.</returns>
            public override Expression Visit(Expression node)
            {
                if (node == this.oldValue)
                {
                    return this.newValue;
                }

                return base.Visit(node);
            }
        }
    }
}
