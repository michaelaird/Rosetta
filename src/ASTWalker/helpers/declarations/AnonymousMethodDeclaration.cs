/// <summary>
/// AnonymousMethodDeclaration.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper for accessing methods in AST
    /// </summary>
    public class AnonymousMethodDeclaration : Helper
    {
        // Cached values
        private IEnumerable<Parameter> parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousMethodDeclaration"/> class.
        /// </summary>
        /// <param name="AnonymousMethodDeclarationNode"></param>
        public AnonymousMethodDeclaration(AnonymousMethodExpressionSyntax AnonymousMethodDeclarationNode)
            : this(AnonymousMethodDeclarationNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnonymousMethodDeclaration"/> class.
        /// </summary>
        /// <param name="AnonymousMethodDeclarationNode"></param>
        /// <param name="semanticModel"></param>
        public AnonymousMethodDeclaration(AnonymousMethodExpressionSyntax AnonymousMethodDeclarationNode, SemanticModel semanticModel)
            : base(AnonymousMethodDeclarationNode, semanticModel)
        {
            this.parameters = null;
        }

        /// <summary>
        /// Gets the return type.
        /// </summary>
        /// TODO: Try to use semantic model here?
        //public TypeReference ReturnType => this.CreateTypeReferenceHelper(this.AnonymousMethodDeclarationSyntaxNode.ReturnType, this.SemanticModel);

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        public IEnumerable<Parameter> Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    if (this.AnonymousMethodDeclarationSyntaxNode.ParameterList == null)
                    {
                        this.parameters = new List<Parameter>();
                    }
                    else
                    {
                        this.parameters = this.AnonymousMethodDeclarationSyntaxNode.ParameterList.Parameters.Select(
                            p => this.CreateParameterHelper(p, this.SemanticModel)).ToList();
                    }
                }

                return this.parameters;
            }
        }

        protected AnonymousMethodExpressionSyntax AnonymousMethodDeclarationSyntaxNode
        {
            get { return this.SyntaxNode as AnonymousMethodExpressionSyntax; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel"></param>
        /// <returns></returns>
        protected virtual TypeReference CreateTypeReferenceHelper(TypeSyntax node, SemanticModel semanticModel)
        {
            return new TypeReference(node, semanticModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel"></param>
        /// <returns></returns>
        protected virtual Parameter CreateParameterHelper(ParameterSyntax node, SemanticModel semanticModel)
        {
            return new Parameter(node, semanticModel);
        }
    }
}