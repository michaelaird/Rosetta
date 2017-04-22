/// <summary>
/// MemberAccessExpression.cs
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
                string name = null;

                switch (this.MemberAccessExpressionSyntaxNode.Name.Identifier.ValueText)
                {
                    case "ObservableArray":
                        name = "observableArray";
                        break;
                    case "Observable":
                        name = "observable";
                        break;
                    default:
                        name = this.MemberAccessExpressionSyntaxNode.Name.Identifier.ValueText;
                        break;
                }

                if (this.MemberAccessExpressionSyntaxNode.Name is GenericNameSyntax)
                {
                    GenericNameSyntax genericSyntax = this.MemberAccessExpressionSyntaxNode.Name as GenericNameSyntax;

                    name = name +
                                           SyntaxUtility.ToAngleBracketEnclosedList(genericSyntax.TypeArgumentList.Arguments.Select(unit => unit.MapType()));
                }

                return name;
            }
        }

        private MemberAccessExpressionSyntax MemberAccessExpressionSyntaxNode
        {
            get { return this.SyntaxNode as MemberAccessExpressionSyntax; }
        }
    }
}