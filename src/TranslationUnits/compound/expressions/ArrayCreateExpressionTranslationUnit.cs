/// <summary>
/// ArrayCreateExpressionTranslationUnit.cs
/// Michael Aird - 2017
/// </summary>

namespace Rosetta.Translation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class describing member access syntaxes.
    /// </summary>
    public class ArrayCreateExpressionTranslationUnit : ExpressionTranslationUnit
    {
        private ITranslationUnit expression;
        private IEnumerable<ITranslationUnit> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayCreateExpressionTranslationUnit"/> class.
        /// </summary>
        protected ArrayCreateExpressionTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayCreateExpressionTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected ArrayCreateExpressionTranslationUnit(int nestingLevel)
            : base(nestingLevel)
        {
            this.expression = null;
            this.arguments = new List<ITranslationUnit>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="accessMethod"></param>
        /// <returns></returns>
        public static ArrayCreateExpressionTranslationUnit Create(ITranslationUnit invokeeName)
        {
            if (invokeeName == null)
            {
                throw new ArgumentNullException(nameof(invokeeName));
            }

            return new ArrayCreateExpressionTranslationUnit(AutomaticNestingLevel)
            {
                expression = invokeeName
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

            // Invokation: ([<initializers>])
            writer.WriteLine("{0}",
                SyntaxUtility.ToSquareBracketEnclosedList(this.arguments.Select(unit => unit.Translate()))
                );

            return writer.ToString();
        }

        #region Compound translation unit methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translationUnit"></param>
        public void AddArgument(ITranslationUnit translationUnit)
        {
            if (translationUnit == null)
            {
                throw new ArgumentNullException(nameof(translationUnit));
            }

            ((List<ITranslationUnit>)this.arguments).Add(translationUnit);
        }

        #endregion
    }
}
