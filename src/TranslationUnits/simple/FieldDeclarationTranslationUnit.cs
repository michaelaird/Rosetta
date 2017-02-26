/// <summary>
/// FieldDeclarationTranslationUnit.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.Translation
{
    using System;

    /// <summary>
    /// Class describing a method signature (no body).
    /// </summary>
    public class FieldDeclarationTranslationUnit : MemberTranslationUnit, ITranslationUnit
    {
        private ITranslationUnit type;
        protected ITranslationUnit[] expressions; // Can be null

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDeclarationTranslationUnit"/> class.
        /// </summary>
        protected FieldDeclarationTranslationUnit()
            : this(IdentifierTranslationUnit.Empty, VisibilityToken.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDeclarationTranslationUnit"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="visibility"></param>
        protected FieldDeclarationTranslationUnit(ITranslationUnit name, VisibilityToken visibility)
            : base(name, visibility)
        {
            this.Type = null;
            this.expressions = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FieldDeclarationTranslationUnit Create(VisibilityToken visibility, ITranslationUnit type, ITranslationUnit name, ITranslationUnit[] expressions = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new FieldDeclarationTranslationUnit()
            {
                Visibility = visibility,
                Name = name,
                Type = type,
                expressions = expressions
            };
        }

        /// <summary>
        /// Translate the unit into TypeScript.
        /// </summary>
        /// <returns></returns>
        public virtual string Translate()
        {
            FormatWriter writer = new FormatWriter()
            {
                Formatter = this.Formatter
            };

            // Opening declaration
            string fieldVisibility = this.RenderedVisibilityModifier;

            if (this.Expression == null)
            {
                writer.Write("{0}{1} {2} {3}{4}",
                text => ClassDeclarationCodePerfect.RefineDeclaration(text),
                this.Visibility.ConvertToTypeScriptEquivalent().EmitOptionalVisibility(),
                this.RenderedName,
                Lexems.Colon,
                this.type.Translate(), Lexems.Newline);
            }
            else
            {
                writer.Write("{0}{1} {2} {3} {4} {5}{6}",
               text => ClassDeclarationCodePerfect.RefineDeclaration(text),
               this.Visibility.ConvertToTypeScriptEquivalent().EmitOptionalVisibility(),
               this.RenderedName,
               Lexems.Colon,
               this.type.Translate(),
               Lexems.EqualsSign,
               this.Expression.Translate(),Lexems.Newline);
            }

            return writer.ToString();
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        protected ITranslationUnit Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        protected virtual string RenderedVisibilityModifier => TokenUtility.EmitOptionalVisibility(this.Visibility);

        protected virtual string RenderedName => this.Name.Translate();

        /// <summary>
        /// Until we support multiple declarations, we need to get the expression if it exists.
        /// </summary>
        private ITranslationUnit Expression
        {
            get { return this.expressions == null ? null : this.expressions[0]; }
        }
    }
}
