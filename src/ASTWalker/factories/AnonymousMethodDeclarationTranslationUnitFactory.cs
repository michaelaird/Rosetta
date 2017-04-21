/// <summary>
/// MethodDeclarationTranslationUnitFactory.cs
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
    /// Factory for <see cref="MethodDeclarationTranslationUnit"/>.
    /// </summary>
    public class AnonymousMethodDeclarationTranslationUnitFactory : TranslationUnitFactory, ITranslationUnitFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDeclarationTranslationUnitFactory"/> class.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel">The semantic model</param>
        public AnonymousMethodDeclarationTranslationUnitFactory(CSharpSyntaxNode node, SemanticModel semanticModel = null)
            : base(node, semanticModel)
        {
        }

        /// <summary>
        /// Copy initializes a new instance of the <see cref="MethodDeclarationTranslationUnitFactory"/> class.
        /// </summary>
        /// <param name="other"></param>
        /// <remarks>
        /// For testability.
        /// </remarks>
        public AnonymousMethodDeclarationTranslationUnitFactory(MethodDeclarationTranslationUnitFactory other)
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

            AnonymousMethodDeclaration helper = new AnonymousMethodDeclaration(this.Node as AnonymousMethodExpressionSyntax, this.SemanticModel);

            var lambdaDeclaration = LambdaTranslationUnit.Create(null);

            //var lambdaDeclaration = LambdaTranslationUnit.Create(TypeIdentifierTranslationUnit.Create(helper.ReturnType.FullName.MapType()));

            foreach (Parameter parameter in helper.Parameters)
            {
                lambdaDeclaration.AddArgument(ArgumentDefinitionTranslationUnit.Create(
                    TypeIdentifierTranslationUnit.Create(parameter.Type.TypeSyntaxNode.MapType()),
                    IdentifierTranslationUnit.Create(parameter.IdentifierName)));
            }

            return lambdaDeclaration;
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
        /// <remarks>
        /// Must return a type inheriting from <see cref="MethodSignatureDeclarationTranslationUnit"/>.
        /// </remarks>
        /// <param name="visibility"></param>
        /// <param name="returnType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual ITranslationUnit CreateTranslationUnit(
            VisibilityToken visibility, ITranslationUnit returnType, ITranslationUnit name)
        {
            return LambdaTranslationUnit.Create(returnType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel"></param>
        /// <returns></returns>
        /// <remarks>
        /// Must be a type derived from <see cref="MethodDeclaration"/>.
        /// </remarks>
        protected virtual AnonymousMethodDeclaration CreateHelper(AnonymousMethodExpressionSyntax node, SemanticModel semanticModel)
        {
            return new AnonymousMethodDeclaration(node, semanticModel);
        }
    }
}