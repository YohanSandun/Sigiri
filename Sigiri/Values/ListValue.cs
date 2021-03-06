﻿using System.Collections.Generic;

namespace Sigiri.Values
{
    class ListValue : Value
    {
        List<Value> Elements { get; set; }
        public ListValue(List<Value> elemetns) : base(ValueType.LIST)
        {
            this.Elements = elemetns;
        }

        public void AddElement(Value value) {
            Elements.Add(value);
        }

        public override RuntimeResult CallMethod(string name, List<(string, Value)> args)
        {
            switch (name) {
                case "append":
                    for (int i = 0; i < args.Count; i++)
                    {
                        if (args[i].Item2.Type == ValueType.LIST)
                            Elements.Add(((ListValue)args[i].Item2).Clone());
                        else 
                            Elements.Add(args[i].Item2);
                    }
                    return new RuntimeResult(this);
                case "getCount":
                    return new RuntimeResult(new IntegerValue(Elements.Count).SetPositionAndContext(Position, Context));
            }
            return base.CallMethod(name, args);
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

        public Value Clone() {
            List<Value> newVals = new List<Value>(Elements);
            return new ListValue(newVals).SetPositionAndContext(Position, Context);
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

        public static ListValue FromArray(byte[] array) {
            List<Value> values = new List<Value>();
            for (int i = 0; i < array.Length; i++)
            {
                values.Add(new IntegerValue(array[i]));
            }
            return new ListValue(values);
        }

        public static ListValue FromArray(int[] array)
        {
            List<Value> values = new List<Value>();
            for (int i = 0; i < array.Length; i++)
            {
                values.Add(new IntegerValue(array[i]));
            }
            return new ListValue(values);
        }

        public static ListValue FromArray(string[] array)
        {
            List<Value> values = new List<Value>();
            for (int i = 0; i < array.Length; i++)
            {
                values.Add(new StringValue(array[i]));
            }
            return new ListValue(values);
        }

        public ByteArrayValue ToByteArray() {
            try
            {
                byte[] array = new byte[Elements.Count];
                for (int i = 0; i < Elements.Count; i++)
                {
                    array[i] = System.Convert.ToByte(Elements[i].Data);
                }
                return new ByteArrayValue(array);
            }
            catch { return null; }
        }
        public string[] ToStringArray()
        {
            try
            {
                string[] array = new string[Elements.Count];
                for (int i = 0; i < Elements.Count; i++)
                {
                    array[i] = (Elements[i].ToString());
                }
                return array;
            }
            catch { return null; }
        }

        public override int GetElementCount()
        {
            return Elements.Count;
        }

        public override RuntimeResult GetElementAt(int index)
        {
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

        public override RuntimeResult NotEquals(Value other)
        {
            RuntimeResult result = Equals(other);
            if (result.HasError) return result;
            return result.Value.GetAsBoolean() ? new RuntimeResult(new IntegerValue(false).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(true).SetPositionAndContext(Position, Context));
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
