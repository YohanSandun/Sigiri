﻿using System;
using System.Collections.Generic;

namespace Sigiri.Values
{
    class ClassValue : Value
    {
        public Node Body { get; set; }
        public string Name { get; set; }
        public Value BaseClass { get; set; } = null;
        public string BaseName { get; set; } = "";
        public ClassValue(string name, Node body, Value baseClass, string baseName) : base(ValueType.CLASS)
        {
            this.Body = body;
            this.Name = name;
            this.BaseClass = baseClass;
            this.BaseName = baseName;
        }

        public ClassValue Clone() {
            return new ClassValue(Name, Body, BaseClass, BaseName);
        }

        public RuntimeResult InitializeBaseClass(Context ctx, Interpreter interpreter) {
            if (BaseClass != null) {
                ClassValue classValue = ((ClassValue)BaseClass).Clone();
                //classValue.Type = ValueType.OBJECT;
                if (classValue.BaseClass != null)
                {
                    RuntimeResult baseResult = classValue.InitializeBaseClass(ctx, interpreter);
                    if (baseResult.HasError) return baseResult;

                    Context newContext = new Context(BaseName, baseResult.Value.Context);
                    classValue.SetPositionAndContext(Position, newContext);
                    newContext.AddSymbol("this", classValue);
                    newContext.AddSymbol("base", baseResult.Value);
                    return interpreter.Visit(classValue.Body, newContext);
                }
                else {
                    Context newContext = new Context(BaseName, ctx);
                    classValue.SetPositionAndContext(Position, newContext);
                    newContext.AddSymbol("this", classValue);
                    return interpreter.Visit(classValue.Body, newContext);
                }
            }
            return new RuntimeResult(new RuntimeError(Position, "Base class '" + BaseName + "' not found!", ctx));
        }

        private RuntimeResult OperatorOverload(string name, Value other) {
            Value method = Context.GetSymbol(name);
            if (method == null)
                return new RuntimeResult(new RuntimeError(Position, "Unsupported operator, operator overloading method not found!", Context));
            MethodValue methodValue = (MethodValue)method;
            if (methodValue.Parameters.Count > 1)
                return new RuntimeResult(new RuntimeError(Position, "Too many parameters for operator overloading method. expected 1 or 0 parameters.", Context));
            List<(string, Value)> args = new List<(string, Value)>();
            args.Add(("", other));
            return methodValue.Execute(args, new Interpreter());
        }
        private RuntimeResult OperatorOverload(string name)
        {
            Value method = Context.GetSymbol(name);
            if (method == null)
                return new RuntimeResult(new RuntimeError(Position, "Unsupported operator, operator overloading method not found!", Context));
            MethodValue methodValue = (MethodValue)method;
            List<(string, Value)> args = new List<(string, Value)>();
            return methodValue.Execute(args, new Interpreter());
        }
        public override string ToString()
        {
            Value method = Context.GetSymbol("-str-");
            if (method == null)
                return "<class-" + Name + ">";
            RuntimeResult result = OperatorOverload("-str-");
            if (result.HasError) return result.Error.ToString();
            return result.Value.ToString();
        }
        public override RuntimeResult Add(Value other)
        {
            return OperatorOverload("-add-", other);
        }
        public override RuntimeResult Substract(Value other)
        {
            return OperatorOverload("-sub-", other);
        }
        public override RuntimeResult Divide(Value other)
        {
            return OperatorOverload("-div-", other);
        }
        public override RuntimeResult Multiply(Value other)
        {
            return OperatorOverload("-mul-", other);
        }
        public override RuntimeResult Exponent(Value other)
        {
            return OperatorOverload("-exp-", other);
        }
        public override RuntimeResult Modulus(Value other)
        {
            return OperatorOverload("-mod-", other);
        }
        public override RuntimeResult LessThan(Value other)
        {
            return OperatorOverload("-les-", other);
        }
        public override RuntimeResult LessOrEqual(Value other)
        {
            return OperatorOverload("-lte-", other);
        }
        public override RuntimeResult GreaterThan(Value other)
        {
            return OperatorOverload("-gre-", other);
        }
        public override RuntimeResult GreaterOrEqual(Value other)
        {
            return OperatorOverload("-gte-", other);
        }
        public override RuntimeResult Equals(Value other)
        {
            return OperatorOverload("-eeq-", other);
        }
        public override RuntimeResult NotEquals(Value other)
        {
            return OperatorOverload("-neq-", other);
        }
        public override RuntimeResult BitwiseAnd(Value other)
        {
            return OperatorOverload("-ban-", other);
        }
        public override RuntimeResult BitwiseOr(Value other)
        {
            return OperatorOverload("-bor-", other);
        }
        public override RuntimeResult BitwiseXor(Value other)
        {
            return OperatorOverload("-xor-", other);
        }
        public override RuntimeResult BitwiseComplement()
        {
            return OperatorOverload("-com-");
        }
        public override RuntimeResult Subscript(Value value)
        {
            return OperatorOverload("-sst-", value);
        }
        public override RuntimeResult SubscriptAssign(Value index, Value value)
        {
            Value method = Context.GetSymbol("-sse-");
            if (method == null)
                return new RuntimeResult(new RuntimeError(Position, "Unsupported operator, operator overloading method not found!", Context));
            MethodValue methodValue = (MethodValue)method;
            if (methodValue.Parameters.Count != 2)
                return new RuntimeResult(new RuntimeError(Position, "Too many parameters for operator overloading method. expected 2 parameters.", Context));
            List<(string, Value)> args = new List<(string, Value)>();
            args.Add(("", index));
            args.Add(("", value));
            return methodValue.Execute(args, new Interpreter());
        }
        public override RuntimeResult BooleanAnd(Value other)
        {
            return OperatorOverload("-and-", other);
        }
        public override RuntimeResult BooleanOr(Value other)
        {
            return OperatorOverload("-orr-", other);
        }
        public override RuntimeResult BooleanNot()
        {
            return OperatorOverload("-not-");
        }
        public override RuntimeResult LeftShift(Value other)
        {
            return OperatorOverload("-lsh-", other);
        }
        public override RuntimeResult RightShift(Value other)
        {
            return OperatorOverload("-rsh-", other);
        }
        public override RuntimeResult In(Value other)
        {
            return OperatorOverload("-inn-", other);
        }
    }
}
