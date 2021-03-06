﻿/// <summary>
/// ForEachStatement.cs
/// Scott Bannister - 2017
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Helper for accessing conditional statements.
    /// </summary>
    public class ForEachStatement : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForEachStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public ForEachStatement(ForEachStatementSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public ForEachStatement(ForEachStatementSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        private ForEachStatementSyntax ForEachStatementSyntaxNode
        {
            get { return this.SyntaxNode as ForEachStatementSyntax; }
        }
    }
}