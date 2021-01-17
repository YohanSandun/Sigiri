using System;
using System.Reflection;

namespace Sigiri.Values
{
    class AssemblyValue : Value
    {
        public Assembly Assembly { get; set; }
        public object Instance { get; set; }
        public Type AsmType { get; set; }
        public override string ToString()
        {
            return "Assembly";
        }
        public RuntimeResult Invoke(string name, object[] args) {
            try {
                MethodInfo methodInfo = AsmType.GetMethod(name);
                if (methodInfo == null)
                    return new RuntimeResult(new RuntimeError(Position, "Method " + name + " not found in assembly", Context));
                object output = methodInfo.Invoke(Instance, args);
                Value value = ParseValue(output, Position, Context);
                if (value != null)
                    return new RuntimeResult(ParseValue(output, Position, Context));
            }
            catch (Exception ex) { return new RuntimeResult(new RuntimeError(Position, "Error while invoking the method '"+name+"' - "+ex.Message+"", Context)); }
            return new RuntimeResult(new RuntimeError(Position, "Error while invoking the method '" + name + "'", Context));
        }

        public static Value ParseValue(object value, Position position, Context context) {
            if (value == null)
                return new NullValue().SetPositionAndContext(position, context);
            string type = value.GetType().Name;
            if (type.Equals("Double") || type.Equals("Single") || type.Equals("Decimal"))
                return new FloatValue(value).SetPositionAndContext(position, context);
            if (type.Equals("Byte") || type.Equals("Short") || type.Equals("Int32") || type.Equals("Int64"))
                return new IntegerValue(value).SetPositionAndContext(position, context);
            if (type.Equals("Char") || type.Equals("String"))
                return new StringValue(value).SetPositionAndContext(position, context);
            if (type.Equals("Byte[]"))
                return ListValue.FromArray((byte[])value).SetPositionAndContext(position,context);
            return null;
        }

        public AssemblyValue() : base(ValueType.ASSEMBLY)
        {

        }
    }
}
