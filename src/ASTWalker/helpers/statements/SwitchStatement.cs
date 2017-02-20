/// <summary>
/// SwitchStatement.cs
/// Michael Aird - 2017
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Helper for accessing conditional statements.
    /// </summary>
    public class SwitchStatement : Helper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        public SwitchStatement(SwitchStatementSyntax syntaxNode)
            : this(syntaxNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalStatement"/> class.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <param name="semanticModel"></param>
        public SwitchStatement(SwitchStatementSyntax syntaxNode, SemanticModel semanticModel)
            : base(syntaxNode, semanticModel)
        {
        }

        private SwitchStatementSyntax SwitchStatementSyntaxNode
        {
            get { return this.SyntaxNode as SwitchStatementSyntax; }
        }
    }
}