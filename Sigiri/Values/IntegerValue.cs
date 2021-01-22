using System;

namespace Sigiri.Values
{
    class IntegerValue : Value
    {
        private bool asBoolean;
        public override bool IsBoolean => asBoolean;
        public IntegerValue(object data, bool asBoolean = false) : base(ValueType.INTEGER)
        {
            Data = data;
            this.asBoolean = asBoolean;
        }
        public IntegerValue(bool value) : base(ValueType.INTEGER)
        {
            this.asBoolean = true;
            Data = value ? 1 : 0;
        }
        public override Value Cast(ValueType toType)
        {
            try
            {
                if (toType == ValueType.INTEGER)
                    return new IntegerValue(Data).SetPositionAndContext(Position, Context);
                if (toType == ValueType.INT64)
                    return new Int64Value(Convert.ToInt64(Data)).SetPositionAndContext(Position, Context);
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

        public override RuntimeResult Abs()
        {
            int data = (int)Data;
            return data < 0 ? new RuntimeResult(new IntegerValue(data * -1).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(data).SetPositionAndContext(Position, Context));
        }

        public override string ToString()
        {
            if (asBoolean)
                return (int)Data == 0 ? "false" : "true";
            return Data.ToString();
        }
        public override RuntimeResult In(Value other)
        {
            if (other.Type == ValueType.LIST || other.Type == ValueType.STRING)
                return other.ContainsElement(this) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'in' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        #region Arithmetic
        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data + (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value((int)Data + (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((int)Data + (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((int)Data + (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((int)Data + (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.STRING)
                return new RuntimeResult(new StringValue(Data + other.Data.ToString()).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'+' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Divide(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new IntegerValue((int)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new Int64Value((int)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.BIGINTEGER)
            {
                System.Numerics.BigInteger otherValue = (System.Numerics.BigInteger)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((int)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.COMPLEX)
            {
                System.Numerics.Complex otherValue = (System.Numerics.Complex)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new ComplexValue((int)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((int)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'/' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult Exponent(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new FloatValue(Math.Pow((int)Data, (int)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value(Math.Pow((int)Data, (long)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(Util.BigPow((System.Numerics.BigInteger)other.Data, (int)Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue(System.Numerics.Complex.Pow((System.Numerics.Complex)other.Data, Convert.ToDouble(Data))).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
            {
                double exp = (double)other.Data;
                if ((int)Data < 0 && Math.Floor(exp) != exp) {
                    Value complex = Cast(ValueType.COMPLEX);
                    if (complex != null)
                        return complex.Exponent(other);
                }
                return new RuntimeResult(new FloatValue(Math.Pow((int)Data, (double)other.Data)).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'**' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult Modulus(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new IntegerValue((int)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new Int64Value((int)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.BIGINTEGER)
            {
                System.Numerics.BigInteger otherValue = (System.Numerics.BigInteger)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((int)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue((int)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'%' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data * (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((int)Data * (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value((int)Data * (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((int)Data * (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((int)Data * (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult Substract(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data - (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue((int)Data - (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new Int64Value((int)Data - (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((int)Data - (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((int)Data - (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        #endregion

        #region Bitwise
        public override RuntimeResult BitwiseAnd(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data & (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new IntegerValue((int)Data & (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new IntegerValue((int)Data & (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'&' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult BitwiseComplement()
        {
            return new RuntimeResult(new IntegerValue(~(int)Data).SetPositionAndContext(Position, Context));
        }
        public override RuntimeResult BitwiseOr(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data | (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new IntegerValue((long)Data | (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new IntegerValue((int)Data | (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'|' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));

        }
        public override RuntimeResult BitwiseXor(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data ^ (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new IntegerValue((long)Data ^ (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new IntegerValue((int)Data ^ (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'^' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult LeftShift(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data << (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'<<' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override RuntimeResult RightShift(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new IntegerValue((int)Data >> (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'>>' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        #endregion

        #region Boolean
        public override bool GetAsBoolean()
        {
            return (int)Data == 0 ? false : true;
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
                return (int)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (int)Data == (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (int)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (int)Data == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (int)Data == (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)); 
        }

        public override RuntimeResult GreaterOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (int)Data >= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (int)Data >= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (int)Data >= (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (int)Data >= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GreaterThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (int)Data > (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (int)Data > (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (int)Data > (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (int)Data > (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LessOrEqual(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (int)Data <= (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (int)Data <= (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (int)Data <= (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (int)Data <= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LessThan(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (int)Data < (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (int)Data < (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (int)Data < (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (int)Data < (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        
        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return (int)Data != (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (int)Data != (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (int)Data != (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (int)Data != (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (int)Data != (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
        #endregion

    }
}
