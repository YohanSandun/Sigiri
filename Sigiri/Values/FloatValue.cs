using System;

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

        public override RuntimeResult In(Value other)
        {
            if (other.Type == ValueType.LIST || other.Type == ValueType.STRING)
                return other.ContainsElement(this) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'in' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        // todo fix arithmetic with bigints
        #region Arithmetic
        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data + (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data + (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new FloatValue((double)Data + (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt(Convert.ToInt64(Data) + (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((double)Data + (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'+' is not supported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Substract(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data - (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data - (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new FloatValue((double)Data - (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt(Convert.ToInt64(Data) - (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((double)Data - (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Divide(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((double)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((double)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.COMPLEX)
            {
                System.Numerics.Complex otherValue = (System.Numerics.Complex)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new ComplexValue((double)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((double)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'/' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Exponent(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue(Math.Pow((double)Data, (double)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue(Math.Pow((double)Data, (int)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new FloatValue(Math.Pow((double)Data, (long)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue(System.Numerics.Complex.Pow((System.Numerics.Complex)Data, (System.Numerics.Complex)other.Data)).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'**' is not supported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Modulus(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((double)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((double)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((double)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'%' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));

        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((double)Data * (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue((double)Data * (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new FloatValue((double)Data * (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt(Convert.ToInt64(Data) * (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((double)Data * (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        #endregion

        #region Boolean
        public override bool GetAsBoolean()
        {
            return (double)Data == 0 ? false : true;
        }
        public override RuntimeResult BooleanAnd(Value other)
        {
            if (other.Type == ValueType.INTEGER || other.Type == ValueType.FLOAT || other.Type == ValueType.INT64 || other.Type == ValueType.BIGINTEGER || other.Type == ValueType.COMPLEX)
                return GetAsBoolean() && other.GetAsBoolean() ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'and' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BooleanNot()
        {
            return !GetAsBoolean() ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult BooleanOr(Value other)
        {
            if (other.Type == ValueType.INTEGER || other.Type == ValueType.FLOAT || other.Type == ValueType.INT64 || other.Type == ValueType.BIGINTEGER || other.Type == ValueType.COMPLEX)
                return GetAsBoolean() || other.GetAsBoolean() ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'or' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        #endregion

        #region Comparison
        public override RuntimeResult Equals(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data == (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (double)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (double)Data == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (double)Data == 0 && 0 == (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        
        public override RuntimeResult GreaterOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data >= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data >= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (double)Data >= (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return Convert.ToInt64(Data) >= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GreaterThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data > (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data > (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (double)Data > (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return Convert.ToInt64(Data) > (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LessOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data <= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data <= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (double)Data <= (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return Convert.ToInt64(Data) <= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LessThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data < (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data < (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (double)Data < (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return Convert.ToInt64(Data) < (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        
        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (double)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (double)Data == (double)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (double)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (double)Data == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (double)Data == 0 && 0 == (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
        }
        #endregion

    }
}
