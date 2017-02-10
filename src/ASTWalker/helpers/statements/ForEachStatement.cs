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

        /// <summary>
        /// Gets an indication whether the ELSE block is present or not.
        /// </summary>
        //public bool HasElseBlock
        //{
        //    get
        //    {
        //        if (!this.hasElseBlock.HasValue)
        //        {
        //            this.CalculateCachedQuantities();
        //        }

        //        return this.hasElseBlock.Value;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        //public int BlocksNumber
        //{
        //    get
        //    {
        //        if (!this.blocksNumber.HasValue)
        //        {
        //            this.CalculateCachedQuantities();
        //        }

        //        return this.blocksNumber.Value;
        //    }
        //}

        //private void CalculateCachedQuantities()
        //{
        //    var ifStatement = this.IfStatementSyntaxNode;
        //    var blocksNum = 0;
        //    var hasFinalElse = false;

        //    while (true)
        //    {
        //        blocksNum++;

        //        if (ifStatement.Else == null)
        //        {
        //            // No final else, end of ifs
        //            break;
        //        }

        //        var ifElseStatement = ifStatement.Else.Statement as IfStatementSyntax;
        //        if (ifElseStatement == null)
        //        {
        //            hasFinalElse = true;
        //            break;
        //        }

        //        // One more if else
        //        ifStatement = ifElseStatement;
        //    }

        //    this.hasElseBlock = hasFinalElse;
        //    this.blocksNumber = blocksNum;
        //}

        private ForEachStatementSyntax ForEachStatementSyntaxNode
        {
            get { return this.SyntaxNode as ForEachStatementSyntax; }
        }
    }
}
