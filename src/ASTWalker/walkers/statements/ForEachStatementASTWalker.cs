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
    public class ForEachStatementASTWalker : StatementASTWalker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalStatementASTWalker"/> class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="conditionalStatement"></param>
        /// <param name="semanticModel">The semantic model.</param>
        protected ForEachStatementASTWalker(CSharpSyntaxNode node, ForEachStatementTranslationUnit forEachStatement, SemanticModel semanticModel) 
            : base(node, semanticModel)
        {
            var statementSyntaxNode = node as ForEachStatementSyntax;
            if (statementSyntaxNode == null)
            {
                throw new ArgumentException(
                    string.Format("Specified node is not of type {0}",
                    typeof(IfStatementSyntax).Name));
            }

            if (forEachStatement == null)
            {
                throw new ArgumentNullException(nameof(forEachStatement));
            }

            // Node assigned in base, no need to assign it here
            this.statement = forEachStatement;

            // Going through parts in the statement and filling the translation unit with initial data
            this.VisitNode(statementSyntaxNode, 0);
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="ForEachStatementASTWalker"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public ForEachStatementASTWalker(ForEachStatementASTWalker other)
            : base(other)
        {
        }

        /// <summary>
        /// Factory method for class <see cref="ForEachStatementASTWalker"/>.
        /// </summary>
        /// <param name="node"><see cref="CSharpSyntaxNode"/> Used to initialize the walker.</param>
        /// <param name="semanticModel">The semantic model.</param>
        /// <returns></returns>
        public static ForEachStatementASTWalker Create(CSharpSyntaxNode node, SemanticModel semanticModel = null)
        {
            ForEachStatement helper = new ForEachStatement(node as ForEachStatementSyntax);

            var statement = ForEachStatementTranslationUnit.Create();

            return new ForEachStatementASTWalker(node, statement, semanticModel);
        }

        /// <summary>
        /// In charge of executing a fixed visit of this node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        private void VisitNode(ForEachStatementSyntax node, int index)
        {
            //Passing through strings makes it a little easier to initialize the lambda 'foreach' statement.
            string[] forEachExpression = new string[3] { node.Type.ToString(), node.Identifier.ValueText, node.Expression.ToString() } ;
            this.Statement.SetForEachExpression(forEachExpression);

            // Handling body
            IASTWalker walker = (node.Statement as BlockSyntax != null) ? 
                BlockASTWalker.Create(node.Statement) : 
                new StatementASTWalkerBuilder(node.Statement).Build();
            ITranslationUnit translationUnit = walker.Walk();
            this.Statement.SetStatementInConditionalBlock(translationUnit);
        }

        /// <summary>
        /// Gets the <see cref="ForEachStatementTranslationUnit"/> associated to the AST walker.
        /// </summary>
        protected ForEachStatementTranslationUnit Statement
        {
            get { return this.statement as ForEachStatementTranslationUnit; }
        }
    }
}
