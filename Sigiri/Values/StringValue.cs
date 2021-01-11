using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri.Values
{
    class StringValue : Value
    {
        public StringValue(object data) : base(ValueType.STRING)
        {
            Data = data;
        }
        public override RuntimeResult Subscript(Value value)
        {
            if (value.Type != ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(Position, "Index must be an integer", Context));
            int index = (int)value.Data;
            string str = Data.ToString();
            if (index >= 0 && index < str.Length)
                return new RuntimeResult(new StringValue(str[index]).SetPositionAndContext(Position, Context));
            else if (index < 0 && index >= (str.Length * -1))
                return new RuntimeResult(new StringValue(str[str.Length - 1]).SetPositionAndContext(Position, Context));
            else
                return new RuntimeResult(new RuntimeError(Position, "Index out of range", Context));
        }

        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.STRING || other.Type == ValueType.INTEGER || other.Type == ValueType.FLOAT || other.Type == ValueType.LIST)
                return new RuntimeResult(new StringValue(Data.ToString() + other.ToString()).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "String concadinate is unsupported with " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseAnd(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'and' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseComplement()
        {
            return new RuntimeResult(new RuntimeError(Position, "'~' is unsupported with " + Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseOr(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'or' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseXor(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'^' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BooleanAnd(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'and' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BooleanNot()
        {
            return new RuntimeResult(new RuntimeError(Position, "'not' is unsupported with " + Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BooleanOr(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'or' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Divide(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'/' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Equals(Value other)
        {
            if (other.Type == ValueType.NULL || other.Data == null)
                return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return Data.ToString().Equals(other.Data.ToString()) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult Exponent(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'**' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult GreaterOrEqual(Value other)
        { 
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length >= other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'>=' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult GreaterThan(Value other)
        {
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length > other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'>' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LeftShift(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'<<' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LessOrEqual(Value other)
        {
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length <= other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'<=' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LessThan(Value other)
        {
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length < other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'<' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Modulus(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'%' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.INTEGER) {
                string newStr = "";
                for (int i = 0; i < (int)other.Data; i++)
                {
                    newStr += Data;
                }
                return new RuntimeResult(new StringValue(newStr).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.NULL)
                return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return !Data.ToString().Equals(other.Data.ToString()) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult RightShift(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'>>' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Substract(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
    }
}
