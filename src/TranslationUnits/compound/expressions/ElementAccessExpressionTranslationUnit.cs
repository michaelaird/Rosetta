﻿/// <summary>
/// ElementAccessExpressionTranslationUnit.cs
/// Scott Bannister - 2017
/// </summary>

namespace Rosetta.Translation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class describing member access syntaxes.
    /// </summary>
    public class ElementAccessExpressionTranslationUnit : ExpressionTranslationUnit, ICompoundTranslationUnit
    {
        private ITranslationUnit expression;
        private IEnumerable<ITranslationUnit> arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokationExpressionTranslationUnit"/> class.
        /// </summary>
        protected ElementAccessExpressionTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokationExpressionTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected ElementAccessExpressionTranslationUnit(int nestingLevel)
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
        public static ElementAccessExpressionTranslationUnit Create(ITranslationUnit invokeeName)
        {
            if (invokeeName == null)
            {
                throw new ArgumentNullException(nameof(invokeeName));
            }

            return new ElementAccessExpressionTranslationUnit(AutomaticNestingLevel)
            {
                expression = invokeeName
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ITranslationUnit> InnerUnits
        {
            get
            {
                return this.arguments;
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

            // Invokation: <expression>([<params>])
            writer.WriteLine("{0}{1}",
                this.expression.Translate(),
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