using System.Collections.Generic;

namespace Sigiri.Values
{
    enum ValueType
    {
        NULL,
        INTEGER,
        INT64,
        BIGINTEGER,
        COMPLEX,
        FLOAT,
        STRING,
        LIST,
        DICTIONARY,
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
        public bool TypeDefined { get; set; } = false;
        public virtual bool GetAsBoolean() { return false; }
        public virtual bool IsBoolean { get { return false; } }
        public Value(ValueType type) { this.Type = type; }
        public Value SetPositionAndContext(Position position, Context context) {
            this.Position = position;
            this.Context = context;
            return this;
        }
        public virtual Value Cast(ValueType toType) { return null; }
        public virtual RuntimeResult Add(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult Substract(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult Multiply(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult Divide(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult Modulus(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult Exponent(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult BitwiseAnd(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult BitwiseOr(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult BitwiseXor(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult BitwiseComplement() { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult LeftShift(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult RightShift(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult Equals(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult NotEquals(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult LessThan(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult GreaterThan(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult LessOrEqual(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult GreaterOrEqual(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult BooleanAnd(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult BooleanOr(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult BooleanNot() { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator.", Context)); }
        public virtual RuntimeResult CallMethod(string name, List<(string, Value)> args) { return new RuntimeResult(new RuntimeError(Position, "Not found such a method.", Context)); }
        public virtual RuntimeResult GetAttribute(string name) { return new RuntimeResult(new RuntimeError(Position, "Attribute '"+name+"' is not defined!", Context)); }
        public virtual int GetElementCount() { return 0; }
        public virtual RuntimeResult GetElementAt(int index) { return new RuntimeResult(new RuntimeError(Position, "Subscript not supported on this object", Context)); }
        public virtual RuntimeResult Abs() { return new RuntimeResult(new RuntimeError(Position, "abs() is not possible on " + Type.ToString().ToLower() + "", Context)); }
        public virtual RuntimeResult In(Value other) { return new RuntimeResult(new RuntimeError(Position, "Unsupported operator 'in'", Context)); }
        public virtual bool ContainsElement(Value value) { return false; }
        public virtual RuntimeResult Subscript(Value value) { return new RuntimeResult(new RuntimeError(Position, "Subscript not supported on this object", Context)); }
        public virtual RuntimeResult SubscriptAssign(Value index, Value value) { return new RuntimeResult(new RuntimeError(Position, "Subscript not supported on this object", Context)); }
        public override string ToString() { return Data != null ? Data.ToString() : Type.ToString(); }
    }
}
