/// <summary>
/// MemberAccessExpressionTranslationUnit.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.Translation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class describing member access syntaxes.
    /// </summary>
    public class ElementAccessExpressionTranslationUnit : ExpressionTranslationUnit, ICompoundTranslationUnit
    {
        private ITranslationUnit left;
        private ITranslationUnit member;
        private ElementAccessMethod accessMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAccessExpressionTranslationUnit"/> class.
        /// </summary>
        protected ElementAccessExpressionTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAccessExpressionTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected ElementAccessExpressionTranslationUnit(int nestingLevel)
            : base(nestingLevel)
        {
            this.member = null;
            this.accessMethod = ElementAccessMethod.This;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="member"></param>
        /// <param name="accessMethod"></param>
        /// <returns></returns>
        public static ElementAccessExpressionTranslationUnit Create(ITranslationUnit member, ElementAccessMethod accessMethod)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            return new ElementAccessExpressionTranslationUnit(AutomaticNestingLevel)
            {
                Member = member,
                accessMethod = accessMethod
            };
        }

        public static ElementAccessExpressionTranslationUnit Create(ITranslationUnit left, ITranslationUnit member)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            var unit = ElementAccessExpressionTranslationUnit.Create(member, ElementAccessMethod.Member);
            unit.left = left;

            return unit;
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<ITranslationUnit> InnerUnits
        {
            get
            {
                return new ITranslationUnit[] { this.Member };
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

            if (this.accessMethod == ElementAccessMethod.None)
            {
                writer.Write("{0}", this.Member.Translate());
            }
            else
            {
                writer.Write("{0}{1}{2}{3}",
                ElementAccessMethod2String(this.accessMethod),
                Lexems.OpenSquareBracket,
                this.Member.Translate(),
                Lexems.CloseSquareBracket);
            }

            return writer.ToString();
        }

        #region Compound translation unit methods

        private ITranslationUnit Member
        {
            get { return this.member; }
            set { this.member = value; }
        }

        #endregion Compound translation unit methods

        private string ElementAccessMethod2String(ElementAccessMethod accessMethod)
        {
            switch (accessMethod)
            {
                case ElementAccessMethod.Base:
                    return Lexems.BaseKeyword;

                case ElementAccessMethod.This:
                    return Lexems.ThisKeyword;

                case ElementAccessMethod.Member:
                    return left.Translate();

                default:
                    return string.Empty;
            }
        }

        #region Types

        /// <summary>
        /// Enumerates member access methods
        /// </summary>
        public enum ElementAccessMethod
        {
            /// <summary>
            ///
            /// </summary>
            This,

            /// <summary>
            ///
            /// </summary>
            Base,

            /// <summary>
            ///
            /// </summary>
            Member,

            /// <summary>
            ///
            /// </summary>
            None
        }

        #endregion Types
    }
}