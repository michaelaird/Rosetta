/// <summary>
/// MemberAccessExpression.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Helper for accessing member access expressions in AST.
    /// </summary>
    public class ElementAccessExpression : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public ElementAccessExpression(ElementAccessExpressionSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public ElementAccessExpression(ElementAccessExpressionSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        /// <summary>
        /// Gets the member name value.
        /// </summary>
        public string MemberName
        {
            get
            {
                switch (this.ElementAccessExpressionSyntaxNode.Expression.ToString())
                {
                    case "ObservableArray":
                        return "observableArray";

                    case "Observable":
                        return "observable";

                    default:
                        return this.ElementAccessExpressionSyntaxNode.Expression.ToString();
                }
            }
        }

        public string Argument
        {
            get
            {
                return this.ElementAccessExpressionSyntaxNode.ArgumentList.Arguments.ToString();
            }
        }

        private ElementAccessExpressionSyntax ElementAccessExpressionSyntaxNode
        {
            get { return this.SyntaxNode as ElementAccessExpressionSyntax; }
        }
    }
}