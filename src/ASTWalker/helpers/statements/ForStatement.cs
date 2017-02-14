/// <summary>
/// ForEachStatement.cs
/// Scott Bannister - 2017
/// </summary>

namespace Rosetta.AST.Helpers
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Helper for accessing conditional statements.
    /// </summary>
    public class ForStatement : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public ForStatement(ForStatementSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public ForStatement(ForStatementSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        private ForStatementSyntax ForStatementSyntaxNode
        {
            get { return this.SyntaxNode as ForStatementSyntax; }
        }
    }
}
