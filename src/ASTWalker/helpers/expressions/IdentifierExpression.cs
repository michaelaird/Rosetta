/// <summary>
/// IdentifierExpression.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Linq;
    using Translation;
    using Utilities;

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
                string identifier = null;

                switch (this.IdentifierNameSyntaxNode.Identifier.ValueText)
                {
                    case "Knockout":
                        identifier = "ko";
                        break;
                    case "KnockoutUtils":
                        identifier = "ko.utils";
                        break;
                    default:
                        identifier = this.IdentifierNameSyntaxNode.Identifier.ValueText;
                        break;
                }

                if (this.IdentifierNameSyntaxNode is GenericNameSyntax)
                {
                    GenericNameSyntax genericSyntax = this.IdentifierNameSyntaxNode as GenericNameSyntax;

                    identifier = identifier +
                                           SyntaxUtility.ToAngleBracketEnclosedList(genericSyntax.TypeArgumentList.Arguments.Select(unit => unit.MapType()));
                }

                return identifier;
            }
        }

        private SimpleNameSyntax IdentifierNameSyntaxNode
        {
            get { return this.SyntaxNode as SimpleNameSyntax; }
        }
    }
}