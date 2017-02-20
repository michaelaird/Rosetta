/// <summary>
/// SwitchStatementTranslationUnit.cs
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
    public class SwitchStatementTranslationUnit : StatementTranslationUnit
    {
        /// <summary>
        /// Contains the array to run through, and the local variable initialized
        /// For Example: foreach (string recipient in EmailRecipients)
        /// </summary>
        protected ITranslationUnit switchExpression;

        /// <summary>
        /// Contains the
        /// </summary>
        protected ITranslationUnit body;

        /// <summary>

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchStatementTranslationUnit"/> class.
        /// </summary>
        protected SwitchStatementTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected SwitchStatementTranslationUnit(int nestingLevel)
            : base(nestingLevel)
        {
            this.switchExpression = null;
            this.body = null;
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="SwitchStatementTranslationUnit"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        protected SwitchStatementTranslationUnit(SwitchStatementTranslationUnit other)
            : base(other)
        {
            this.switchExpression = other.switchExpression;
            this.body = other.body;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="blocksNumber"></param>
        /// <param name="hasFinalElse"></param>
        /// <returns></returns>
        public static SwitchStatementTranslationUnit Create()
        {
            return new SwitchStatementTranslationUnit(AutomaticNestingLevel)
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
            writer.WriteLine("switch {0}{1}{2}",
                Lexems.OpenRoundBracket, this.switchExpression.Translate(), Lexems.CloseRoundBracket);

            writer.WriteLine("{0}",
                Lexems.OpenCurlyBracket);

            // Body
            if (this.body != null)
            {
                writer.WriteLine("{0}",
                this.body.Translate());
            }

            // Closing
            writer.WriteLine("{0}",
                Lexems.CloseCurlyBracket);

            return writer.ToString();
        }

        #region Compound translation unit methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="testExpression"></param>
        /// <param name="index"></param>
        public void SetSwitchExpression(ITranslationUnit switchExpression)
        {
            if (switchExpression == null)
            {
                throw new ArgumentNullException(nameof(switchExpression));
            }
            //if (forEachExpression as ExpressionTranslationUnit == null)
            //{
            //    throw new ArgumentException(nameof(forEachExpression), "Expected an expression!");
            //}

            this.switchExpression = switchExpression;
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

        #endregion Compound translation unit methods
    }
}