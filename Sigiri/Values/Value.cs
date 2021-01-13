using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri.Values
{
    enum ValueType
    {
        NULL,
        INTEGER,
        FLOAT,
        STRING,
        LIST,
        METHOD,
        CLASS,
        OBJECT,
        ASSEMBLY
    }

    abstract class Value
    {
        public ValueType Type { get; set; }
        public object Data { get; set; }
        public Position Position { get; set; }
        public Context Context { get; set; }
        public virtual bool GetAsBoolean() {
            return false;
        }
        public Value(ValueType type)
        {
            this.Type = type;
        }
        public Value SetPositionAndContext(Position position, Context context) {
            this.Position = position;
            this.Context = context;
            return this;
        }
        public abstract RuntimeResult Add(Value other);
        public abstract RuntimeResult Substract(Value other);
        public abstract RuntimeResult Multiply(Value other);
        public abstract RuntimeResult Divide(Value other);
        public abstract RuntimeResult Modulus(Value other);
        public abstract RuntimeResult Exponent(Value other);
        public abstract RuntimeResult BitwiseAnd(Value other);
        public abstract RuntimeResult BitwiseOr(Value other);
        public abstract RuntimeResult BitwiseXor(Value other);
        public abstract RuntimeResult BitwiseComplement();
        public abstract RuntimeResult LeftShift(Value other);
        public abstract RuntimeResult RightShift(Value other);
        public abstract RuntimeResult Equals(Value other);
        public abstract RuntimeResult NotEquals(Value other);
        public abstract RuntimeResult LessThan(Value other);
        public abstract RuntimeResult GreaterThan(Value other);
        public abstract RuntimeResult LessOrEqual(Value other);
        public abstract RuntimeResult GreaterOrEqual(Value other);
        public abstract RuntimeResult BooleanAnd(Value other);
        public abstract RuntimeResult BooleanOr(Value other);
        public abstract RuntimeResult BooleanNot();

        public virtual int GetElementCount() {
            return 0;
        }

        public virtual RuntimeResult GetElementAt(int index) {
            return new RuntimeResult(new RuntimeError(Position, "Subscript not supported on this object", Context));
        }

        public virtual RuntimeResult Abs() {
            return new RuntimeResult(new RuntimeError(Position, "abs() is not possible on " + Type.ToString().ToLower() + "", Context));
        }

        public virtual RuntimeResult In(Value other) {
            return new RuntimeResult(new RuntimeError(Position, "Unsupported operator 'in'", Context));
        }

        public virtual bool ContainsElement(Value value) {
            return false;
        }

        public virtual RuntimeResult Subscript(Value value) {
            return new RuntimeResult(new RuntimeError(Position, "Subscript not supported on this object", Context));
        }
        public virtual RuntimeResult SubscriptAssign(Value index, Value value)
        {
            return new RuntimeResult(new RuntimeError(Position, "Subscript not supported on this object", Context));
        }
        public override string ToString() {
            return Data.ToString();
        }
    }
}
