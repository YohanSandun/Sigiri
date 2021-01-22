using System;

namespace Sigiri.Values
{
    class ComplexValue : Value
    {
        public ComplexValue(double real, double imag) : base(ValueType.COMPLEX)
        {
            this.Data = new System.Numerics.Complex(real, imag);
        }
        public ComplexValue(object complex) : base(ValueType.COMPLEX)
        {
            this.Data = complex;
        }

        public override RuntimeResult Abs()
        {
            return new RuntimeResult(new ComplexValue(System.Numerics.Complex.Abs((System.Numerics.Complex)Data)).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GetAttribute(string name)
        {
            switch (name) {
                case "real":
                    return new RuntimeResult(new FloatValue(((System.Numerics.Complex)Data).Real).SetPositionAndContext(Position, Context));
                case "imag":
                case "imaginary":
                    return new RuntimeResult(new FloatValue(((System.Numerics.Complex)Data).Imaginary).SetPositionAndContext(Position, Context));
                case "phase":
                    return new RuntimeResult(new FloatValue(((System.Numerics.Complex)Data).Phase).SetPositionAndContext(Position, Context));
                case "mag":
                    return new RuntimeResult(new FloatValue(((System.Numerics.Complex)Data).Magnitude).SetPositionAndContext(Position, Context));
                case "conj":
                case "conjugate":
                    return new RuntimeResult(new ComplexValue(System.Numerics.Complex.Conjugate((System.Numerics.Complex)Data)).SetPositionAndContext(Position, Context));
                case "isFinite":
                    return new RuntimeResult(new IntegerValue(System.Numerics.Complex.IsFinite((System.Numerics.Complex)Data)).SetPositionAndContext(Position, Context));
                case "isInf":
                    return new RuntimeResult(new IntegerValue(System.Numerics.Complex.IsInfinity((System.Numerics.Complex)Data)).SetPositionAndContext(Position, Context));
                case "isNan":
                    return new RuntimeResult(new IntegerValue(System.Numerics.Complex.IsNaN((System.Numerics.Complex)Data)).SetPositionAndContext(Position, Context));
            }
            return base.GetAttribute(name);
        }

        public override string ToString()
        {
            System.Numerics.Complex complex = (System.Numerics.Complex)Data;
            if (complex.Real != 0)
                return "(" + complex.Real + "+" + complex.Imaginary + "i)";
            return "(" + complex.Imaginary + "i)";
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
                if (toType == ValueType.COMPLEX)
                    return new ComplexValue(Data).SetPositionAndContext(Position, Context);
            }
            catch { }
            return null;
        }

        //todo add bigint arithmetic
        #region Arithmetic
        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data + (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data + (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.STRING)
                return new RuntimeResult(new StringValue(ToString() + other.Data.ToString()).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data + (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data + (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'+' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Substract(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data - (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data - (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data - (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data - (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'-' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.INTEGER)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data * (int)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data * (long)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data * (double)other.Data).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data * (System.Numerics.Complex)other.Data).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Divide(Value other)
        {
            if (other.Type == ValueType.INTEGER)
            {
                int otherValue = (int)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.INT64)
            {
                long otherValue = (long)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.COMPLEX)
            {
                System.Numerics.Complex otherValue = (System.Numerics.Complex)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            else if (other.Type == ValueType.FLOAT)
            {
                double otherValue = (double)other.Data;
                if (otherValue == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Division by zero", Context));
                return new RuntimeResult(new ComplexValue((System.Numerics.Complex)Data / otherValue).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'/' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Exponent(Value other)
        {
            if (other.Type == ValueType.INT64 || other.Type == ValueType.INTEGER || other.Type == ValueType.FLOAT)
                return new RuntimeResult(new ComplexValue(System.Numerics.Complex.Pow((System.Numerics.Complex)Data, Convert.ToDouble(other.Data))).SetPositionAndContext(Position,Context));
            else if (other.Type == ValueType.COMPLEX)
                return new RuntimeResult(new ComplexValue(System.Numerics.Complex.Pow((System.Numerics.Complex)Data, (System.Numerics.Complex)other.Data)).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'**' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }
        #endregion

        #region Comparison
        public override RuntimeResult Equals(Value other)
        {
            if (other.Type == ValueType.COMPLEX)
                return (System.Numerics.Complex)Data == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.Complex)Data == 0 && 0== (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.Complex)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.Complex)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.Complex)Data == (double)other.Data ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.COMPLEX)
                return (System.Numerics.Complex)Data == (System.Numerics.Complex)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.BIGINTEGER)
                return (System.Numerics.Complex)Data == 0 && 0 == (System.Numerics.BigInteger)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INT64)
                return (System.Numerics.Complex)Data == (long)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.INTEGER)
                return (System.Numerics.Complex)Data == (int)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            else if (other.Type == ValueType.FLOAT)
                return (System.Numerics.Complex)Data == (double)other.Data ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
        }
        #endregion

        #region Boolean
        public override bool GetAsBoolean()
        {
            System.Numerics.Complex complex = (System.Numerics.Complex)Data;
            if (complex.Real == 0 && complex.Imaginary == 0)
                return false;
            return true;
        }

        public override RuntimeResult BooleanNot()
        {
            return GetAsBoolean() ? new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
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
        #endregion

    }
}
