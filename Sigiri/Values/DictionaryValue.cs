using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri.Values
{
    class DictionaryValue : Value
    {
        List<(Value, Value)> Pairs { get; set; }
        public DictionaryValue(List<(Value, Value)> pairs) : base(ValueType.DICTIONARY)
        {
            this.Pairs = pairs;
        }

        public override string ToString()
        {
            string str = "{";
            for (int i = 0; i < Pairs.Count; i++)
            {
                str += Pairs[i].Item1 + ":" + Pairs[i].Item2;
                if (i != Pairs.Count - 1)
                    str += ", ";
            }
            return str + "}";
        }

        public override int GetElementCount()
        {
            return Pairs.Count;
        }

        public override RuntimeResult GetElementAt(int index)
        {
            if (index >= Pairs.Count || index < 0)
                return new RuntimeResult(new RuntimeError(Position, "Index out of range!", Context));
            ListValue listValue = new ListValue(new List<Value>() { Pairs[index].Item1, Pairs[index].Item2 });
            return new RuntimeResult(listValue);
        }

        public override RuntimeResult Subscript(Value value)
        {
            for (int i = 0; i < Pairs.Count; i++)
            {
                if (value.Equals(Pairs[i].Item1).Value.GetAsBoolean())
                    return new RuntimeResult(Pairs[i].Item2);
            }
            return new RuntimeResult(new RuntimeError(Position, "Key " + value + " not found in the dictionary", Context));
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
