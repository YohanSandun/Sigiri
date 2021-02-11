using System;

namespace Sigiri.Values
{
    class Int64Value : Value
    {
        public Int64Value(object value) : base(ValueType.INT64)
        {
            this.Data = value;
        }

        public override RuntimeResult In(Value other)
        {
            if (other.Type == ValueType.LIST || other.Type == ValueType.STRING)
                return other.ContainsElement(this) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'in' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Abs()
        {
            long data = (long)Data;
            return data < 0 ? new RuntimeResult(new Int64Value(data * -1).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(data).SetPositionAndContext(Position, Context));
        }

        public override Value Cast(ValueType toType)
        {
            try
            {
                if (toType == ValueType.INTEGER)
                    return new IntegerValue(Convert.ToInt32(Data)).SetPositionAndContext(Position, Context);
                if (toType == ValueType.INT64)
                    return new Int64Value(Data).SetPositionAndContext(Position, Context);
                if (toType == ValueType.BIGINTEGER)
                    return new BigInt(System.Numerics.BigInteger.Parse(Data.ToString())).SetPositionAndContext(Position, Context);
                if (toType == ValueType.FLOAT)
                    return new FloatValue(Convert.ToDouble(Data)).SetPositionAndContext(Position, Context);
                if (toType == ValueType.COMPLEX)
                    return new ComplexValue(Convert.ToDouble(Data), 0).SetPositionAndContext(Position, Context);
            }
            catch { }
            return null;
        }

        #region Arithmetic
        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new Int64Value((long)Data + (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value((long)Data + (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((long)Data + (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((long)Data + (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((long)Data + (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.STRING)
                return new RuntimeResult(new StringValue(Data + other.Data.ToString()).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'+' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Substract(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new Int64Value((long)Data - (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value((long)Data - (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((long)Data - (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((long)Data - (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((long)Data - (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new Int64Value((long)Data * (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value((long)Data * (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((long)Data * (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((long)Data * (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((long)Data * (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Divide(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new Int64Value((long)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new Int64Value((long)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.BIGINTEGER)
            {
                System.Numerics.BigInteger otherValue = (System.Numerics.BigInteger)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((long)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.COMPLEX)
            {
                System.Numerics.Complex otherValue = (System.Numerics.Complex)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new ComplexValue((long)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((long)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'/' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context)); 
        }

        public override RuntimeResult Modulus(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new Int64Value((long)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new Int64Value((long)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.BIGINTEGER)
            {
                System.Numerics.BigInteger otherValue = (System.Numerics.BigInteger)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((long)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((long)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'/' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult Exponent(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new Int64Value(Math.Pow((long)Data, (int)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value(Math.Pow((long)Data, (long)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
            {
                double exp = (double)other.Data;
                if ((long)Data < 0 && Math.Floor(exp) != exp)
                {
                    Value complex = Cast(ValueType.COMPLEX);
                    if (complex != null)
                        return complex.Exponent(other);
                }
                return new RuntimeResult(new FloatValue(Math.Pow((long)Data, (double)other.Data)).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue(System.Numerics.Complex.Pow((System.Numerics.Complex)other.Data, (long)Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(Util.BigPow((System.Numerics.BigInteger)other.Data, (long)Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        #endregion
      
        #region Bitwise
        public override RuntimeResult BitwiseAnd(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((long)Data & (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new IntegerValue((long)Data & (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new IntegerValue((long)Data & (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'&' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult BitwiseComplement()
        {
            return new RuntimeResult(new IntegerValue(~(long)Data).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult BitwiseOr(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((long)Data | (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new IntegerValue((long)Data | (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new IntegerValue((long)Data | (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'|' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult BitwiseXor(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((long)Data ^ (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new IntegerValue((long)Data ^ (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new IntegerValue((long)Data ^ (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'^' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LeftShift(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((long)Data << (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'<<' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult RightShift(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((long)Data >> (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'>>' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        #endregion

        #region Boolean
        public override bool GetAsBoolean()
        {
            return (long)Data == 0 ? false : true;
        }

        public override RuntimeResult BooleanAnd(Value other)
        {
            if (other.Type == ValueType.INTEGER || other.Type == ValueType.FLOAT || other.Type == ValueType.INT64 || other.Type == ValueType.BIGINTEGER || other.Type == ValueType.COMPLEX)
                return GetAsBoolean() && other.GetAsBoolean() ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'and' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult BooleanOr(Value other)
        {
            if (other.Type == ValueType.INTEGER || other.Type == ValueType.FLOAT || other.Type == ValueType.INT64 || other.Type == ValueType.BIGINTEGER || other.Type == ValueType.COMPLEX)
                return GetAsBoolean() || other.GetAsBoolean() ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'or' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult BooleanNot()
        {
            return !GetAsBoolean() ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        #endregion

        #region Comparison

        public override RuntimeResult Equals(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (long)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (long)Data == (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (long)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (long)Data == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (long)Data == (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (long)Data != (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (long)Data != (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (long)Data != (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (long)Data != (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (long)Data != (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        public override RuntimeResult LessThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (long)Data < (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (long)Data < (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (long)Data < (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (long)Data < (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        public override RuntimeResult LessOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (long)Data <= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (long)Data <= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (long)Data <= (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (long)Data <= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        public override RuntimeResult GreaterThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (long)Data > (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (long)Data > (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (long)Data > (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (long)Data > (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        public override RuntimeResult GreaterOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (long)Data >= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (long)Data >= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (long)Data >= (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (long)Data >= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        #endregion
    }
}
