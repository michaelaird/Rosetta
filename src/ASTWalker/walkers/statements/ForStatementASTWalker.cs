/// <summary>
/// ForEachStatementASTWalker.cs
/// Scott Bannister - 2017
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
    public class ForStatementASTWalker : StatementASTWalker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForStatementASTWalker"/> class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="conditionalStatement"></param>
        /// <param name="semanticModel">The semantic model.</param>
        protected ForStatementASTWalker(CSharpSyntaxNode node, ForStatementTranslationUnit forStatement, SemanticModel semanticModel) 
            : base(node, semanticModel)
        {
            var statementSyntaxNode = node as ForStatementSyntax;
            if (statementSyntaxNode == null)
            {
                throw new ArgumentException(
                    string.Format("Specified node is not of type {0}",
                    typeof(ForStatementSyntax).Name));
            }

            if (forStatement == null)
            {
                throw new ArgumentNullException(nameof(forStatement));
            }

            // Node assigned in base, no need to assign it here
            this.statement = forStatement;

            // Going through parts in the statement and filling the translation unit with initial data
            this.VisitNode(statementSyntaxNode, 0);
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="ForStatementASTWalker"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public ForStatementASTWalker(ForStatementASTWalker other)
            : base(other)
        {
        }

        /// <summary>
        /// Factory method for class <see cref="ForStatementASTWalker"/>.
        /// </summary>
        /// <param name="node"><see cref="CSharpSyntaxNode"/> Used to initialize the walker.</param>
        /// <param name="semanticModel">The semantic model.</param>
        /// <returns></returns>
        public static ForStatementASTWalker Create(CSharpSyntaxNode node, SemanticModel semanticModel = null)
        {
            ForStatement helper = new ForStatement(node as ForStatementSyntax);

            var statement = ForStatementTranslationUnit.Create();

            return new ForStatementASTWalker(node, statement, semanticModel);
        }

        /// <summary>
        /// In charge of executing a fixed visit of this node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        private void VisitNode(ForStatementSyntax node, int index)
        {
            //for condition
            this.Statement.SetForExpression(
                new ExpressionTranslationUnitBuilder(node.Condition, this.semanticModel).Build());


            // Handling body
            IASTWalker walker = (node.Statement as BlockSyntax != null) ? 
                BlockASTWalker.Create(node.Statement) : 
                new StatementASTWalkerBuilder(node.Statement).Build();
            ITranslationUnit translationUnit = walker.Walk();
            this.Statement.SetStatementInConditionalBlock(translationUnit);
        }

        /// <summary>
        /// Gets the <see cref="ForStatementTranslationUnit"/> associated to the AST walker.
        /// </summary>
        protected ForStatementTranslationUnit Statement
        {
            get { return this.statement as ForStatementTranslationUnit; }
        }
    }
}
