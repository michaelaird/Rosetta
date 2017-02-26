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
    public class MemberAccessExpressionTranslationUnit : ExpressionTranslationUnit, ICompoundTranslationUnit
    {
        private ITranslationUnit left;
        private ITranslationUnit member;
        private MemberAccessMethod accessMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessExpressionTranslationUnit"/> class.
        /// </summary>
        protected MemberAccessExpressionTranslationUnit()
            : this(AutomaticNestingLevel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessExpressionTranslationUnit"/> class.
        /// </summary>
        /// <param name="nestingLevel"></param>
        protected MemberAccessExpressionTranslationUnit(int nestingLevel)
            : base(nestingLevel)
        {
            this.member = null;
            this.accessMethod = MemberAccessMethod.This;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="member"></param>
        /// <param name="accessMethod"></param>
        /// <returns></returns>
        public static MemberAccessExpressionTranslationUnit Create(ITranslationUnit member, MemberAccessMethod accessMethod)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            return new MemberAccessExpressionTranslationUnit(AutomaticNestingLevel)
            {
                Member = member,
                accessMethod = accessMethod
            };
        }

        public static MemberAccessExpressionTranslationUnit Create(ITranslationUnit left, ITranslationUnit member)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            var unit = MemberAccessExpressionTranslationUnit.Create(member, MemberAccessMethod.Member);
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

            if (this.accessMethod == MemberAccessMethod.None)
            {
                writer.Write("{0}", this.Member.Translate());
            }
            else
            {
                writer.Write("{0}{1}{2}",
                MemberAccessMethod2String(this.accessMethod),
                Lexems.Dot,
                this.Member.Translate());
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

        private string MemberAccessMethod2String(MemberAccessMethod accessMethod)
        {
            switch (accessMethod)
            {
                case MemberAccessMethod.Base:
                    return Lexems.BaseKeyword;

                case MemberAccessMethod.This:
                    return Lexems.ThisKeyword;

                case MemberAccessMethod.Member:
                    return left.Translate();

                default:
                    return string.Empty;
            }
        }

        #region Types

        /// <summary>
        /// Enumerates member access methods
        /// </summary>
        public enum MemberAccessMethod
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