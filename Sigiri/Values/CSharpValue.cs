using System.Collections.Generic;
using System.Reflection;

namespace Sigiri.Values
{
    class CSharpValue : Value
    {
        public CSharpValue(object value) : base(ValueType.C_SHARP)
        {
            Data = value;
        }

        public override string ToString()
        {
            return "(CSharpObject:" + Data.GetType().Name + ")";
        }

        public override RuntimeResult CallMethod(string name, List<(string, Value)> args)
        {
            try
            {
                System.Type t = Data.GetType();
                
                object[] argArray = new object[args.Count];
                for (int i = 0; i < args.Count; i++)
                {
                    argArray[i] = Util.Convert(args[i].Item2);
                }

                object ret = t.InvokeMember(name, System.Reflection.BindingFlags.InvokeMethod |
                                                  System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                                                  null, Data, argArray);

                return new RuntimeResult(AssemblyValue.ParseValue(ret, Position, Context));
            }
            catch {
                return new RuntimeResult(new RuntimeError(Position, "Error while invoking the method: " + name, Context));
            }
        }

        public override RuntimeResult GetAttribute(string name)
        {
            try
            {
                System.Type t = Data.GetType();

                FieldInfo fieldInfo = t.GetField(name);
                if (fieldInfo != null)
                    return new RuntimeResult(AssemblyValue.ParseValue(fieldInfo.GetValue(Data), Position, Context));
                PropertyInfo propertyInfo = t.GetProperty(name);
                if (propertyInfo != null)
                    return new RuntimeResult(AssemblyValue.ParseValue(propertyInfo.GetValue(Data), Position, Context));
                return new RuntimeResult(new RuntimeError(Position, "Error while accessing the attribute '" + name + "'", Context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(Position, "Error while acessing the attribute: " + name, Context));
            }
        }
    }
}
