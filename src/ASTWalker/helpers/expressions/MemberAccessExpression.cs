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
    public class MemberAccessExpression : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public MemberAccessExpression(MemberAccessExpressionSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public MemberAccessExpression(MemberAccessExpressionSyntax syntaxNode, SemanticModel semanticModel)
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
                switch (this.MemberAccessExpressionSyntaxNode.Name.Identifier.ValueText)
                {
                    case "ObservableArray":
                        return "observableArray";

                    case "Observable":
                        return "observable";

                    default:
                        return this.MemberAccessExpressionSyntaxNode.Name.Identifier.ValueText;
                }
            }
        }

        private MemberAccessExpressionSyntax MemberAccessExpressionSyntaxNode
        {
            get { return this.SyntaxNode as MemberAccessExpressionSyntax; }
        }
    }
}