using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri.Values
{
    class FloatValue : Value
    {
        public FloatValue(object data) : base(ValueType.FLOAT)
        {
            Data = data;
        }
        public override RuntimeResult Abs()
        {
            double data = (double)Data;
            return data < 0 ? new RuntimeResult(new IntegerValue(data * -1).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(data).SetPositionAndContext(Position, Context));
        }
        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data + (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data + (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'+' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseAnd(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'&' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseComplement()
        {
            return new RuntimeResult(new RuntimeError(Position, "'~' is unsupported for " + Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseOr(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'|' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
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
            return new RuntimeResult(new RuntimeError(Position, "'not' is unsupported for " + Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BooleanOr(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'or' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Divide(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data / (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data / (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'/' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Equals(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data == (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult Exponent(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue(Math.Pow((double)Data, (double)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue(Math.Pow((double)Data, (int)other.Data)).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'**' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult GreaterOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data >= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data >= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GreaterThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data > (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data > (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LeftShift(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'<<' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LessOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data <= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data <= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LessThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data < (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data < (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult Modulus(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data % (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data % (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'%' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data * (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data * (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data != (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data != (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult RightShift(Value other)
        {
            return new RuntimeResult(new RuntimeError(Position, "'>>' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Substract(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data - (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data - (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
    }
}
