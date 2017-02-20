/// <summary>
/// SwitchStatementASTWalker.cs
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
    public class SwitchStatementASTWalker : StatementASTWalker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchStatementASTWalker"/> class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="conditionalStatement"></param>
        /// <param name="semanticModel">The semantic model.</param>
        protected SwitchStatementASTWalker(CSharpSyntaxNode node, SwitchStatementTranslationUnit SwitchStatement, SemanticModel semanticModel) 
            : base(node, semanticModel)
        {
            var statementSyntaxNode = node as SwitchStatementSyntax;
            if (statementSyntaxNode == null)
            {
                throw new ArgumentException(
                    string.Format("Specified node is not of type {0}",
                    typeof(ForStatementSyntax).Name));
            }

            if (SwitchStatement == null)
            {
                throw new ArgumentNullException(nameof(SwitchStatement));
            }

            // Node assigned in base, no need to assign it here
            this.statement = SwitchStatement;

            // Going through parts in the statement and filling the translation unit with initial data
            this.VisitNode(statementSyntaxNode, 0);
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="SwitchStatementASTWalker"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public SwitchStatementASTWalker(SwitchStatementASTWalker other)
            : base(other)
        {
        }

        /// <summary>
        /// Factory method for class <see cref="SwitchStatementASTWalker"/>.
        /// </summary>
        /// <param name="node"><see cref="CSharpSyntaxNode"/> Used to initialize the walker.</param>
        /// <param name="semanticModel">The semantic model.</param>
        /// <returns></returns>
        public static SwitchStatementASTWalker Create(CSharpSyntaxNode node, SemanticModel semanticModel = null)
        {
            SwitchStatement helper = new SwitchStatement(node as SwitchStatementSyntax);

            var statement = SwitchStatementTranslationUnit.Create();

            return new SwitchStatementASTWalker(node, statement, semanticModel);
        }

        /// <summary>
        /// In charge of executing a fixed visit of this node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        private void VisitNode(SwitchStatementSyntax node, int index)
        {
            //Passing through strings makes it a little easier to initialize the lambda 'foreach' statement.
            this.Statement.SetSwitchExpression(
                new ExpressionTranslationUnitBuilder(node.Expression, this.semanticModel).Build());

            //TODO: Handle the body of the switch statement
            // Handling body
            //IASTWalker walker = (node.Statement as BlockSyntax != null) ? 
            //    BlockASTWalker.Create(node.Statement) : 
            //    new StatementASTWalkerBuilder(node.Statement).Build();
            //ITranslationUnit translationUnit = walker.Walk();
            //this.Statement.SetStatementInConditionalBlock(translationUnit);
        }

        /// <summary>
        /// Gets the <see cref="SwitchStatementTranslationUnit"/> associated to the AST walker.
        /// </summary>
        protected SwitchStatementTranslationUnit Statement
        {
            get { return this.statement as SwitchStatementTranslationUnit; }
        }
    }
}
