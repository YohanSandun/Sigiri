namespace Sigiri.Values
{
    class NullValue : Value
    {
        public NullValue() : base(ValueType.NULL)
        {

        }
        public override string ToString()
        {
            return "null";
        }

        public override RuntimeResult Equals(Value other)
        {
            if (other.Type == ValueType.NULL)
                return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type != ValueType.NULL)
                return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
    }
}
