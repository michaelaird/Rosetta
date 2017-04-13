/// <summary>
/// ExpressionTranslationUnitBuilder.cs
/// Andrea Tino - 2015
/// </summary>

namespace Rosetta.AST.Helpers
{
    using Factories;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Rosetta.Translation;
    using System;
    using System.Reflection;
    using Utilities;

    /// <summary>
    /// Builder responsible for creating the correct <see cref="ITranslationUnit"/>
    /// from an expression syntax node.
    /// This is the main entry point for whatever AST walker which needs to create an expression.
    /// </summary>
    public sealed class ExpressionTranslationUnitBuilder
    {
        private readonly ExpressionSyntax node;
        private readonly SemanticModel semanticModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTranslationUnitBuilder"/> class.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="semanticModel">The semantic model</param>
        public ExpressionTranslationUnitBuilder(ExpressionSyntax node, SemanticModel semanticModel)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node), "A node is needed!");
            }

            this.node = node;
            this.semanticModel = semanticModel;
        }

        /// <summary>
        /// Builds the proper <see cref="ITranslationUnit"/> for the specific expression.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public ITranslationUnit Build()
        {
            switch (this.node.Kind())
            {
                // Binary expressions
                case SyntaxKind.AddExpression:
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.SubtractExpression:
                case SyntaxKind.EqualsExpression:
                case SyntaxKind.NotEqualsExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.LogicalAndExpression:
                case SyntaxKind.LogicalOrExpression:
                    var binaryExpression = this.node as BinaryExpressionSyntax;
                    if (binaryExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected binary expression to binary expression!");
                    }
                    return BuildBinaryExpressionTranslationUnit(binaryExpression, this.semanticModel);

                // Unary expressions
                case SyntaxKind.PostIncrementExpression:
                case SyntaxKind.PreIncrementExpression:
                case SyntaxKind.PostDecrementExpression:
                case SyntaxKind.PreDecrementExpression:
                case SyntaxKind.LogicalNotExpression:
                    var prefixUnaryExpression = this.node as PrefixUnaryExpressionSyntax;
                    var postfixUnaryExpression = this.node as PostfixUnaryExpressionSyntax;
                    if (prefixUnaryExpression == null && postfixUnaryExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected unary expression to unary expression!");
                    }
                    return prefixUnaryExpression != null ?
                        BuildUnaryExpressionTranslationUnit(prefixUnaryExpression, this.semanticModel) :
                        BuildUnaryExpressionTranslationUnit(postfixUnaryExpression, this.semanticModel);

                // Literal expressions
                case SyntaxKind.NumericLiteralExpression:
                case SyntaxKind.StringLiteralExpression:
                case SyntaxKind.NullLiteralExpression:
                case SyntaxKind.CharacterLiteralExpression:
                case SyntaxKind.FalseLiteralExpression:
                case SyntaxKind.TrueLiteralExpression:
                    var literalExpression = this.node as LiteralExpressionSyntax;
                    if (literalExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected literal expression to literal expression!");
                    }
                    return BuildLiteralExpressionTranslationUnit(literalExpression, this.semanticModel);

                //case SyntaxKind.IdentifierName:
                case SyntaxKind.GenericName:
                case SyntaxKind.IdentifierName:
                    var identifierNameExpression = this.node as SimpleNameSyntax;
                    if (identifierNameExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected identifier name expression to identifer name expression!");
                    }
                    return BuildIdentifierNameExpressionTranslationUnit(identifierNameExpression, this.semanticModel);

                case SyntaxKind.InvocationExpression:
                    var invokationExpression = this.node as InvocationExpressionSyntax;
                    if (invokationExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected invokation expression to invokation expression!");
                    }
                    return BuildInvokationExpressionTranslationUnit(invokationExpression, this.semanticModel);

                case SyntaxKind.ObjectCreationExpression:
                    var objectCreationExpression = this.node as ObjectCreationExpressionSyntax;
                    if (objectCreationExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected object creation expression to object creation expression!");
                    }
                    return BuildObjectCreationExpressionTranslationUnit(objectCreationExpression, this.semanticModel);

                // Parenthetical
                case SyntaxKind.ParenthesizedExpression:
                    var parenthesizedExpression = this.node as ParenthesizedExpressionSyntax;
                    if (parenthesizedExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected parenthesized expression to parenthesized expression!");
                    }
                    return BuildParenthesizedExpressionTranslationUnit(parenthesizedExpression, this.semanticModel);

                // Member access expressions
                case SyntaxKind.SimpleMemberAccessExpression:
                    var memberAccessExpression = this.node as MemberAccessExpressionSyntax;
                    if (memberAccessExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected member access expression to member access expression!");
                    }
                    return BuildMemberAccessExpressionTranslationUnit(memberAccessExpression, this.semanticModel);

                // Element access expressions
                case SyntaxKind.ElementAccessExpression:
                    var elementAccessExpression = this.node as ElementAccessExpressionSyntax;
                    if (elementAccessExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected element access expression to element access expression!");
                    }
                    return BuildElementAccessExpressionTranslationUnit(elementAccessExpression, this.semanticModel);

                // Assignment expressions
                case SyntaxKind.AddAssignmentExpression:
                case SyntaxKind.AndAssignmentExpression:
                case SyntaxKind.DivideAssignmentExpression:
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                case SyntaxKind.LeftShiftAssignmentExpression:
                case SyntaxKind.ModuloAssignmentExpression:
                case SyntaxKind.MultiplyAssignmentExpression:
                case SyntaxKind.OrAssignmentExpression:
                case SyntaxKind.RightShiftAssignmentExpression:
                case SyntaxKind.SubtractAssignmentExpression:
                case SyntaxKind.SimpleAssignmentExpression:
                    var assignmentExpression = this.node as AssignmentExpressionSyntax;
                    if (assignmentExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected assignment expression to assignment expression!");
                    }
                    return BuildAssignmentExpressionTranslationUnit(assignmentExpression, this.semanticModel);
                case SyntaxKind.AnonymousMethodExpression:
                    var anonymousMethodExpression = this.node as AnonymousMethodExpressionSyntax;
                    if (anonymousMethodExpression == null)
                    {
                        throw new InvalidCastException("Unable to correctly cast expected anonymous method expression to anonymous method expression!");
                    }
                    return BuildAnonymousMethodExpressionTranslationUnit(anonymousMethodExpression, this.semanticModel);
                default:
                    var defaultExpression = this.node as CSharpSyntaxNode;
                    if (defaultExpression == null)
                    {
                        throw new InvalidCastException("Error while converting the default scenario");
                    }
                    return BuildDefaultExpressionTranslationUnit(defaultExpression, this.semanticModel);
            }

            throw new InvalidOperationException(string.Format("Cannot build an expression for node type {0}!", this.node.Kind()));
        }



        #region Builder methods

        private static ITranslationUnit BuildBinaryExpressionTranslationUnit(BinaryExpressionSyntax expression, SemanticModel semanticModel)
        {
            OperatorToken token = OperatorToken.Undefined;

            switch (expression.Kind())
            {
                case SyntaxKind.AddExpression:
                    token = OperatorToken.Addition;
                    break;

                case SyntaxKind.MultiplyExpression:
                    token = OperatorToken.Multiplication;
                    break;

                case SyntaxKind.DivideExpression:
                    token = OperatorToken.Divide;
                    break;

                case SyntaxKind.SubtractExpression:
                    token = OperatorToken.Subtraction;
                    break;

                case SyntaxKind.EqualsExpression:
                    token = OperatorToken.LogicalEquals;
                    break;

                case SyntaxKind.NotEqualsExpression:
                    token = OperatorToken.NotEquals;
                    break;

                case SyntaxKind.GreaterThanExpression:
                    token = OperatorToken.GreaterThan;
                    break;

                case SyntaxKind.GreaterThanOrEqualExpression:
                    token = OperatorToken.GreaterThanOrEquals;
                    break;

                case SyntaxKind.LessThanExpression:
                    token = OperatorToken.LessThan;
                    break;

                case SyntaxKind.LessThanOrEqualExpression:
                    token = OperatorToken.LessThanOrEquals;
                    break;

                case SyntaxKind.LogicalAndExpression:
                    token = OperatorToken.LogicalAnd;
                    break;

                case SyntaxKind.LogicalOrExpression:
                    token = OperatorToken.LogicalOr;
                    break;
            }

            if (token == OperatorToken.Undefined)
            {
                throw new InvalidOperationException("Binary operator could not be detected!");
            }

            BinaryExpression binaryExpressionHelper = new BinaryExpression(expression, semanticModel);
            ITranslationUnit leftHandOperand = new ExpressionTranslationUnitBuilder(binaryExpressionHelper.LeftHandOperand, semanticModel).Build();
            ITranslationUnit rightHandOperand = new ExpressionTranslationUnitBuilder(binaryExpressionHelper.RightHandOperand, semanticModel).Build();

            return BinaryExpressionTranslationUnit.Create(leftHandOperand, rightHandOperand, token);
        }

        private static ITranslationUnit BuildUnaryExpressionTranslationUnit(PrefixUnaryExpressionSyntax expression, SemanticModel semanticModel)
        {
            OperatorToken token = OperatorToken.Undefined;

            switch (expression.Kind())
            {
                case SyntaxKind.PreIncrementExpression:
                    token = OperatorToken.Increment;
                    break;

                case SyntaxKind.PreDecrementExpression:
                    token = OperatorToken.Decrement;
                    break;

                case SyntaxKind.LogicalNotExpression:
                    token = OperatorToken.LogicalNot;
                    break;
            }

            if (token == OperatorToken.Undefined)
            {
                throw new InvalidOperationException("Unary operator could not be detected!");
            }

            UnaryExpression unaryExpressionHelper = new UnaryExpression(expression, semanticModel);
            ITranslationUnit operand = new ExpressionTranslationUnitBuilder(unaryExpressionHelper.Operand, semanticModel).Build();

            return UnaryExpressionTranslationUnit.Create(operand, token, UnaryExpressionTranslationUnit.UnaryPosition.Prefix);
        }

        private static ITranslationUnit BuildUnaryExpressionTranslationUnit(PostfixUnaryExpressionSyntax expression, SemanticModel semanticModel)
        {
            OperatorToken token = OperatorToken.Undefined;

            switch (expression.Kind())
            {
                case SyntaxKind.PostIncrementExpression:
                    token = OperatorToken.Increment;
                    break;

                case SyntaxKind.PostDecrementExpression:
                    token = OperatorToken.Decrement;
                    break;
            }

            if (token == OperatorToken.Undefined)
            {
                throw new InvalidOperationException("Unary operator could not be detected!");
            }

            UnaryExpression unaryExpressionHelper = new UnaryExpression(expression, semanticModel);
            ITranslationUnit operand = new ExpressionTranslationUnitBuilder(unaryExpressionHelper.Operand, semanticModel).Build();

            return UnaryExpressionTranslationUnit.Create(operand, token, UnaryExpressionTranslationUnit.UnaryPosition.Postfix);
        }

        private static ITranslationUnit BuildLiteralExpressionTranslationUnit(LiteralExpressionSyntax expression, SemanticModel semanticModel)
        {
            SyntaxToken token = expression.Token;

            switch (token.Kind())
            {
                case SyntaxKind.NumericLiteralToken:
                    Type arg_type = token.Value.GetType();

                    Type genericLiteralTranslationUnit = typeof(LiteralTranslationUnit<>).MakeGenericType(arg_type);
                    MethodInfo mi = genericLiteralTranslationUnit.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);

                    return (ITranslationUnit)mi.Invoke(null, new object[] { token.Value });

                    //return LiteralTranslationUnit<token.Value.GetType()>.Create((int)token.Value);
                    //}
                    //else
                    //{
                    //    return LiteralTranslationUnit<decimal>.Create((decimal)token.Value);
                    //}

                case SyntaxKind.StringLiteralToken:
                    return LiteralTranslationUnit<string>.Create((string)token.Value);

                case SyntaxKind.CharacterLiteralToken:
                    return LiteralTranslationUnit<char>.Create((char)token.Value);

                case SyntaxKind.CharacterLiteralExpression:
                    return null;

                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    return LiteralTranslationUnit<bool>.Create((bool)token.Value);

                case SyntaxKind.NullKeyword:
                    return LiteralTranslationUnit.Null;
            }

            throw new InvalidOperationException(string.Format("Cannot build a literal expression for token type {0}!", token.Kind()));
        }

        private static ITranslationUnit BuildParenthesizedExpressionTranslationUnit(ParenthesizedExpressionSyntax expression, SemanticModel semanticModel)
        {
            ParenthesizedExpression parenthesizedExpressionHelper = new ParenthesizedExpression(expression, semanticModel);

            return ParenthesizedExpressionTranslationUnit.Create(
                new ExpressionTranslationUnitBuilder(parenthesizedExpressionHelper.Expression, semanticModel).Build());
        }

        private static ITranslationUnit BuildMemberAccessExpressionTranslationUnit(MemberAccessExpressionSyntax expression, SemanticModel semanticModel)
        {
            var helper = new MemberAccessExpression(expression, semanticModel);

            //translate these Knockout-specific statements
            if ("SetValue".Equals(helper.MemberName)
                || "GetValue".Equals(helper.MemberName)
                || "SetItems".Equals(helper.MemberName)
                || "GetItems".Equals(helper.MemberName))
            {
                return new ExpressionTranslationUnitBuilder(expression.Expression, semanticModel).Build();
            }

            if (expression.Expression is ThisExpressionSyntax)
            {
                return MemberAccessExpressionTranslationUnit.Create(
                    IdentifierTranslationUnit.Create(helper.MemberName),
                    MemberAccessExpressionTranslationUnit.MemberAccessMethod.This);
            }
            else if (expression.Expression is BaseExpressionSyntax)
            {
                return MemberAccessExpressionTranslationUnit.Create(
                    IdentifierTranslationUnit.Create(helper.MemberName),
                    MemberAccessExpressionTranslationUnit.MemberAccessMethod.Base);
            }
            else if (expression.Expression is IdentifierNameSyntax)
            {
                // The target is a simple identifier, the code being analysed is of the form
                // "command.ExecuteReader()" and memberAccess.Expression is the "command"
                // node
                return MemberAccessExpressionTranslationUnit.Create(
                   new ExpressionTranslationUnitBuilder(expression.Expression, semanticModel).Build(),
                    IdentifierTranslationUnit.Create(helper.MemberName));
            }
            else if (expression.Expression is InvocationExpressionSyntax)
            {
                // The target is another invocation, the code being analysed is of the form
                // "GetCommand().ExecuteReader()" and memberAccess.Expression is the
                // "GetCommand()" node
                return MemberAccessExpressionTranslationUnit.Create(
                    new ExpressionTranslationUnitBuilder(expression.Expression, semanticModel).Build(),
                    IdentifierTranslationUnit.Create(helper.MemberName));
            }
            else if (expression.Expression is MemberAccessExpressionSyntax)
            {
                // The target is a member access, the code being analysed is of the form
                // "x.Command.ExecuteReader()" and memberAccess.Expression is the "x.Command"
                // node
                return MemberAccessExpressionTranslationUnit.Create(
                    new ExpressionTranslationUnitBuilder(expression.Expression, semanticModel).Build(),
                    IdentifierTranslationUnit.Create(helper.MemberName));
            }
            else if (expression.Expression is ElementAccessExpressionSyntax)
            {
                // The target is a member access, the code being analysed is of the form
                // "x.Command.ExecuteReader()" and memberAccess.Expression is the "x.Command"
                // node
                return ElementAccessExpressionTranslationUnit.Create(
                    new ExpressionTranslationUnitBuilder(expression.Expression, semanticModel).Build(),
                    IdentifierTranslationUnit.Create(helper.MemberName));
            }

            return MemberAccessExpressionTranslationUnit.Create(
                IdentifierTranslationUnit.Create(helper.MemberName),
                MemberAccessExpressionTranslationUnit.MemberAccessMethod.This);
        }

        private static ITranslationUnit BuildElementAccessExpressionTranslationUnit(ElementAccessExpressionSyntax expression, SemanticModel semanticModel)
        {
            var helper = new ElementAccessExpression(expression, semanticModel);

            if (expression.Expression is IdentifierNameSyntax)
            {
                // The target is a simple identifier, the code being analysed is of the form
                // "command.ExecuteReader()" and memberAccess.Expression is the "command"
                // node
                return ElementAccessExpressionTranslationUnit.Create(
                   new ExpressionTranslationUnitBuilder(expression.Expression, semanticModel).Build(),
                    IdentifierTranslationUnit.Create(helper.Argument));
            }
            else if (expression.Expression is InvocationExpressionSyntax)
            {
                // The target is another invocation, the code being analysed is of the form
                // "GetCommand().ExecuteReader()" and memberAccess.Expression is the
                // "GetCommand()" node
                return ElementAccessExpressionTranslationUnit.Create(
                    new ExpressionTranslationUnitBuilder(expression.Expression, semanticModel).Build(),
                    IdentifierTranslationUnit.Create(helper.Argument));
            }

            return ElementAccessExpressionTranslationUnit.Create(
                IdentifierTranslationUnit.Create(helper.ToString()),
                ElementAccessExpressionTranslationUnit.ElementAccessMethod.This);
        }

        private static ITranslationUnit BuildAssignmentExpressionTranslationUnit(AssignmentExpressionSyntax expression, SemanticModel semanticModel)
        {
            var helper = new AssignmentExpression(expression, semanticModel);

            return AssignmentExpressionTranslationUnit.Create(
                new ExpressionTranslationUnitBuilder(helper.LeftHand, semanticModel).Build(),
                new ExpressionTranslationUnitBuilder(helper.RightHand, semanticModel).Build(),
                helper.Operator);
        }

        private static ITranslationUnit BuildInvokationExpressionTranslationUnit(InvocationExpressionSyntax expression, SemanticModel semanticModel)
        {
            var helper = new InvokationExpression(expression, semanticModel);

            var translationUnit = InvokationExpressionTranslationUnit.Create(
                new ExpressionTranslationUnitBuilder(helper.Expression, semanticModel).Build());

            foreach (var argument in helper.Arguments)
            {
                var argumentTranslationUnit = new ExpressionTranslationUnitBuilder(argument.Expression, semanticModel).Build();

                translationUnit.AddArgument(argumentTranslationUnit);
            }

            return translationUnit;
        }

        private static ITranslationUnit BuildObjectCreationExpressionTranslationUnit(ObjectCreationExpressionSyntax expression, SemanticModel semanticModel)
        {
            var helper = new ObjectCreationExpression(expression, semanticModel);

            var translationUnit = ObjectCreateExpressionTranslationUnit.Create(
                new ExpressionTranslationUnitBuilder(helper.Expression, semanticModel).Build());

            foreach (var argument in helper.Arguments)
            {
                var argumentTranslationUnit = new ExpressionTranslationUnitBuilder(argument.Expression, semanticModel).Build();

                translationUnit.AddArgument(argumentTranslationUnit);
            }

            return translationUnit;
        }

        private static ITranslationUnit BuildIdentifierNameExpressionTranslationUnit(SimpleNameSyntax expression, SemanticModel semanticModel)
        {
            var helper = new IdentifierExpression(expression, semanticModel);

            return IdentifierTranslationUnit.Create(helper.Identifier);
        }


        private ITranslationUnit BuildAnonymousMethodExpressionTranslationUnit(AnonymousMethodExpressionSyntax anonymousMethodExpression, SemanticModel semanticModel)
        {
            var methodWalker = AnonymousMethodASTWalker.Create(node, new ASTWalkerContext(), this.semanticModel);
            var translationUnit = methodWalker.Walk();

            return translationUnit;
        }

        private static ITranslationUnit BuildDefaultExpressionTranslationUnit(CSharpSyntaxNode expression, SemanticModel semanticModel)
        {
            //SyntaxToken token = expression.Token;

            string expressionString = expression.ToString();

            return DefaultTranslationUnit.Create(expressionString);

            //switch (token.Kind())
            //{
            //    case SyntaxKind.NumericLiteralToken:
            //        return LiteralTranslationUnit<int>.Create((int)token.Value);

            //    case SyntaxKind.StringLiteralToken:
            //        return LiteralTranslationUnit<string>.Create((string)token.Value);

            //    case SyntaxKind.CharacterLiteralExpression:
            //        return null;

            //    case SyntaxKind.TrueKeyword:
            //    case SyntaxKind.FalseKeyword:
            //        return LiteralTranslationUnit<bool>.Create((bool)token.Value);

            //    case SyntaxKind.NullKeyword:
            //        return LiteralTranslationUnit.Null;
            //}

            throw new InvalidOperationException(string.Format("Default Expression has failed!"));
        }

        #endregion Builder methods
    }
}