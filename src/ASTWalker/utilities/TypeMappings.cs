/// <summary>
/// TypeMappings.cs
/// Andrea Tino - 2016
/// </summary>

namespace Rosetta.AST.Utilities
{
    using System;

    using Rosetta.Translation;
    using Helpers;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.CSharp;
    using System.Linq;

    /// <summary>
    /// Maps system (MsCoreLib) types to TypeScript types.
    /// </summary>
    public static class TypeMappings
    {
        /// <summary>
        /// Maps a type into another.
        /// </summary>
        /// <param name="originalType">The original type.</param>
        /// <returns>The new type, or the original type if no mapping is possible.</returns>
        public static string MapType(this TypeSyntax originalTypeSyntax)
        {
            //  ArrayTypeSyntax
            //  NameSyntax
            //NullableTypeSyntax
            //OmittedTypeArgumentSyntax
            //PointerTypeSyntax
            //PredefinedTypeSyntax
            //RefTypeSyntax
            //TupleTypeSyntax

            string originalType = null;

            switch (originalTypeSyntax.Kind())
            {
                case SyntaxKind.PredefinedType:
                    var predefinedSyntaxNode = originalTypeSyntax as PredefinedTypeSyntax;

                    originalType = predefinedSyntaxNode.Keyword.Text;

                    break;
                case SyntaxKind.GenericName:
                    var genericNameSyntaxNode = originalTypeSyntax as GenericNameSyntax;

                    originalType = genericNameSyntaxNode.Identifier.ValueText;

                    break;
                case SyntaxKind.IdentifierName:
                    var simpleNameSyntaxNode = originalTypeSyntax as SimpleNameSyntax;

                    originalType = simpleNameSyntaxNode.Identifier.ValueText;
                    break;
                case SyntaxKind.ArrayType:
                    var arrayTypeSyntaxNode = originalTypeSyntax as ArrayTypeSyntax;

                    originalType = arrayTypeSyntaxNode.ElementType.MapType();
                    break;
                case SyntaxKind.NullableType:
                    var nullableTypeSyntaxNode = originalTypeSyntax as NullableTypeSyntax;

                    originalType = nullableTypeSyntaxNode.ElementType.MapType();
                    break;
                case SyntaxKind.QualifiedName:
                    var qualifiedNameSyntaxNode = originalTypeSyntax as QualifiedNameSyntax;

                    originalType = qualifiedNameSyntaxNode.Right.MapType();

                    //TODO should we handle the left? or leave it up to TypeScript cleanup?

                    break;
                case SyntaxKind.RefType:
                case SyntaxKind.TupleType:
                default:
                    throw new NotImplementedException();

                    //break;
            }

            if (IsVoid(originalType))
            {
                originalType = Lexems.VoidReturnType;
            }

            if (IsString(originalType))
            {
                originalType = Lexems.StringType;
            }

            if (IsInt(originalType))
            {
                originalType = Lexems.NumberType;
            }

            if (IsDouble(originalType))
            {
                originalType = Lexems.NumberType;
            }

            if (IsFloat(originalType))
            {
                originalType = Lexems.NumberType;
            }

            if (IsBool(originalType))
            {
                originalType = Lexems.BooleanType;
            }

            if (IsKnockoutObservable(originalType))
            {
                originalType = "KnockoutObservable";
            }

            if (IsKnockoutObservableArray(originalType))
            {
                originalType = "KnockoutObservableArray";
            }

            if (IsKnockoutComputedObservable(originalType))
            {
                originalType = "KnockoutComputed";
            }

            if (IsObject(originalType))
            {
                originalType = Lexems.AnyType;
            }

            switch (originalTypeSyntax.Kind())
            {
                case SyntaxKind.GenericName:
                    var genericNameSyntaxNode = originalTypeSyntax as GenericNameSyntax;

                    originalType = originalType +
                        SyntaxUtility.ToAngleBracketEnclosedList(genericNameSyntaxNode.TypeArgumentList.Arguments.Select(unit => unit.MapType()));

                    break;
                case SyntaxKind.ArrayType:
                    var arrayTypeSyntaxNode = originalTypeSyntax as ArrayTypeSyntax;

                    originalType = originalType + "[]";

                    break;
                case SyntaxKind.NullableType:
                    var nullableTypeSyntaxNode = originalTypeSyntax as NullableTypeSyntax;

                    originalType = originalType + "|null";

                    break;
                default:
                    break;
            }

            return originalType;
        }

        private static bool IsVoid(string originalType) => originalType == typeof(void).FullName || originalType.ToLower().Contains("void");

        private static bool IsString(string originalType) => originalType == typeof(string).FullName || originalType.ToLower().Contains("string");

        private static bool IsInt(string originalType) =>
            originalType == typeof(int).FullName ||
            originalType == typeof(System.Int16).FullName ||
            originalType == typeof(System.Int32).FullName ||
            originalType == typeof(System.Int64).FullName ||
            originalType == typeof(System.IntPtr).FullName ||
            originalType == typeof(System.UInt16).FullName ||
            originalType == typeof(System.UInt32).FullName ||
            originalType == typeof(System.UInt64).FullName ||
            originalType == typeof(System.UIntPtr).FullName ||
            originalType.ToLower().Contains("int");

        private static bool IsDouble(string originalType) => originalType == typeof(double).FullName || originalType.ToLower().Contains("double");

        private static bool IsFloat(string originalType) => originalType == typeof(float).FullName || originalType.ToLower().Contains("float");

        private static bool IsBool(string originalType) => originalType == typeof(bool).FullName || originalType.ToLower().Contains("bool");

        private static bool IsObject(string originalType) => originalType == typeof(object).FullName || originalType.ToLower().Contains("object");

        private static bool IsKnockoutObservable(string originalType) => originalType.Equals("Observable");

        private static bool IsKnockoutObservableArray(string originalType) => originalType.Equals("ObservableArray");

        private static bool IsKnockoutComputedObservable(string originalType) => originalType.Equals("DependentObservable");

    }
}
