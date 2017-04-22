/// <summary>
/// ForEachStatementTranslationUnit.cs
/// Scott Bannister - 2017
/// </summary>

namespace Rosetta.Translation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Class describing conditional statements.
    /// </summary>
    /// <remarks>
    /// Internal members protected for testability.
    /// </remarks>
    public class ForEachStatementTranslationUnit : StatementTranslationUnit
    {
        /// <summary>
        /// Contains the array to run through, and the local variable initialized
        /// For Example: foreach (string recipient in EmailRecipients)
        /// </summary>
        //protected ITranslationUnit forEachExpression;
        protected string[] forEachExpression;


        /// <summary>
        /// Contains the 
        /// </summary>
        protected ITranslationUnit body;
        /// <summary>


        /// <summary>
        /// Initializes a new instance of the <see cref="ForEachStatementTranslationUnit"/> class.
        /// </summary>
        protected ForEachStatementTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForEachStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected ForEachStatementTranslationUnit(int nestingLevel)
            : base(nestingLevel)
        {
            this.forEachExpression = null;
            this.body = null;
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="ForEachStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        protected ForEachStatementTranslationUnit(ForEachStatementTranslationUnit other) 
            : base(other)
        {
            this.forEachExpression = other.forEachExpression;
            this.body = other.body;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocksNumber"></param>
        /// <param name="hasFinalElse"></param>
        /// <returns></returns>
        public static ForEachStatementTranslationUnit Create()
        {
            return new ForEachStatementTranslationUnit(AutomaticNestingLevel)
            {
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<ITranslationUnit> InnerUnits
        {
            get
            {
                return null;
            }
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
                writer.WriteLine("{0}.forEach(({1}: {2}{3} {4}{5}",
                    this.forEachExpression[2],this.forEachExpression[1], this.forEachExpression[0], Lexems.CloseRoundBracket, Lexems.EqualsSign, Lexems.CloseAngularBracket);

                writer.WriteLine("{0}",
                    Lexems.OpenCurlyBracket);

                // Body
                writer.WriteLine("{0}",
                    this.body.Translate());

            // Closing
            writer.WriteLine("{0}{1}",
                Lexems.CloseCurlyBracket, Lexems.CloseRoundBracket);

            return writer.ToString();
        }

        #region Compound translation unit methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testExpression"></param>
        /// <param name="index"></param>
        public void SetForEachExpression(string[] forEachExpression)
        {
            if (forEachExpression == null)
            {
                throw new ArgumentNullException(nameof(forEachExpression));
            }
            //if (forEachExpression as ExpressionTranslationUnit == null)
            //{
            //    throw new ArgumentException(nameof(forEachExpression), "Expected an expression!");
            //}

            this.forEachExpression = forEachExpression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement">Can be a block or a statement.</param>
        /// <param name="index"></param>
        public void SetStatementInConditionalBlock(ITranslationUnit statement)
        {
            if (statement == null)
            {
                throw new ArgumentNullException(nameof(statement));
            }

            if (statement as NestedElementTranslationUnit != null)
            {
                ((NestedElementTranslationUnit)statement).NestingLevel = this.NestingLevel + 1;
            }

            this.body = statement;
        }
        #endregion
    }
}
