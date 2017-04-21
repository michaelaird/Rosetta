/// <summary>
/// ElementAccessExpression.cs
/// Scott Bannister - 2017
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper for accessing member access expressions in AST.
    /// </summary>
    public class ElementAccessExpression : Helper
    {
        // Cached values
        private IEnumerable<Argument> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAccessExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public ElementAccessExpression(ElementAccessExpressionSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAccessExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public ElementAccessExpression(ElementAccessExpressionSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        public IEnumerable<Argument> Arguments
        {
            get
            {
                if (this.arguments == null)
                {
                    this.arguments = this.ElementAccessExpressionSyntaxNode.ArgumentList.Arguments.Select(
                        (ArgumentSyntax p) => new Argument(p)).ToList();
                }

                return this.arguments;
            }
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public ExpressionSyntax Expression
        {
            get { return this.ElementAccessExpressionSyntaxNode.Expression; }
        }

        private ElementAccessExpressionSyntax ElementAccessExpressionSyntaxNode
        {
            get { return this.SyntaxNode as ElementAccessExpressionSyntax; }
        }
    }
}