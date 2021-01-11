using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri.Values
{
    class ListValue : Value
    {
        List<Value> Elements { get; set; }
        public ListValue(List<Value> elemetns) : base(ValueType.LIST)
        {
            this.Elements = elemetns;
        }

        public override string ToString()
        {
            string str = "[";
            for (int i = 0; i < Elements.Count; i++)
            {
                str += Elements[i].ToString();
                if (i != Elements.Count - 1)
                    str += ", ";
            }
            return str + "]";
        }

        public override RuntimeResult Subscript(Value value)
        {
            if (value.Type != ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(Position, "Index must be an integer", Context));
            int index = (int)value.Data;
            if (index >= 0 && index < Elements.Count)
                return new RuntimeResult(Elements[index]);
            else if (index < 0 && index >= (Elements.Count * -1))
                return new RuntimeResult(Elements[Elements.Count + index]);
            else
                return new RuntimeResult(new RuntimeError(Position, "Index out of range", Context));
        }

        public override RuntimeResult SubscriptAssign(Value index, Value value)
        {
            if (index.Type != ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(Position, "Index must be an integer", Context));
            int indexInt = (int)index.Data;
            if (indexInt >= 0 && indexInt < Elements.Count)
            {
                Elements[indexInt] = value;
                return new RuntimeResult(this);
            }
            else if (indexInt < 0 && indexInt >= (Elements.Count * -1))
            {
                Elements[Elements.Count + indexInt] = value;
                return new RuntimeResult(this);
            }
            else
                return new RuntimeResult(new RuntimeError(Position, "Index out of range", Context));
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
            if (other.Type == ValueType.LIST) {
                ListValue list = (ListValue)other;
                if (list.Elements.Count == Elements.Count) {
                    for (int i = 0; i < Elements.Count; i++)
                    {
                        RuntimeResult compResult = Elements[i].Equals(list.Elements[i]);
                        if (compResult.HasError) return compResult;
                        if (!compResult.Value.GetAsBoolean())
                            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
                    }
                    return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
                }
            }
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
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

        public override bool ContainsElement(Value value)
        {
            foreach (Value item in Elements)
            {
                RuntimeResult result = item.Equals(value);
                if (result.HasError) continue;
                if (result.Value.GetAsBoolean())
                    return true;
            }
            return false;
        }
    }
}
