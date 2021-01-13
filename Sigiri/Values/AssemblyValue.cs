using System;
using System.Reflection;

namespace Sigiri.Values
{
    class AssemblyValue : Value
    {

        public Assembly Assembly { get; set; }
        public Type AsmType { get; set; }

        public override string ToString()
        {
            return "Assembly";
        }

        public RuntimeResult Invoke(string name, object[] args) {
            try {
                MethodInfo methodInfo = AsmType.GetMethod(name);
                if (methodInfo == null)
                    return new RuntimeResult(new RuntimeError(Position, "Method " + name + " not found in assembly", Context));
                object output = methodInfo.Invoke(null, args);
                if (output == null)
                    return new RuntimeResult(new NullValue().SetPositionAndContext(Position, Context));
                string type = output.GetType().Name;
                Console.WriteLine(type);
                if (type.Equals("Double") || type.Equals("Single") || type.Equals("Decimal"))
                    return new RuntimeResult(new FloatValue(output).SetPositionAndContext(Position, Context));
                if (type.Equals("Byte") || type.Equals("Short") || type.Equals("Int32") || type.Equals("Int64"))
                    return new RuntimeResult(new IntegerValue(output).SetPositionAndContext(Position, Context));
                if (type.Equals("Char") || type.Equals("String"))
                    return new RuntimeResult(new StringValue(output).SetPositionAndContext(Position, Context));
                if (type.Equals("Byte[]"))
                    return new RuntimeResult(ListValue.FromArray((byte[])output).SetPositionAndContext(Position, Context));
            }
            catch (Exception ex) { return new RuntimeResult(new RuntimeError(Position, "Error while invoking the method '"+name+"' - "+ex.Message+"", Context)); }
            return new RuntimeResult(new RuntimeError(Position, "Error while invoking the method '" + name + "'", Context));
        }

        public static Value ParseValue(object value, Position position, Context context) {
            if (value == null)
                return new NullValue().SetPositionAndContext(position, context);
            string type = value.GetType().Name;
            if (type.Equals("Double") || type.Equals("Single") || type.Equals("Decimal"))
                return new FloatValue(value).SetPositionAndContext(position, context);
            if (type.Equals("Byte") || type.Equals("Short") || type.Equals("Int32") || type.Equals("Long"))
                return new IntegerValue(value).SetPositionAndContext(position, context);
            if (type.Equals("Char") || type.Equals("String"))
                return new StringValue(value).SetPositionAndContext(position, context);
            return new NullValue().SetPositionAndContext(position, context);
        }

        public AssemblyValue() : base(ValueType.ASSEMBLY)
        {

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
