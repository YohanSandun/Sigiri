﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri.Values
{
    class StringValue : Value
    {
        public StringValue(object data) : base(ValueType.STRING)
        {
            Data = data;
        }
        public override RuntimeResult Subscript(Value value)
        {
            if (value.Type != ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(Position, "Index must be an integer", Context));
            int index = (int)value.Data;
            string str = Data.ToString();
            if (index >= 0 && index < str.Length)
                return new RuntimeResult(new StringValue(str[index]).SetPositionAndContext(Position, Context));
            else if (index < 0 && index >= (str.Length * -1))
                return new RuntimeResult(new StringValue(str[str.Length - 1]).SetPositionAndContext(Position, Context));
            else
                return new RuntimeResult(new RuntimeError(Position, "Index out of range", Context));
        }

        public override RuntimeResult CallMethod(string name, List<(string, Value)> args)
        {
            switch (name) {
                case "append":
                    for (int i = 0; i < args.Count; i++)
                    {
                        ValueType t = args[i].Item2.Type;
                        if (t == ValueType.STRING || t == ValueType.INTEGER || t == ValueType.FLOAT)
                            Data = Data.ToString() + args[i].Item2.Data.ToString();
                        else
                            return new RuntimeResult(new RuntimeError(Position, "String concadination error", Context));
                    }
                    return new RuntimeResult(this);
                case "getLength":
                    return new RuntimeResult(new IntegerValue(Data.ToString().Length).SetPositionAndContext(Position, Context));
                case "subString":

                    break;
            }
            return base.CallMethod(name, args);
        }

        public override int GetElementCount()
        {
            return Data.ToString().Length;
        }

        public override RuntimeResult In(Value other)
        {
            string val = other.Data.ToString();
            if (val.Contains(Data.ToString()))
                return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GetElementAt(int index)
        {
            string str = Data.ToString();
            if (index >= 0 && index < str.Length)
                return new RuntimeResult(new StringValue(str[index]).SetPositionAndContext(Position, Context));
            else if (index < 0 && index >= (str.Length * -1))
                return new RuntimeResult(new StringValue(str[str.Length - 1]).SetPositionAndContext(Position, Context));
            else
                return new RuntimeResult(new RuntimeError(Position, "Index out of range", Context));
        }

        public override RuntimeResult Add(Value other)
        {
            if (other.Type == ValueType.STRING || other.Type == ValueType.INTEGER || other.Type == ValueType.FLOAT || other.Type == ValueType.LIST)
                return new RuntimeResult(new StringValue(Data.ToString() + other.ToString()).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "String concadinate is unsupported with " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Equals(Value other)
        {
            if (other.Type == ValueType.NULL || other.Data == null)
                return new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return Data.ToString().Equals(other.Data.ToString()) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }

        public override RuntimeResult GreaterOrEqual(Value other)
        { 
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length >= other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'>=' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult GreaterThan(Value other)
        {
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length > other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'>' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LessOrEqual(Value other)
        {
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length <= other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'<=' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult LessThan(Value other)
        {
            if (other.Type == ValueType.STRING)
                return Data.ToString().Length < other.Data.ToString().Length ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
            return new RuntimeResult(new RuntimeError(Position, "'<' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult Multiply(Value other)
        {
            if (other.Type == ValueType.INTEGER) {
                string newStr = "";
                for (int i = 0; i < (int)other.Data; i++)
                {
                    newStr += Data;
                }
                return new RuntimeResult(new StringValue(newStr).SetPositionAndContext(Position, Context));
            }
            return new RuntimeResult(new RuntimeError(Position, "'*' is unsupported between " + Type.ToString().ToLower() + " and " + other.Type.ToString().ToLower(), Context));
        }

        public override RuntimeResult NotEquals(Value other)
        {
            if (other.Type == ValueType.NULL)
                return new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context));
            return !Data.ToString().Equals(other.Data.ToString()) ? new RuntimeResult(new IntegerValue(1, true).SetPositionAndContext(Position, Context)) : new RuntimeResult(new IntegerValue(0, true).SetPositionAndContext(Position, Context));
        }
    }
}
