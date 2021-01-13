﻿using System;

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
        public override string ToString()
        {
            return "<class-" + Name + ">";
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

        public override RuntimeResult Add(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult BitwiseAnd(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult BitwiseComplement()
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult BitwiseOr(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult BitwiseXor(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult BooleanAnd(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult BooleanNot()
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult BooleanOr(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult Divide(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult Equals(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult Exponent(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult GreaterOrEqual(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult GreaterThan(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult LeftShift(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult LessOrEqual(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult LessThan(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult Modulus(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult Multiply(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult NotEquals(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult RightShift(Value other)
        {
            throw new NotImplementedException();
        }

        public override RuntimeResult Substract(Value other)
        {
            throw new NotImplementedException();
        }
    }
}
