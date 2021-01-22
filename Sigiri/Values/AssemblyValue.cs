using System;
using System.IO;
using System.Reflection;

namespace Sigiri.Values
{
    class AssemblyValue : Value
    {
        public AsmLoadContext AsmLoadContext { get; set; }
        public Assembly Assembly { get; set; }
        public object Instance { get; set; } = null;
        public Type AsmType { get; set; }

        public override string ToString()
        {
            return "Assembly";
        }

        public override RuntimeResult GetAttribute(string name)
        {
            Value value = Context.GetSymbol(name);
            if (value != null)
                return new RuntimeResult(value);
            FieldInfo fieldInfo = AsmType.GetField(name);
            Value parsed = ParseValue(fieldInfo.GetValue(null), Position, Context);
            if (parsed != null)
                return new RuntimeResult(parsed);
            return new RuntimeResult(new RuntimeError(Position, "Error while accessing the attribute '" + name + "'", Context));
        }

        public RuntimeResult Invoke(string name, object[] args) {
            try {
                MethodInfo methodInfo = AsmType.GetMethod(name);
                if (methodInfo == null)
                    return new RuntimeResult(new RuntimeError(Position, "Method " + name + " not found in assembly", Context));
                object output = null;
                
                ParameterInfo[] p = methodInfo.GetParameters();
                if (p.Length == args.Length)
                    output = methodInfo.Invoke(Instance, args);
                else
                {
                    object[] newArgs = new object[p.Length];
                    for (int i = 0; i < p.Length; i++)
                    {
                        if (i < args.Length)
                            newArgs[i] = args[i];
                        else
                            newArgs[i] = p[i].DefaultValue;
                    }
                    output = methodInfo.Invoke(Instance, newArgs);
                }
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
            if (type.Equals("Complex"))
                return new ComplexValue(value).SetPositionAndContext(position, context);
            if (type.Equals("BigInteger"))
                return new BigInt(value).SetPositionAndContext(position, context);
            return null;
        }

        public bool LoadAsm(string path, string name, string typeName) {
            string root = Path.GetFullPath(Path.Combine(
        Path.GetDirectoryName(
            Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, path.Replace('\\', Path.DirectorySeparatorChar)));
            
            AsmLoadContext = new AsmLoadContext(pluginLocation);
            Assembly = AsmLoadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
            if (Assembly != null) {
                AsmType = Assembly.GetType(name + "." + typeName);
                if (AsmType != null)
                {
                    ConstructorInfo constructor = AsmType.GetConstructor(System.Type.EmptyTypes);
                    if (constructor != null)
                        Instance = constructor.Invoke(new object[] { });
                    FieldInfo[] fields = AsmType.GetFields();
                    for (int j = 0; j < fields.Length; j++)
                    {
                        if (fields.IsReadOnly)
                            Context.AddSymbol(fields[j].Name, ParseValue(fields[j].GetValue(null), Position, Context));
                    }
                }
            }
            return !(Assembly == null);
        }

        public AssemblyValue() : base(ValueType.ASSEMBLY)
        {

        }
    }
}
