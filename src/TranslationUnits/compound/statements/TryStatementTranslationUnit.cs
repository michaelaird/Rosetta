/// <summary>
/// TryStatementTranslationUnit.cs
/// Michael Aird - 2017
/// </summary>

namespace Rosetta.Translation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class describing conditional statements.
    /// </summary>
    /// <remarks>
    /// Internal members protected for testability.
    /// </remarks>
    public class TryStatementTranslationUnit : StatementTranslationUnit
    {
        /// <summary>
        /// </summary>
        protected ITranslationUnit block;

        /// <summary>

        /// </summary>
        protected ITranslationUnit[] catches;
        /// <summary>

        /// </summary>
        protected ITranslationUnit finallyBlock;


        /// <summary>

        /// <summary>
        /// Initializes a new instance of the <see cref="TryStatementTranslationUnit"/> class.
        /// </summary>
        protected TryStatementTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TryStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected TryStatementTranslationUnit(int nestingLevel)
            : base(nestingLevel)
        {
            this.block = null;
            this.catches = new ITranslationUnit[] { };
            this.finallyBlock = null;
            //this.body = null;
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="TryStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        protected TryStatementTranslationUnit(TryStatementTranslationUnit other)
            : base(other)
        {
            this.block = other.block;
            this.catches = other.catches;
            this.finallyBlock = other.finallyBlock;
            //this.body = other.body;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="blocksNumber"></param>
        /// <param name="hasFinalElse"></param>
        /// <returns></returns>
        public static TryStatementTranslationUnit Create()
        {
            return new TryStatementTranslationUnit(AutomaticNestingLevel)
            {
            };
        }

        /// <summary>
        /// Translate the unit into TypeScript.
        /// </summary>
        /// <returns></returns>
        public override string Translate()
        {
            FormatWriter writer = new FormatWriter()
            {
                Formatter = this.Formatter
            };

            // Opening
            writer.WriteLine("try");

            writer.WriteLine("{0}",
                Lexems.OpenCurlyBracket);

            // Block
            if (this.block != null)
            {
                writer.WriteLine("{0}",
                this.block.Translate());
            }

            // Closing
            writer.WriteLine("{0}",
                Lexems.CloseCurlyBracket);

            //TODO: handle catches

            //TODO: handle finally

            return writer.ToString();
        }

        #region Compound translation unit methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="testExpression"></param>
        /// <param name="index"></param>
        public void SetBlockExpression(ITranslationUnit blockExpression)
        {
            if (blockExpression == null)
            {
                throw new ArgumentNullException(nameof(blockExpression));
            }

            this.block = blockExpression;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="statement">Can be a block or a statement.</param>
        ///// <param name="index"></param>
        //public void SetStatementInConditionalBlock(ITranslationUnit statement)
        //{
        //    if (statement == null)
        //    {
        //        throw new ArgumentNullException(nameof(statement));
        //    }

        //    if (statement as NestedElementTranslationUnit != null)
        //    {
        //        ((NestedElementTranslationUnit)statement).NestingLevel = this.NestingLevel + 1;
        //    }

        //    //this.body = statement;
        //}

        #endregion Compound translation unit methods
    }
}