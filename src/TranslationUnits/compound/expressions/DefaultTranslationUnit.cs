/// <summary>
/// DefaultTranslationUnit.cs
/// Scott Bannister - 2017
/// </summary>

namespace Rosetta.Translation
{
    using System;

    /// <summary>
    /// Translation unit for literals.
    /// </summary>
    public class DefaultTranslationUnit<T> : ExpressionTranslationUnit
    {
        protected T literalValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTranslationUnit(T)"/> class.
        /// </summary>
        /// <param name="name"></param>
        protected DefaultTranslationUnit(T value)
        {
            this.literalValue = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DefaultTranslationUnit<T> Create(T value)
        {
            return new DefaultTranslationUnit<T>(value);
        }

        /// <summary>
        /// Translate the unit into TypeScript.
        /// </summary>
        /// <returns></returns>
        public override string Translate()
        {
            string value = this.literalValue.ToString();

            return value;
        }
    }

    /// <summary>
    /// Translation unit for literals when we want to directly assign them.
    /// </summary>
    public class DefaultTranslationUnit : DefaultTranslationUnit<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTranslationUnit"/> class.
        /// </summary>
        /// <param name="name"></param>
        private DefaultTranslationUnit(string value) :
            base(value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new static DefaultTranslationUnit Create(string value)
        {
            return new DefaultTranslationUnit(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DefaultTranslationUnit Null
        {
            get { return Create(Lexems.NullKeyword); }
        }

        public override string Translate()
        {
            string value = string.Format("{0}", this.literalValue);

            return value;
        }
    }
}
