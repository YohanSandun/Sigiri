using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Interpreter
    {
        public Interpreter()
        {
            BuiltinMethods.InitializeBuiltinMethods();
        }

        public RuntimeResult Visit(Node node, Context context) {
            if (node.Type == NodeType.NUMBER)
                return VisitNumberNode((NumberNode)node, context);
            else if (node.Type == NodeType.STRING)
                return VisitStringNode((StringNode)node, context);
            else if (node.Type == NodeType.BINARY)
                return VisitBinaryNode((BinaryNode)node, context);
            else if (node.Type == NodeType.UNARY)
                return VisitUnaryNode((UnaryNode)node, context);
            else if (node.Type == NodeType.VAR_ASSIGN)
                return VisitVarAssignNode((VarAssignNode)node, context);
            else if (node.Type == NodeType.VAR_ACCESS)
                return VisitVarAccessNode((VarAccessNode)node, context);
            else if (node.Type == NodeType.LIST)
                return VisitListNode((ListNode)node, context);
            else if (node.Type == NodeType.SUBSCRIPT)
                return VisitSubscriptNode((SubscriptNode)node, context);
            else if (node.Type == NodeType.BLOCK)
                return VisitBlockNode((BlockNode)node, context);
            else if (node.Type == NodeType.IF)
                return VisitIfNode((IfNode)node, context);
            else if (node.Type == NodeType.FOR)
                return VisitForNode((ForNode)node, context);
            else if (node.Type == NodeType.WHILE)
                return VisitWhileNode((WhileNode)node, context);
            else if (node.Type == NodeType.DO)
                return VisitDoNode((DoNode)node, context);
            else if (node.Type == NodeType.SUBSCRIPT_ASSIGN)
                return VisitSubscriptAssignNode((SubscriptAssignNode)node, context);
            else if (node.Type == NodeType.WHEN)
                return VisitWhenNode((WhenNode)node, context);
            else if (node.Type == NodeType.METHOD)
                return VisitMethodNode((MethodNode)node, context);
            else if (node.Type == NodeType.CALL)
                return VisitCallNode((CallNode)node, context);
            else if (node.Type == NodeType.RETURN)
                return VisitReturnNode((ReturnNode)node, context);
            else if (node.Type == NodeType.BREAK)
                return VisitBreakNode((BreakNode)node, context);
            else if (node.Type == NodeType.CONTINUE)
                return VisitContinueNode((ContinueNode)node, context);
            else if (node.Type == NodeType.CLASS)
                return VisitClassNode((ClassNode)node, context);
            else if (node.Type == NodeType.ATTRIBUTE)
                return VisitAttributeNode((AttributeNode)node, context);

            throw new NotImplementedException();
        }

        private RuntimeResult VisitNumberNode(NumberNode node, Context context) {
            if (node.Token.Type == TokenType.INTEGER)
                return new RuntimeResult(new Values.IntegerValue(node.Token.Value).SetPositionAndContext(node.Token.Position, context));
            else if (node.Token.Type == TokenType.FLOAT)
                return new RuntimeResult(new Values.FloatValue(node.Token.Value).SetPositionAndContext(node.Token.Position, context));
            return null; // todo error handling
        }

        private RuntimeResult VisitStringNode(StringNode node, Context context) {
            return new RuntimeResult(new Values.StringValue(node.Token.Value.ToString()).SetPositionAndContext(node.Token.Position, context));
        }

        private RuntimeResult VisitBinaryNode(BinaryNode node, Context context) {
            RuntimeResult leftResult = Visit(node.LeftNode, context);
            if (leftResult.HasError) return leftResult;
            RuntimeResult rightResult = Visit(node.RightNode, context);
            if (rightResult.HasError) return rightResult;

            if (node.OpToken.Type == TokenType.PLUS)
                return leftResult.Value.Add(rightResult.Value);
            else if (node.OpToken.Type == TokenType.MINUS)
                return leftResult.Value.Substract(rightResult.Value);
            else if (node.OpToken.Type == TokenType.MULTIPLY)
                return leftResult.Value.Multiply(rightResult.Value);
            else if (node.OpToken.Type == TokenType.DIVIDE)
                return leftResult.Value.Divide(rightResult.Value);
            else if (node.OpToken.Type == TokenType.MODULUS)
                return leftResult.Value.Modulus(rightResult.Value);
            else if (node.OpToken.Type == TokenType.EXPONENT)
                return leftResult.Value.Exponent(rightResult.Value);

            else if (node.OpToken.Type == TokenType.BITWISE_AND)
                return leftResult.Value.BitwiseAnd(rightResult.Value);
            else if (node.OpToken.Type == TokenType.BITWISE_OR)
                return leftResult.Value.BitwiseOr(rightResult.Value);
            else if (node.OpToken.Type == TokenType.BITWISE_XOR)
                return leftResult.Value.BitwiseXor(rightResult.Value);
            else if (node.OpToken.Type == TokenType.BITWISE_OR)
                return leftResult.Value.BitwiseOr(rightResult.Value);
            else if (node.OpToken.Type == TokenType.BITWISE_XOR)
                return leftResult.Value.BitwiseXor(rightResult.Value);
            else if (node.OpToken.Type == TokenType.LEFT_SHIFT)
                return leftResult.Value.LeftShift(rightResult.Value);
            else if (node.OpToken.Type == TokenType.RIGHT_SHIFT)
                return leftResult.Value.RightShift(rightResult.Value);

            else if (node.OpToken.Type == TokenType.EQUALS_EQUALS)
                return leftResult.Value.Equals(rightResult.Value);
            else if (node.OpToken.Type == TokenType.NOT_EQUALS)
                return leftResult.Value.NotEquals(rightResult.Value);
            else if (node.OpToken.Type == TokenType.GREATER_THAN)
                return leftResult.Value.GreaterThan(rightResult.Value);
            else if (node.OpToken.Type == TokenType.LESS_THAN)
                return leftResult.Value.LessThan(rightResult.Value);
            else if (node.OpToken.Type == TokenType.GREATER_TOE)
                return leftResult.Value.GreaterOrEqual(rightResult.Value);
            else if (node.OpToken.Type == TokenType.LESS_TOE)
                return leftResult.Value.LessOrEqual(rightResult.Value);

            else if (node.OpToken.Type == TokenType.BOOLEAN_AND)
                return leftResult.Value.BooleanAnd(rightResult.Value);
            else if (node.OpToken.Type == TokenType.BOOLEAN_OR)
                return leftResult.Value.BooleanOr(rightResult.Value);
            else if (node.OpToken.Type == TokenType.IN)
                return leftResult.Value.In(rightResult.Value);

            return null; // todo: error handling
        }

        private RuntimeResult VisitUnaryNode(UnaryNode node, Context context) {
            RuntimeResult runtimeResult = Visit(node.Node, context);
            if (runtimeResult.HasError) return runtimeResult;
            if (node.Token.Type == TokenType.MINUS)
                return runtimeResult.Value.Multiply(new Values.IntegerValue(-1).SetPositionAndContext(node.Token.Position, context));
            else if (node.Token.Type == TokenType.BOOLEAN_NOT)
                return runtimeResult.Value.BooleanNot();
            else if (node.Token.Type == TokenType.COMPLEMENT)
                return runtimeResult.Value.BitwiseComplement();
            return null; // todo: error handling
        }

        private RuntimeResult VisitVarAssignNode(VarAssignNode node, Context context) {
            string name = node.Token.Value.ToString();
            if (name == "null" || name == "true" || name == "false")
                return new RuntimeResult(new RuntimeError(node.Token.Position, "'" + name + "' is a reserved keyword", context));
            RuntimeResult runtimeResult = Visit(node.Node, context);
            if (runtimeResult.HasError) return runtimeResult;
            context.AddSymbol(name, runtimeResult.Value);
            return new RuntimeResult(runtimeResult.Value);
        }

        private RuntimeResult VisitVarAccessNode(VarAccessNode node, Context context) {
            string name = node.Token.Value.ToString();
            if (name == "null")
                return new RuntimeResult(new Values.NullValue().SetPositionAndContext(node.Token.Position, context));
            else if (name == "true")
                return new RuntimeResult(new Values.IntegerValue(1, true).SetPositionAndContext(node.Token.Position, context));
            else if (name == "false")
                return new RuntimeResult(new Values.IntegerValue(0, true).SetPositionAndContext(node.Token.Position, context));
            Values.Value value = context.GetSymbol(name);
            if (value != null)
                return new RuntimeResult(value);
            return new RuntimeResult(new RuntimeError(node.Token.Position, "Variable '" + name + "' not defined", context));
        }

        private RuntimeResult VisitListNode(ListNode node, Context context) {
            List<Values.Value> elements = new List<Values.Value>();
            for (int i = 0; i < node.ElementNodes.Count; i++)
            {
                RuntimeResult runtimeResult = Visit(node.ElementNodes[i], context);
                if (runtimeResult.HasError) return runtimeResult;
                elements.Add(runtimeResult.Value);
            }
            return new RuntimeResult(new Values.ListValue(elements).SetPositionAndContext(node.Position, context));
        }

        private RuntimeResult VisitSubscriptNode(SubscriptNode node, Context context) {
            RuntimeResult baseResult = Visit(node.BaseNode, context);
            if (baseResult.HasError) return baseResult;
            RuntimeResult indexResult = Visit(node.IndexNode, context);
            if (indexResult.HasError) return indexResult;
            return baseResult.Value.Subscript(indexResult.Value);
        }

        private RuntimeResult VisitBlockNode(BlockNode node, Context context) {
            Values.Value output = new Values.NullValue().SetPositionAndContext(node.Position, context);
            for (int i = 0; i < node.Statements.Count; i++)
            {
                RuntimeResult runtimeResult = Visit(node.Statements[i], context);
                if (runtimeResult.HasError) return runtimeResult;
                if (context.Return)
                    return runtimeResult;
                if (context.Continue)
                {
                    context.Continue = false;
                    return runtimeResult;
                }
                output = runtimeResult.Value;
            }
            return new RuntimeResult(output);
        }

        private RuntimeResult VisitIfNode(IfNode node, Context context) {
            for (int i = 0; i < node.Cases.Count; i++)
            {
                RuntimeResult condResult = Visit(node.Cases[i].Item1, context);
                if (condResult.HasError) return condResult;
                if (condResult.Value.GetAsBoolean()) 
                    return Visit(node.Cases[i].Item2, context);
            }
            if (node.ElseCase != null) 
                return Visit(node.ElseCase, context);
            return new RuntimeResult(new Values.NullValue().SetPositionAndContext(node.Position, context)); 
        }

        private RuntimeResult VisitForNode(ForNode node, Context context) {
            RuntimeResult startResult = Visit(node.Start, context);
            if (startResult.HasError) return startResult;
            if (startResult.Value.Type  != Values.ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(node.Token.Position, "Start value must be an integer", context));
            int start = (int)startResult.Value.Data;
            RuntimeResult endResult = Visit(node.End, context);
            if (endResult.HasError) return endResult;
            if (endResult.Value.Type != Values.ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(node.Token.Position, "Stop value must be an integer", context));
            int end = (int)endResult.Value.Data;
            int step = 1;
            if (node.Step != null) {
                RuntimeResult stepResult = Visit(node.Step, context);
                if (stepResult.HasError) return stepResult;
                if (endResult.Value.Type != Values.ValueType.INTEGER)
                    return new RuntimeResult(new RuntimeError(node.Token.Position, "Step value must be an integer", context));
                step = (int)stepResult.Value.Data;
            }
            string id = node.Token.Value.ToString();
            context.AddSymbol(id, startResult.Value);
            Values.Value output = new Values.NullValue().SetPositionAndContext(node.Token.Position, context);
            if (step > 0)
            {
                while (start < end)
                {
                    RuntimeResult bodyResult = Visit(node.Body, context);
                    if (bodyResult.HasError) return bodyResult;
                    if (context.Return)
                        return bodyResult;
                    output = bodyResult.Value;
                    if (context.Break)
                    {
                        context.Break = false;
                        break;
                    }
                    start += step;
                    startResult.Value.Data = start;
                }
            }
            else {
                while (start > end)
                {
                    RuntimeResult bodyResult = Visit(node.Body, context);
                    if (bodyResult.HasError) return bodyResult;
                    if (context.Return)
                        return bodyResult;
                    output = bodyResult.Value;
                    if (context.Break)
                    {
                        context.Break = false;
                        break;
                    }
                    start += step;
                    startResult.Value.Data = start;
                }
            }
            return new RuntimeResult(output);
        }

        private RuntimeResult VisitWhileNode(WhileNode node, Context context) {
            RuntimeResult condResult = Visit(node.Condition, context);
            if (condResult.HasError) return condResult;
            Values.Value output = new Values.NullValue().SetPositionAndContext(node.Position, context);
            while (condResult.Value.GetAsBoolean()) {
                RuntimeResult bodyResult = Visit(node.Body, context);
                if (bodyResult.HasError) return bodyResult;
                output = bodyResult.Value;
                condResult = Visit(node.Condition, context);
                if (condResult.HasError) return condResult;
            }
            return new RuntimeResult(output);
        }

        private RuntimeResult VisitDoNode(DoNode node, Context context)
        {
            Values.Value output = new Values.NullValue().SetPositionAndContext(node.Position, context);
            while (true)
            {
                RuntimeResult bodyResult = Visit(node.Body, context);
                if (bodyResult.HasError) return bodyResult;
                output = bodyResult.Value;
                RuntimeResult condResult = Visit(node.Condition, context);
                if (condResult.HasError) return condResult;
                if (!condResult.Value.GetAsBoolean())
                    break;
            }
            return new RuntimeResult(output);
        }

        private RuntimeResult VisitSubscriptAssignNode(SubscriptAssignNode node, Context context) {
            SubscriptNode subscript = (SubscriptNode)node.Target;
            RuntimeResult targetResult = Visit(subscript.BaseNode, context);
            if (targetResult.HasError) return targetResult;
            RuntimeResult indexResult = Visit(subscript.IndexNode, context);
            if (indexResult.HasError) return indexResult;
            RuntimeResult exprResult = Visit(node.Node, context);
            if (exprResult.HasError) return exprResult;
            return targetResult.Value.SubscriptAssign(indexResult.Value, exprResult.Value);
        }

        private RuntimeResult VisitWhenNode(WhenNode node, Context context) {
            Values.Value variable = context.GetSymbol(node.Token.Value.ToString());
            if (variable == null)
                return new RuntimeResult(new RuntimeError(node.Token.Position, "Variable '" + node.Token.Value + "' not defined", context));
            for (int i = 0; i < node.Cases.Count; i++)
            {
                RuntimeResult condResult = Visit(node.Cases[i].Item1, context);
                if (condResult.HasError) return condResult;
                if (condResult.Value.Equals(variable).Value.GetAsBoolean())
                    return Visit(node.Cases[i].Item2, context);
            }
            //if (node.ElseCase != null)
            //    return Visit(node.ElseCase, context);
            return new RuntimeResult(new Values.NullValue().SetPositionAndContext(node.Token.Position, context));
        }

        private RuntimeResult VisitMethodNode(MethodNode node, Context context) {
            string name = "anonymous";
            if (node.Token != null)
                name = node.Token.Value.ToString();
            Values.Value method = new Values.MethodValue(name, node.Parameters, node.Body, node.DefaultValues).SetPositionAndContext(node.Position, context);
            if (node.Token != null)
                context.AddSymbol(name, method);
            return new RuntimeResult(method);
        }

        private RuntimeResult VisitCallNode(CallNode node, Context context) {
            List<(string, Values.Value)> args = new List<(string, Values.Value)>();
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                RuntimeResult result = Visit(node.Arguments[i].Item2, context);
                if (result.HasError) return result;
                args.Add((node.Arguments[i].Item1, result.Value));
            }
            if (node.Node.Type == NodeType.VAR_ACCESS) {
                VarAccessNode accessNode = (VarAccessNode)node.Node;
                string name = accessNode.Token.Value.ToString();
                if (BuiltinMethods.BuiltinMethodList.ContainsKey(name))
                    return BuiltinMethods.Execute(name, args, node.Position, context);
                else {
                    Values.Value method = context.GetSymbol(name);
                    if (method == null)
                        return new RuntimeResult(new RuntimeError(node.Position, "Method '" + name + "' is not defined", context));
                    if (method.Type == Values.ValueType.METHOD)
                        return ((Values.MethodValue)method).Execute(args, this);
                    else if (method.Type == Values.ValueType.CLASS)
                    {
                        Values.ClassValue classValue = ((Values.ClassValue)method).Clone();
                        Context newContext = new Context(name, context);
                        classValue.SetPositionAndContext(accessNode.Token.Position, newContext);
                        newContext.AddSymbol("this", classValue);
                        RuntimeResult runtimeResult = Visit(classValue.Body, newContext);
                        if (runtimeResult.HasError) return runtimeResult;

                        //Constructor
                        Values.Value ctorValue = newContext.GetSymbol("init");
                        if (ctorValue != null && ctorValue.Type == Values.ValueType.METHOD) {
                            Values.MethodValue initMethod = (Values.MethodValue)ctorValue;
                            initMethod.Execute(args, this);
                        }
                        return new RuntimeResult(classValue);
                    }
                    return new RuntimeResult(new RuntimeError(node.Position, "'" + name + "' is not a method", context));
                }
            }
            return new RuntimeResult(new RuntimeError(node.Position, "Can't call as a method", context));
        }

        private RuntimeResult VisitReturnNode(ReturnNode node, Context context) {
            if (node.Node == null)
            {
                Values.Value val = new Values.NullValue().SetPositionAndContext(node.Position, context);
                context.Return = true;
                return new RuntimeResult(val);
            }
            else {
                RuntimeResult result = Visit(node.Node, context);
                if (result.HasError) return result;
                context.Return = true;
                return new RuntimeResult(result.Value);
            }
        }

        private RuntimeResult VisitBreakNode(BreakNode node, Context context) {
            context.Break = true;
            return new RuntimeResult(new Values.NullValue().SetPositionAndContext(node.Position, context));
        }

        private RuntimeResult VisitContinueNode(ContinueNode node, Context context)
        {
            context.Continue = true;
            return new RuntimeResult(new Values.NullValue().SetPositionAndContext(node.Position, context));
        }

        private RuntimeResult VisitClassNode(ClassNode node, Context context) {
            string name = node.Token.Value.ToString();
            Values.Value cls = new Values.ClassValue(name, node.Body).SetPositionAndContext(node.Token.Position, context);
            context.AddSymbol(name, cls);
            return new RuntimeResult(cls);
        }

        private RuntimeResult VisitAttributeNode(AttributeNode node, Context context) {
            RuntimeResult runtimeResult = Visit(node.BaseNode, context);
            if (runtimeResult.HasError) return runtimeResult;
            if (node.Node.Type == NodeType.VAR_ASSIGN) {
                VarAssignNode assignNode = (VarAssignNode)node.Node;
                string name = assignNode.Token.Value.ToString();
                if (name == "null" || name == "true" || name == "false")
                    return new RuntimeResult(new RuntimeError(assignNode.Token.Position, "'" + name + "' is a reserved keyword", context));
                RuntimeResult result = Visit(node.Node, context);
                if (result.HasError) return result;
                runtimeResult.Value.Context.AddSymbol(name, result.Value);
                return result;
            }
            return Visit(node.Node, runtimeResult.Value.Context);
        }
    }

    class RuntimeResult
    {
        public Values.Value Value { get; set; }
        public Error Error { get; set; }
        public bool HasError { get { return Error != null; } }
        public RuntimeResult(Values.Value value)
        {
            this.Value = value;
        }
        public RuntimeResult(Error error)
        {
            this.Error = error;
        }
    }
}
