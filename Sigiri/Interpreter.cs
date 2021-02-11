using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
            else if (node.Type == NodeType.LOAD)
                return VisitLoadNode((LoadNode)node, context);
            else if (node.Type == NodeType.FOR_EACH)
                return VisitForEachNode((ForEachNode)node, context);
            else if (node.Type == NodeType.DICTIONARY)
                return VisitDictionaryNode((DictionaryNode)node, context);
            throw new NotImplementedException();
        }

        private RuntimeResult VisitNumberNode(NumberNode node, Context context) {
            if (node.Token.Type == TokenType.INTEGER)
                return new RuntimeResult(new Values.IntegerValue(Convert.ToInt32(node.Token.Value)).SetPositionAndContext(node.Token.Position, context));
            else if (node.Token.Type == TokenType.LONG)
                return new RuntimeResult(new Values.Int64Value(Convert.ToInt64(node.Token.Value)).SetPositionAndContext(node.Token.Position, context));
            else if (node.Token.Type == TokenType.FLOAT)
                return new RuntimeResult(new Values.FloatValue(Convert.ToDouble(node.Token.Value)).SetPositionAndContext(node.Token.Position, context));
            else if (node.Token.Type == TokenType.BIGINTEGER)
                return new RuntimeResult(new Values.BigInt(System.Numerics.BigInteger.Parse(node.Token.Value.ToString())).SetPositionAndContext(node.Token.Position, context));
            else if (node.Token.Type == TokenType.COMPLEX)
                return new RuntimeResult(new Values.ComplexValue(
                    Convert.ToDouble(node.Token.Value), Convert.ToDouble(node.Token.SecondaryValue)
                    ).SetPositionAndContext(node.Token.Position, context));
            else if (node.Token.Type == TokenType.BYTE_ARRAY)
                return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.UTF8.GetBytes(node.Token.Value.ToString())).SetPositionAndContext(node.Token.Position, context));
            return null; // todo error handling
        }

        private RuntimeResult VisitStringNode(StringNode node, Context context) {
            return new RuntimeResult(new Values.StringValue(node.Token.Value.ToString()).SetPositionAndContext(node.Token.Position, context));
        }

        private RuntimeResult VisitBinaryNode(BinaryNode node, Context context, Context other = null) {

            RuntimeResult leftResult = Visit(node.LeftNode, context);
            if (leftResult.HasError) return leftResult;
            RuntimeResult rightResult = Visit(node.RightNode, other != null ? other : context);
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

        private RuntimeResult VisitVarAssignNode(VarAssignNode node, Context context, Context baseCtx = null) {
            string name = node.Token.Value.ToString();
            if (name == "null" || name == "true" || name == "false" || name == "this" || name == "base")
                return new RuntimeResult(new RuntimeError(node.Token.Position, "'" + name + "' is a reserved keyword", context));
            RuntimeResult runtimeResult = Visit(node.Node, baseCtx != null ? baseCtx : context);

            if (runtimeResult.HasError) return runtimeResult;

            if (node.TypeToken == null)
            {
                if (context.ContainsSymbol(name) && context.GetSymbol(name).TypeDefined) {
                    Values.ValueType toType = context.GetSymbol(name).Type;
                    if (toType != runtimeResult.Value.Type)
                    {
                        Values.Value castedValue = runtimeResult.Value.Cast(toType);
                        if (castedValue == null)
                            return new RuntimeResult(new RuntimeError(node.Token.Position, "Can't convert '" + runtimeResult.Value.Type.ToString().ToLower() + "' to '" + toType.ToString().ToLower() + "'", context));
                        else {
                            context.AddSymbol(name, castedValue);
                            return runtimeResult;
                        }
                    }
                }
                context.AddSymbol(name, runtimeResult.Value);
                return runtimeResult;
            }
            else {
                if (node.TypeToken.CheckKeyword("int"))
                    return TypeDefAssign(Values.ValueType.INTEGER, runtimeResult.Value, name, node.Token.Position, context);
                if (node.TypeToken.CheckKeyword("complex"))
                    return TypeDefAssign(Values.ValueType.COMPLEX, runtimeResult.Value, name, node.Token.Position, context);
                if (node.TypeToken.CheckKeyword("long"))
                    return TypeDefAssign(Values.ValueType.INT64, runtimeResult.Value, name, node.Token.Position, context);
                if (node.TypeToken.CheckKeyword("big"))
                    return TypeDefAssign(Values.ValueType.BIGINTEGER, runtimeResult.Value, name, node.Token.Position, context);

            }
            return new RuntimeResult(new RuntimeError(node.Token.Position, "Type conversion error", context));
        }

        private RuntimeResult TypeDefAssign(Values.ValueType type, Values.Value from, string name, Position position, Context context) {
            if (from.Type == type)
            {
                context.AddSymbol(name, from);
                return new RuntimeResult(from);
            }
            Values.Value castedVal = from.Cast(type);
            castedVal.TypeDefined = true;
            if (castedVal == null)
                return new RuntimeResult(new RuntimeError(position, "Can't convert '" + from.Type.ToString().ToLower() + "' to '" + type.ToString().ToLower() + "'", context));
            context.AddSymbol(name, castedVal);
            return new RuntimeResult(castedVal);
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
            return new RuntimeResult(new RuntimeError(node.Token.Position, "Variable '" + name + "' not defined----", context));
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
                if (context.Return)
                    return bodyResult;
                output = bodyResult.Value;
                if (context.Break)
                {
                    context.Break = false;
                    break;
                }
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
            if (node.Token != null) {
                switch (node.Token.Type) {
                    case TokenType.IDENTIFIER:
                        name = node.Token.Value.ToString();
                        break;
                    case TokenType.PLUS:
                        name = "-add-";
                        break;
                    case TokenType.MINUS:
                        name = "-sub-";
                        break;
                    case TokenType.MULTIPLY:
                        name = "-mul-";
                        break;
                    case TokenType.DIVIDE:
                        name = "-div-";
                        break;
                    case TokenType.MODULUS:
                        name = "-mod-";
                        break;
                    case TokenType.EXPONENT:
                        name = "-exp-";
                        break;
                    case TokenType.STRING:
                        name = "-str-";
                        break;
                    case TokenType.LEFT_SQB:
                        name = "-sst-";
                        break;
                    case TokenType.LESS_THAN:
                        name = "-les-";
                        break;
                    case TokenType.LESS_TOE:
                        name = "-lte-";
                        break;
                    case TokenType.GREATER_THAN:
                        name = "-gre-";
                        break;
                    case TokenType.GREATER_TOE:
                        name = "-gte-";
                        break;
                    case TokenType.EQUALS_EQUALS:
                        name = "-eeq-";
                        break;
                    case TokenType.NOT_EQUALS:
                        name = "-neq-";
                        break;
                    case TokenType.BITWISE_AND:
                        name = "-ban-";
                        break;
                    case TokenType.BITWISE_OR:
                        name = "-bor-";
                        break;
                    case TokenType.BITWISE_XOR:
                        name = "-xor-";
                        break;
                    case TokenType.COMPLEMENT:
                        name = "-com-";
                        break;
                    case TokenType.EQUALS:
                        name = "-sse-";
                        break;
                    case TokenType.LEFT_SHIFT:
                        name = "-lsh-";
                        break;
                    case TokenType.RIGHT_SHIFT:
                        name = "-rsh-";
                        break;
                    case TokenType.BOOLEAN_AND:
                        name = "-and-";
                        break;
                    case TokenType.BOOLEAN_OR:
                        name = "-orr-";
                        break;
                    case TokenType.BOOLEAN_NOT:
                        name = "-not-";
                        break;
                    case TokenType.IN:
                        name = "-inn-";
                        break;
                }
            }
            Values.Value method = new Values.MethodValue(name, node.Parameters, node.Body, node.DefaultValues).SetPositionAndContext(node.Position, context);
            if (node.Token != null)
                context.AddSymbol(name, method);
            return new RuntimeResult(method);
        }

        private RuntimeResult VisitCallNode(CallNode node, Context context, Context fromCtx = null) {
            List<(string, Values.Value)> args = new List<(string, Values.Value)>();
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                RuntimeResult result = Visit(node.Arguments[i].Item2, fromCtx == null ? context : fromCtx);
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
                        //classValue.Type = Values.ValueType.OBJECT;
                        if (classValue.BaseClass != null)
                        {
                            RuntimeResult baseResult = classValue.InitializeBaseClass(context, this);
                            if (baseResult.HasError) return baseResult;

                            Context newContext = new Context(name, baseResult.Value.Context);
                            classValue.SetPositionAndContext(accessNode.Token.Position, newContext);
                            newContext.AddSymbol("this", classValue);
                            newContext.AddSymbol("base", baseResult.Value);
                            RuntimeResult runtimeResult = Visit(classValue.Body, newContext);
                            if (runtimeResult.HasError) return runtimeResult;

                            //Constructor
                            Values.Value ctorValue = newContext.GetSymbol("init");
                            if (ctorValue != null && ctorValue.Type == Values.ValueType.METHOD)
                            {
                                Values.MethodValue initMethod = (Values.MethodValue)ctorValue;
                                RuntimeResult res = initMethod.Execute(args, this);
                                if (res.HasError)
                                    return res;
                            }
                            return new RuntimeResult(classValue);
                        }
                        else {
                            Context newContext = new Context(name, context);
                            classValue.SetPositionAndContext(accessNode.Token.Position, newContext);
                            newContext.AddSymbol("this", classValue);
                            RuntimeResult runtimeResult = Visit(classValue.Body, newContext);
                            if (runtimeResult.HasError) return runtimeResult;

                            //Constructor
                            Values.Value ctorValue = newContext.GetSymbol("init");
                            if (ctorValue != null && ctorValue.Type == Values.ValueType.METHOD)
                            {
                                Values.MethodValue initMethod = (Values.MethodValue)ctorValue;
                                initMethod.Execute(args, this);
                            }
                            return new RuntimeResult(classValue);
                        }
                        
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
            if (node.BaseClassToken != null) {
                string baseName = node.BaseClassToken.Value.ToString();
                Values.Value baseValue = context.GetSymbol(baseName);
                if (baseValue == null)
                    return new RuntimeResult(new RuntimeError(node.BaseClassToken.Position, "Base class '"+baseName+"' not found!", context));
                if (baseValue.Type != Values.ValueType.CLASS)
                    return new RuntimeResult(new RuntimeError(node.BaseClassToken.Position, "Object '" + baseName + "' is not a class!", context));
                Values.Value newCls = new Values.ClassValue(name, node.Body, baseValue, baseName).SetPositionAndContext(node.Token.Position, context);
                context.AddSymbol(name, newCls);
                return new RuntimeResult(newCls);
            }
            Values.Value cls = new Values.ClassValue(name, node.Body, null, "").SetPositionAndContext(node.Token.Position, context);
            context.AddSymbol(name, cls);
            return new RuntimeResult(cls);
        }

        private RuntimeResult VisitAttributeNode(AttributeNode node, Context context) {
            RuntimeResult runtimeResult = Visit(node.BaseNode, context);
            if (runtimeResult.HasError) return runtimeResult;

            if (node.Node.Type == NodeType.VAR_ASSIGN)
                return VisitVarAssignNode((VarAssignNode)node.Node, runtimeResult.Value.Context, context);
            else if (node.Node.Type == NodeType.CALL)
            {
                if (runtimeResult.Value.Type == Values.ValueType.ASSEMBLY)
                {
                    CallNode callNode = (CallNode)node.Node;
                    Values.AssemblyValue asmVal = (Values.AssemblyValue)runtimeResult.Value;
                    string name = ((VarAccessNode)callNode.Node).Token.Value.ToString();
                    List<object> args = new List<object>();
                    for (int i = 0; i < callNode.Arguments.Count; i++)
                    {
                        RuntimeResult result = Visit(callNode.Arguments[i].Item2, context);
                        if (result.HasError) return result;
                        if (result.Value.Type == Values.ValueType.LIST)
                        {
                            object[] array = Util.ListToArray(result.Value);
                            if (array == null)
                                return new RuntimeResult(new RuntimeError(callNode.Position, "Error while converting list to an array", context));
                            args.Add(array);
                        } else if (result.Value.Type == Values.ValueType.BYTE_ARRAY)
                        {
                            byte[] array = ((Values.ByteArrayValue)result.Value).Bytes.ToArray();
                            args.Add(array);
                        }
                        else if (result.Value.IsBoolean)
                            args.Add(result.Value.GetAsBoolean());
                        else
                            args.Add(result.Value.Data);
                    }
                    return asmVal.Invoke(name, args.ToArray());
                }
                else if (Util.isPremitiveType(runtimeResult.Value.Type) || runtimeResult.Value.Type == Values.ValueType.C_SHARP)
                {
                    CallNode callNode = (CallNode)node.Node;
                    List<(string, Values.Value)> args = new List<(string, Values.Value)>();
                    for (int i = 0; i < callNode.Arguments.Count; i++)
                    {
                        RuntimeResult result = Visit(callNode.Arguments[i].Item2, context);
                        if (result.HasError) return result;
                        args.Add((callNode.Arguments[i].Item1, result.Value));
                    }
                    string name = ((VarAccessNode)callNode.Node).Token.Value.ToString();
                    return runtimeResult.Value.CallMethod(name, args);
                }
                return VisitCallNode((CallNode)node.Node, runtimeResult.Value.Context, context);
            }
            else if (node.Node.Type == NodeType.VAR_ACCESS)
            {
                if (runtimeResult.Value.Type == Values.ValueType.ASSEMBLY || runtimeResult.Value.Type == Values.ValueType.C_SHARP)
                {
                    VarAccessNode vaccess = (VarAccessNode)node.Node;
                    return runtimeResult.Value.GetAttribute(vaccess.Token.Value.ToString());
                }
            }
            return Visit(node.Node, runtimeResult.Value.Context);
        }

        private RuntimeResult VisitLoadNode(LoadNode node, Context context) {
            string fname = node.Token.Value.ToString();
            string fileName = "";

            if (File.Exists(Program.FileDirectory + fname + Program.LibraryExt))
                fileName = Program.FileDirectory + fname + Program.LibraryExt;
            else if (File.Exists(Program.FileDirectory + "libs\\" + fname + Program.LibraryExt))
                fileName = Program.FileDirectory + "libs\\" + fname + Program.LibraryExt;
            else if (File.Exists(Program.FileDirectory + fname + ".si"))
                fileName = Program.FileDirectory + fname + ".si";
            else if (File.Exists(Program.FileDirectory + "libs\\" + fname + ".si"))
                fileName = Program.FileDirectory + "libs\\" + fname + ".si";
            else if (File.Exists(Program.BaseDirectory + fname + Program.LibraryExt))
                fileName = Program.BaseDirectory + fname + Program.LibraryExt;
            else if (File.Exists(Program.BaseDirectory + "libs\\" + fname + Program.LibraryExt))
                fileName = Program.BaseDirectory + fname + "libs\\" + Program.LibraryExt;
            else if (File.Exists(Program.BaseDirectory + fname + ".si"))
                fileName = Program.BaseDirectory + fname + ".si";
            else if (File.Exists(Program.BaseDirectory + "libs\\" + fname + ".si"))
                fileName = Program.BaseDirectory + "libs\\" + fname + ".si";

            if (fileName.Equals(""))
                return new RuntimeResult(new RuntimeError(node.Token.Position, "Assembly '" + fname + "' not found!", context));

            if (fileName.EndsWith(Program.LibraryExt))
            {
                if (node.ClassToken != null)
                {
                    Values.AssemblyValue value = new Values.AssemblyValue();
                    value.SetPositionAndContext(node.Token.Position, context);
                    value.LoadAsm("libs\\"+fname+ Program.LibraryExt, fname, node.ClassToken.Value.ToString());
                    context.AddSymbol(node.ClassToken.Value.ToString(), value);
                    return new RuntimeResult(value);
                }
                else {
                    //todo fix this system
                    Assembly asm = Assembly.LoadFile(fileName);
                    Type[] types = asm.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Console.WriteLine(types[i].Name);
                    }
                    for (int i = 0; i < types.Length; i++)
                    {
                        Values.AssemblyValue value = new Values.AssemblyValue();
                        value.Assembly = asm;
                        value.AsmType = types[i];
                        FieldInfo[] fields = types[i].GetFields();
                        for (int j = 0; j < fields.Length; j++)
                        {
                            context.AddSymbol(fields[j].Name, Values.AssemblyValue.ParseValue(fields[j].GetValue(null), node.Token.Position, context));
                        }
                        value.SetPositionAndContext(node.Token.Position, context);
                        context.AddSymbol(types[i].Name, value);
                        if (i == types.Length - 1)
                            return new RuntimeResult(value);
                    }
                }
            }
            else {
                string code = File.ReadAllText(fileName).Replace("\r\n", "\n");
                Tokenizer tokenizer = new Tokenizer(fname, code);
                TokenizerResult tokenizerResult = tokenizer.GenerateTokens();
                if (!tokenizerResult.HasError)
                {
                    Parser parser = new Parser(tokenizerResult.Tokens);
                    ParserResult parserResult = parser.Parse();
                    if (!parserResult.HasError)
                    {
                        Interpreter interpreter = new Interpreter();
                        return interpreter.Visit(parserResult.Node, context);
                    }
                    else
                        return new RuntimeResult(parserResult.Error);
                }
                else
                    return new RuntimeResult(tokenizerResult.Error);
            }
            return new RuntimeResult(new RuntimeError(node.Token.Position, "Library " + fname + " not found!", context));
        }

        private RuntimeResult VisitForEachNode(ForEachNode node, Context context) {
            RuntimeResult iterResult = Visit(node.Iteratable, context);
            if (iterResult.HasError) return iterResult;

            int elementCount = iterResult.Value.GetElementCount();
            string varName = node.Token.Value.ToString();
            for (int i = 0; i < elementCount; i++)
            {
                RuntimeResult runtimeResult = iterResult.Value.GetElementAt(i);
                if (runtimeResult.HasError) return runtimeResult;
                context.AddSymbol(varName, runtimeResult.Value);
                RuntimeResult bodyResult = Visit(node.Body, context);
                if (bodyResult.HasError) return bodyResult;
            }
            return new RuntimeResult(iterResult.Value);
        }

        private RuntimeResult VisitDictionaryNode(DictionaryNode node, Context context) {
            List<(Values.Value, Values.Value)> pairs = new List<(Values.Value, Values.Value)>();
            for (int i = 0; i < node.KeyValuePairs.Count; i++)
            {
                RuntimeResult keyResult = Visit(node.KeyValuePairs[i].Item1, context);
                if (keyResult.HasError) return keyResult;
                RuntimeResult valueResult = Visit(node.KeyValuePairs[i].Item2, context);
                if (valueResult.HasError) return valueResult;
                pairs.Add((keyResult.Value, valueResult.Value));
            }
            return new RuntimeResult(new Values.DictionaryValue(pairs).SetPositionAndContext(node.Position, context));
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
