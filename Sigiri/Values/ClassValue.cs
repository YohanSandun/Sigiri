using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri.Values
{
    class ClassValue : Value
    {
        public Node Body { get; set; }
        public string Name { get; set; }
        public ClassValue(string name, Node body) : base(ValueType.CLASS)
        {
            this.Body = body;
            this.Name = name;
        }
        public override string ToString()
        {
            return "<class-" + Name + ">";
        }

        public ClassValue Clone() {
            return new ClassValue(Name, Body);
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
