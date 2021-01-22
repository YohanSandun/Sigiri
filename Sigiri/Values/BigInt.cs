using System;

namespace Sigiri.Values
{
    class BigInt : Value
    {
        //todo operations with complex
        public BigInt(object data) : base(ValueType.BIGINTEGER)
        {
            this.Data = data;
        }
        public override RuntimeResult In(Value other)
        {
            if (other.Type == ValueType.LIST || other.Type == ValueType.STRING)
                return other.ContainsElement(this) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'in' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        public override Value Cast(ValueType toType)
        {
            try
            {
                if (toType == ValueType.INTEGER)
                    return new IntegerValue(Convert.ToInt32(Data.ToString())).SetPositionAndContext(Position, Context);
                if (toType == ValueType.INT64)
                    return new Int64Value(Convert.ToInt64(Data.ToString())).SetPositionAndContext(Position, Context);
                if (toType == ValueType.BIGINTEGER)
                    return new BigInt(Data).SetPositionAndContext(Position, Context);
                if (toType == ValueType.FLOAT)
                    return new BigInt(Convert.ToDouble(Data.ToString())).SetPositionAndContext(Position, Context);
            }
            catch { }
            return null;
        }

        public override RuntimeResult Abs()
        {
            return new RuntimeResult(new ComplexValue(System.Numerics.BigInteger.Abs((System.Numerics.BigInteger)Data)).SetPositionAndContext(Position, Context));
        }

        #region Arithmetic
        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data + (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data + (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data + (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue(Convert.ToDouble(Data.ToString()) + (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.STRING)
                return new RuntimeResult(new StringValue(Data + other.Data.ToString()).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'+' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Substract(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data - (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data - (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data - (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue(Convert.ToDouble(Data.ToString()) - (double)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data * (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data * (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data * (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new FloatValue(Convert.ToDouble(Data.ToString()) * (double)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Divide(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.BIGINTEGER)
            {
                System.Numerics.BigInteger otherValue = (System.Numerics.BigInteger)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue(Convert.ToDouble(Data.ToString()) / otherValue).SetPositionAndContext(Position, Context));
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
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.BIGINTEGER)
            {
                System.Numerics.BigInteger otherValue = (System.Numerics.BigInteger)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data % otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new FloatValue(Convert.ToDouble(Data.ToString()) % otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'%' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Exponent(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt(System.Numerics.BigInteger.Pow((System.Numerics.BigInteger)Data, (int)other.Data)).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(Util.BigPow((System.Numerics.BigInteger)Data, (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(Util.BigPow((System.Numerics.BigInteger)Data, (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(Util.BigPow((System.Numerics.BigInteger)Data, (double)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        #endregion

        #region Bitwise

        public override RuntimeResult BitwiseAnd(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data & (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data & (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data & (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'&' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseComplement()
        {
            return new RuntimeResult(new BigInt(~(System.Numerics.BigInteger)Data).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult BitwiseOr(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data | (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data | (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data | (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'|' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult BitwiseXor(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data ^ (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data ^ (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data ^ (System.Numerics.BigInteger)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'^' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LeftShift(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data << (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'<<' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult RightShift(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new BigInt((System.Numerics.BigInteger)Data >> (int)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'>>' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        #endregion

        #region Boolean
        public override bool GetAsBoolean()
        {
            return (System.Numerics.BigInteger)Data == 0 ? false : true;
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
            if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.BigInteger)Data == (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.BigInteger)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.BigInteger)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.BigInteger)Data == 0 && 0 == (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (System.Numerics.BigInteger)Data == 0 && 0 == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.BigInteger)Data == (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.BigInteger)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.BigInteger)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.BigInteger)Data == 0 && 0 == (double)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return (System.Numerics.BigInteger)Data == 0 && 0 == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LessThan(Value other)
        {
            if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.BigInteger)Data < (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.BigInteger)Data < (long)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.BigInteger)Data < (int)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.BigInteger)Data < Convert.ToInt64(other.Data) ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult LessOrEqual(Value other)
        {
            if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.BigInteger)Data <= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.BigInteger)Data <= (long)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.BigInteger)Data <= (int)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.BigInteger)Data <= Convert.ToInt64(other.Data) ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GreaterThan(Value other)
        {
            if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.BigInteger)Data > (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.BigInteger)Data > (long)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.BigInteger)Data > (int)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.BigInteger)Data > Convert.ToInt64(other.Data) ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GreaterOrEqual(Value other)
        {
            if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.BigInteger)Data >= (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.BigInteger)Data >= (long)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.BigInteger)Data >= (int)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.BigInteger)Data >= Convert.ToInt64(other.Data) ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
        }
        #endregion
        
    }
}
