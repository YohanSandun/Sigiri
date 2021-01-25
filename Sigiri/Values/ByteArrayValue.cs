using System.Collections.Generic;

namespace Sigiri.Values
{
    class ByteArrayValue : Value
    {
        public List<byte> Bytes { get; set; }
        public ByteArrayValue(byte[] array) : base(ValueType.BYTE_ARRAY)
        {
            Bytes = new List<byte>(array);
        }

        public override string ToString()
        {
            string str = "bytes[";
            for (int i = 0; i < Bytes.Count; i++)
            {
                str += "0x"+ System.Convert.ToString(Bytes[i], 16).ToUpper();
                if (i != Bytes.Count - 1)
                    str += ", ";
            }
            return str + "]";
        }

        public override int GetElementCount()
        {
            return Bytes.Count;
        }

        public override RuntimeResult GetElementAt(int index)
        {
            if (index >= 0 && index < Bytes.Count)
                return new RuntimeResult(new IntegerValue(System.Convert.ToInt32(Bytes[index])).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "Index is out of range!", Context));
        }

        public override RuntimeResult In(Value other)
        {
            if (other.Type == ValueType.INTEGER || other.Type == ValueType.INT64) {
                return Bytes.Contains(System.Convert.ToByte(other.Data)) ? new RuntimeResult(new IntegerValue(true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(false).SetPositionAndContext(Position, Context));
            }
            return base.In(other);
        }

        public override RuntimeResult Subscript(Value value)
        {
            if (value.Type == ValueType.INTEGER || value.Type == ValueType.INT64)
            {
                int index = System.Convert.ToInt32(value.Data);
                if (index >= 0 && index < Bytes.Count) {
                    return new RuntimeResult(new IntegerValue(System.Convert.ToInt32(Bytes[index])).SetPositionAndContext(Position, Context));
                } else
                    return new RuntimeResult(new RuntimeError(Position, "Index is out of range!", Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "Index must be an integer!", Context));
        }

        public override RuntimeResult SubscriptAssign(Value index, Value value)
        {
            if ((index.Type == ValueType.INTEGER || index.Type == ValueType.INT64) &&
                (value.Type == ValueType.INTEGER || value.Type == ValueType.INT64))
            {
                int i = System.Convert.ToInt32(index.Data);
                if (i >= 0 && i < Bytes.Count)
                {
                    Bytes[i] = System.Convert.ToByte(value.Data);
                    return new RuntimeResult(this);
                }
                else
                    return new RuntimeResult(new RuntimeError(Position, "Index is out of range!", Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "Index and value must be integers!", Context));
        }
    }
}
