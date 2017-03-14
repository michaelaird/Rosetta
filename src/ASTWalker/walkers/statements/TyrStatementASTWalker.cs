/// <summary>
/// TryStatementASTWalker.cs
/// Michael Aird - 2017
/// </summary>

namespace Rosetta.AST
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using Rosetta.Translation;
    using Rosetta.AST.Helpers;

    /// <summary>
    /// Describes walkers in iterative statements.
    /// </summary>
    public class TryStatementASTWalker : StatementASTWalker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryStatementASTWalker"/> class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="conditionalStatement"></param>
        /// <param name="semanticModel">The semantic model.</param>
        protected TryStatementASTWalker(CSharpSyntaxNode node, TryStatementTranslationUnit TryStatement, SemanticModel semanticModel) 
            : base(node, semanticModel)
        {
            var statementSyntaxNode = node as TryStatementSyntax;
            if (statementSyntaxNode == null)
            {
                throw new ArgumentException(
                    string.Format("Specified node is not of type {0}",
                    typeof(ForStatementSyntax).Name));
            }

            if (TryStatement == null)
            {
                throw new ArgumentNullException(nameof(TryStatement));
            }

            // Node assigned in base, no need to assign it here
            this.statement = TryStatement;

            // Going through parts in the statement and filling the translation unit with initial data
            this.VisitNode(statementSyntaxNode, 0);
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="TryStatementASTWalker"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public TryStatementASTWalker(TryStatementASTWalker other)
            : base(other)
        {
        }

        /// <summary>
        /// Factory method for class <see cref="TryStatementASTWalker"/>.
        /// </summary>
        /// <param name="node"><see cref="CSharpSyntaxNode"/> Used to initialize the walker.</param>
        /// <param name="semanticModel">The semantic model.</param>
        /// <returns></returns>
        public static TryStatementASTWalker Create(CSharpSyntaxNode node, SemanticModel semanticModel = null)
        {
            TryStatement helper = new TryStatement(node as TryStatementSyntax);

            var statement = TryStatementTranslationUnit.Create();

            return new TryStatementASTWalker(node, statement, semanticModel);
        }

        /// <summary>
        /// In charge of executing a fixed visit of this node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        private void VisitNode(TryStatementSyntax node, int index)
        {
            // Handling body
            IASTWalker walker = (node.Block as BlockSyntax != null) ?
                BlockASTWalker.Create(node.Block) :
                new StatementASTWalkerBuilder(node.Block).Build();
            ITranslationUnit translationUnit = walker.Walk();
            this.Statement.SetBlockExpression(translationUnit);
        }

        /// <summary>
        /// Gets the <see cref="TryStatementTranslationUnit"/> associated to the AST walker.
        /// </summary>
        protected TryStatementTranslationUnit Statement
        {
            get { return this.statement as TryStatementTranslationUnit; }
        }
    }
}
