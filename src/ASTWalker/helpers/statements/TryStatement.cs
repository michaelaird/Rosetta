/// <summary>
/// TryStatement.cs
/// Michael Aird - 2017
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Helper for accessing conditional statements.
    /// </summary>
    public class TryStatement : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public TryStatement(TryStatementSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public TryStatement(TryStatementSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        private TryStatementSyntax TryStatementSyntaxNode
        {
            get { return this.SyntaxNode as TryStatementSyntax; }
        }
    }
}