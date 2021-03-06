﻿/// <summary>
/// ScriptNamespaceAttributeDecoration.cs
/// Andrea Tino - 2016
/// </summary>

namespace Rosetta.ScriptSharp.AST.Helpers
{
    using System;
    using System.Collections.Generic;

    using Rosetta.AST.Helpers;

    /// <summary>
    /// Decorates <see cref="AttributeDecoration"/>.
    /// </summary>
    /// <remarks>
    /// This is a syntax helper which is used to retrieve info on the ScriptNamespace
    /// attribute from a mere syntax point of view.
    /// </remarks>
    public class ScriptNamespaceAttributeDecoration
    {
        public const string ScriptNamespaceFullName = "ScriptNamespace"; // TODO: Find namespace used by ScriptSharp for this class
        public const string ScriptNamespaceName = "ScriptNamespace";

        private AttributeDecoration attribute;

        // Cached quantities
        private string overridenNamespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptNamespaceAttributeDecoration"/> class.
        /// </summary>
        /// <param name="attribute"></param>
        public ScriptNamespaceAttributeDecoration(AttributeDecoration attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            if (!IsScriptNamespaceAttributeDecoration(attribute))
            {
                throw new ArgumentException(nameof(attribute), "Not compatible!");
            }

            this.attribute = attribute;
        }

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        public string Name
        {
            get { return this.attribute.Name; }
        }

        /// <summary>
        /// Gets the name of the overriden namespace.
        /// </summary>
        public string OverridenNamespace
        {
            get
            {
                if (this.overridenNamespace == null)
                {
                    IEnumerable<AttributeArgument> arguments = this.attribute.Arguments;

                    foreach (var argument in arguments)
                    {
                        // For now, we take the first one with a literal expression, but this makes the code very specific to a case
                        // TODO: Make implementation more generic
                        if (argument.LiteralExpression != null)
                        {
                            this.overridenNamespace = argument.LiteralExpression.Literal.ValueText;
                            break;
                        }
                    }
                }

                return this.overridenNamespace;
            }
        }

        /// <summary>
        /// Gets the decorated <see cref="AttributeDecoration"/.>
        /// </summary>
        public AttributeDecoration AttributeDecoration
        {
            get { return this.attribute; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool IsScriptNamespaceAttributeDecoration(AttributeDecoration attribute)
        {
            return attribute.Name == ScriptNamespaceFullName;
        }
    }
}
