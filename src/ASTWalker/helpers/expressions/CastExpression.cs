/// <summary>
/// InvokationExpression.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Rosetta.Translation;

    /// <summary>
    /// Helper for accessing invocation expressions in AST.
    /// </summary>
    public class CastExpression : Helper
    {
        // Cached values
        private IEnumerable<Argument> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokationExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public CastExpression(CastExpressionSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokationExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public CastExpression(CastExpressionSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public ExpressionSyntax Expression
        {
            get { return this.CastExpressionSyntaxNode.Expression; }
        }

        public TypeSyntax Type
        {
            get { return this.CastExpressionSyntaxNode.Type; }
        }

        private CastExpressionSyntax CastExpressionSyntaxNode
        {
            get { return this.SyntaxNode as CastExpressionSyntax; }
        }
    }
}
