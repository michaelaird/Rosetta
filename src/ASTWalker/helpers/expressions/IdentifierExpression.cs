/// <summary>
/// IdentifierExpression.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Helper for accessing identifier expressions in AST.
    /// </summary>
    public class IdentifierExpression : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifierExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public IdentifierExpression(SimpleNameSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifierExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public IdentifierExpression(SimpleNameSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public string Identifier
        {
            get
            {
                switch (this.IdentifierNameSyntaxNode.Identifier.ValueText)
                {
                    case "Knockout":
                        return "ko";
                    case "KnockoutUtils":
                        return "ko.utils";
                    default:
                        return this.IdentifierNameSyntaxNode.Identifier.ValueText;
                }
            }
        }

        private SimpleNameSyntax IdentifierNameSyntaxNode
        {
            get { return this.SyntaxNode as SimpleNameSyntax; }
        }
    }
}