/// <summary>
/// FieldDeclarationTranslationUnitFactory.cs
/// Andrea Tino - 2016
/// </summary>

namespace Rosetta.AST.Factories
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Rosetta.AST.Helpers;
    using Rosetta.AST.Utilities;
    using Rosetta.Translation;

    /// <summary>
    /// Factory for <see cref="FieldDeclarationTranslationUnit"/>.
    /// </summary>
    public class FieldDeclarationTranslationUnitFactory : TranslationUnitFactory, ITranslationUnitFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDeclarationTranslationUnitFactory"/> class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel">The semantic model</param>
        public FieldDeclarationTranslationUnitFactory(CSharpSyntaxNode node, SemanticModel semanticModel = null)
            : base(node, semanticModel)
        {
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="FieldDeclarationTranslationUnitFactory"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public FieldDeclarationTranslationUnitFactory(FieldDeclarationTranslationUnitFactory other)
            : base(other)
        {
        }

        /// <summary>
        /// Creates a <see cref="MethodDeclarationTranslationUnit"/>.
        /// </summary>
        /// <returns>A <see cref="MethodDeclarationTranslationUnit"/>.</returns>
        public ITranslationUnit Create()
        {
            if (this.DoNotCreateTranslationUnit)
            {
                return null;
            }

            var helper = this.CreateHelper(this.Node as FieldDeclarationSyntax, this.SemanticModel);
            ITranslationUnit fieldDeclaration;

            ExpressionSyntax expression = helper.Expressions[0]; // This can contain null, so need to act accordingly
            ITranslationUnit expressionTranslationUnit = expression == null
                ? null
                : new ExpressionTranslationUnitBuilder(expression, this.SemanticModel).Build();

            fieldDeclaration = this.CreateTranslationUnit(
                helper.Visibility,
                TypeIdentifierTranslationUnit.Create(helper.Type.TypeSyntaxNode.MapType()),
                IdentifierTranslationUnit.Create(helper.Name),
                expressionTranslationUnit);

            return fieldDeclaration;
        }

        /// <summary>
        /// Gets a value indicating whether the factory should return <code>null</code>.
        /// </summary>
        protected virtual bool DoNotCreateTranslationUnit
        {
            get { return false; }
        }

        /// <summary>
        /// Creates the translation unit.
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual ITranslationUnit CreateTranslationUnit(
            VisibilityToken visibility, ITranslationUnit type, ITranslationUnit name)
        {
            return FieldDeclarationTranslationUnit.Create(visibility, type, name, null);
        }

        /// <summary>
        /// Creates the translation unit.
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected virtual ITranslationUnit CreateTranslationUnit(
            VisibilityToken visibility, ITranslationUnit type, ITranslationUnit name, ITranslationUnit expression)
        {
            return FieldDeclarationTranslationUnit.Create(visibility, type, name, expression == null ? null : new ITranslationUnit[] { expression });
        }

        /// <summary>
        /// Creates the proper helper.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel"></param>
        /// <returns></returns>
        /// <remarks>
        /// Must return a type deriving from <see cref="FieldDeclaration"/>.
        /// </remarks>
        protected virtual FieldDeclaration CreateHelper(FieldDeclarationSyntax node, SemanticModel semanticModel)
        {
            return new FieldDeclaration(this.Node as FieldDeclarationSyntax, this.SemanticModel);
        }
    }
}