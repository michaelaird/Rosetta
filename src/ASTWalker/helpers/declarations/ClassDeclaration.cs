﻿/// <summary>
/// ClassDeclaration.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST.Helpers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Helper for accessing class in AST
    /// </summary>
    public class ClassDeclaration : InheritableDeclaration
    {
        // Cached values
        private IEnumerable<BaseTypeReference> interfaces;
        private BaseTypeReference baseClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDeclaration"/> class.
        /// </summary>
        /// <param name="classDeclarationNode"></param>
        public ClassDeclaration(ClassDeclarationSyntax classDeclarationNode) 
            : this(classDeclarationNode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDeclaration"/> class.
        /// </summary>
        /// <param name="classDeclarationNode"></param>
        /// <param name="semanticModel"></param>
        public ClassDeclaration(ClassDeclarationSyntax classDeclarationNode, SemanticModel semanticModel)
            : base(classDeclarationNode, semanticModel)
        {
            this.interfaces = null;
            this.baseClass = null;
        }

        /// <summary>
        /// Gets the base class.
        /// </summary>
        /// <remarks>
        /// Value is cached.
        /// If class has no base class, <code>null</code> is returned.
        /// </remarks>
        public BaseTypeReference BaseClass
        {
            get
            {
                if (this.baseClass == null)
                {
                    foreach (BaseTypeReference baseType in this.BaseTypes)
                    {
                        if (baseType.Kind == TypeKind.Class)
                        {
                            this.baseClass = baseType;
                        }
                    }
                }

                return this.baseClass;
            }
        }

        /// <summary>
        /// Gets the <see cref="Helper"/> though which it is possible to access to attributes.
        /// </summary>
        /// <remarks>
        /// These attributes are the decorations applied in syntax to the class.
        /// </remarks>
        public AttributeLists AttributeLists
        {
            get
            {
                return new AttributeLists(this.SyntaxNode as ClassDeclarationSyntax);
            }
        }

        /// <summary>
        /// Gets the collection of implemented interfaces.
        /// </summary>
        /// <remarks>
        /// Value is cached.
        /// If class implements no interfaces, an empty list is returned.
        /// </remarks>
        public IEnumerable<BaseTypeReference> ImplementedInterfaces
        {
            get
            {
                if (this.interfaces == null)
                {
                    this.interfaces = new List<BaseTypeReference>();
                    foreach (BaseTypeReference baseType in this.BaseTypes)
                    {
                        if (baseType.Kind == TypeKind.Interface)
                        {
                            ((List<BaseTypeReference>)this.interfaces).Add(baseType);
                        }
                    }
                }

                return this.interfaces;
            }
        }

        private ClassDeclarationSyntax ClassDeclarationNode
        {
            get { return (ClassDeclarationSyntax)this.SyntaxNode; }
        }
    }
}
