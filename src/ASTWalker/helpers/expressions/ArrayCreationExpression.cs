/// <summary>
/// ArrayCreationExpression.cs
/// Michael Aird - 2017
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
    public class ArrayCreationExpression : Helper
    {
        // Cached values
        private IEnumerable<Argument> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokationExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public ArrayCreationExpression(ArrayCreationExpressionSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokationExpression"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public ArrayCreationExpression(ArrayCreationExpressionSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public ExpressionSyntax Expression
        {
            get { return this.ArrayCreationExpressionSyntaxNode.Type; }
        }

        public InitializerExpressionSyntax Initializer
        {
            get
            {
                return this.ArrayCreationExpressionSyntaxNode.Initializer;
            }
        }

        private ArrayCreationExpressionSyntax ArrayCreationExpressionSyntaxNode
        {
            get { return this.SyntaxNode as ArrayCreationExpressionSyntax; }
        }
    }
}
