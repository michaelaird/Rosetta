/// <summary>
/// ForStatementTranslationUnit.cs
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
    public class ForStatementTranslationUnit : StatementTranslationUnit
    {
        /// <summary>
        /// Contains the array to run through, and the local variable initialized
        /// For Example: foreach (string recipient in EmailRecipients)
        /// </summary>
        protected ITranslationUnit forExpression;


        /// <summary>
        /// Contains the 
        /// </summary>
        protected ITranslationUnit body;
        /// <summary>


        /// <summary>
        /// Initializes a new instance of the <see cref="ForStatementTranslationUnit"/> class.
        /// </summary>
        protected ForStatementTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected ForStatementTranslationUnit(int nestingLevel)
            : base(nestingLevel)
        {
            this.forExpression = null;
            this.body = null;
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="ForStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        protected ForStatementTranslationUnit(ForStatementTranslationUnit other) 
            : base(other)
        {
            this.forExpression = other.forExpression;
            this.body = other.body;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocksNumber"></param>
        /// <param name="hasFinalElse"></param>
        /// <returns></returns>
        public static ForStatementTranslationUnit Create()
        {
            return new ForStatementTranslationUnit(AutomaticNestingLevel)
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
                writer.WriteLine("for {0}let variable of {1}{2}",
                    Lexems.OpenRoundBracket, this.forExpression.Translate(), Lexems.CloseRoundBracket);

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
        public void SetForExpression(ITranslationUnit forExpression)
        {
            if (forExpression == null)
            {
                throw new ArgumentNullException(nameof(forExpression));
            }
            //if (forEachExpression as ExpressionTranslationUnit == null)
            //{
            //    throw new ArgumentException(nameof(forEachExpression), "Expected an expression!");
            //}

            this.forExpression = forExpression;
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
