/// <summary>
/// DefaultExpression.cs
/// Scott Bannister -2017
/// I'm creating this class to handle the cases which haven't been implemented yet
/// and just pass the node through unchanged, allowing the program to continue running without throwing an exception
/// </summary>

namespace Rosetta.AST.Helpers
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Rosetta.Translation;

    /// <summary>
    /// Helper for accessing default expressions in AST. This is meant to pass the node through unchanged.
    /// </summary>
    public class DefaultExpression : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public DefaultExpression(CSharpSyntaxNode syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public DefaultExpression(CSharpSyntaxNode syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        /// <summary>
        /// Gets the literal value.
        /// </summary>
        public SyntaxToken Literal
        {
            get { return this.Literal; }
        }
    }
}
