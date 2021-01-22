using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Sigiri
{
    class BuiltinMethods
    {

        public static Dictionary<string, BuiltinMethod> BuiltinMethodList = new Dictionary<string, BuiltinMethod>();
        public static Dictionary<string, BuiltinMethod> StringMethods = new Dictionary<string, BuiltinMethod>();

        static bool initialized = false;

        public static void InitializeBuiltinMethods() {
            if (initialized) return;

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            initialized = true;
            BuiltinMethodList.Add("print", new BuiltinMethod(new List<string>() { "value", "end"},new Dictionary<string, Values.Value>() {
                { "value", new Values.StringValue("")},
                { "end",  new Values.StringValue("\n")},
            }, print));

            BuiltinMethodList.Add("input", new BuiltinMethod(new List<string>() { "prompt" },new Dictionary<string, Values.Value>() {
                { "prompt", new Values.StringValue("")}
            }, input));

            BuiltinMethodList.Add("abs", new BuiltinMethod(new List<string>() { "value" }, new Dictionary<string, Values.Value>() {
                { "value", null}
            }, abs));

            BuiltinMethodList.Add("char", new BuiltinMethod(new List<string>() { "value" }, new Dictionary<string, Values.Value>() {
                { "value", null}
            }, chr));

            BuiltinMethodList.Add("toInt", new BuiltinMethod(new List<string>() { "value", "fromBase" }, new Dictionary<string, Values.Value>() {
                { "value", null},
                { "fromBase", new Values.IntegerValue(10)}
            }, toInt));

            BuiltinMethodList.Add("toFloat", new BuiltinMethod(new List<string>() { "value" }, new Dictionary<string, Values.Value>() {
                { "value", null}
            }, toFloat));

            BuiltinMethodList.Add("toStr", new BuiltinMethod(new List<string>() { "value" }, new Dictionary<string, Values.Value>() {
                { "value", null}
            }, toStr));

            BuiltinMethodList.Add("split", new BuiltinMethod(new List<string>() { "text", "separator" }, new Dictionary<string, Values.Value>() {
                { "text", null},
                { "separator", null}
            }, split));

            BuiltinMethodList.Add("len", new BuiltinMethod(new List<string>() { "value" }, new Dictionary<string, Values.Value>() {
                { "value", null}
            }, len));

            BuiltinMethodList.Add("typeof", new BuiltinMethod(new List<string>() { "value" }, new Dictionary<string, Values.Value>() {
                { "value", null}
            }, _typeof));

            BuiltinMethodList.Add("complex", new BuiltinMethod(new List<string>() { "real", "imag" }, new Dictionary<string, Values.Value>() {
                { "real", new Values.IntegerValue(0)},
                { "imag", new Values.IntegerValue(0)}
            }, complex));


            /* Methods belongs to string objects (not avaiable in public) */
            StringMethods.Add("subString", new BuiltinMethod(new List<string>() { "startIndex", "length" }, new Dictionary<string, Values.Value>() {
                { "startIndex", new Values.IntegerValue(0) },
                { "length", new Values.NullValue() }
            }, str_substr));

        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string[] locations = { Program.BaseDirectory };
            string asmName = new AssemblyName(args.Name).Name + ".dll";
            for (int i = 0; i < locations.Length; i++)
            {
                //Console.Write("searching " + i + " : " + asmName);
                string dir = locations[i];
                IEnumerable<string> files = Directory.EnumerateFiles(dir, "*.dll", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (string.Compare(Path.GetFileName(file), asmName, true) == 0)
                    {
                        //Console.WriteLine(" - found " + Path.GetFileName(file));
                        return Assembly.LoadFile(file);
                    }
                }
                //Console.WriteLine(" - notFound ");
            }
            return null;
        }

        public static RuntimeResult Execute(string name, List<(string, Values.Value)> args, Position position, Context context) 
        {
            if (!BuiltinMethodList.ContainsKey(name))
                return new RuntimeResult(new RuntimeError(position, "Builtin method '" + name + "' not found!", context));
            Context newCtx = new Context(name, context);
            Dictionary<string, Values.Value> values = new Dictionary<string, Values.Value>(BuiltinMethodList[name].DefaultValues);
            List<string> Parameters = BuiltinMethodList[name].Parameters;
            for (int i = 0; i < args.Count; i++)
            {
                if (values.ContainsKey(args[i].Item1))
                {
                    values[args[i].Item1] = args[i].Item2;
                }
                else if (args[i].Item1.Equals(""))
                {
                    values[Parameters[i]] = args[i].Item2;
                }
                else
                    return new RuntimeResult(new RuntimeError(position, "Unknown parameter name '" + args[i].Item1 + "'", context));
            }
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (values[Parameters[i]] == null)
                    return new RuntimeResult(new RuntimeError(position, "Arguments mismatch for '" + name + "'", context));
                newCtx.AddSymbol(Parameters[i], values[Parameters[i]]);
            }
            return BuiltinMethodList[name].ExecutionMethod(position, newCtx);
        }

        public static RuntimeResult ExecStringMethod(string name, Values.Value baseVal, List<(string, Values.Value)> args, Position position, Context context)
        {
            Context newCtx = new Context(name, context);
            Dictionary<string, Values.Value> values = new Dictionary<string, Values.Value>(StringMethods[name].DefaultValues);
            List<string> Parameters = StringMethods[name].Parameters;
            for (int i = 0; i < args.Count; i++)
            {
                if (values.ContainsKey(args[i].Item1))
                {
                    values[args[i].Item1] = args[i].Item2;
                }
                else if (args[i].Item1.Equals(""))
                {
                    values[Parameters[i]] = args[i].Item2;
                }
                else
                    return new RuntimeResult(new RuntimeError(position, "Unknown parameter name '" + args[i].Item1 + "'", context));
            }
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (values[Parameters[i]] == null)
                    return new RuntimeResult(new RuntimeError(position, "Arguments mismatch for '" + name + "'", context));
                newCtx.AddSymbol(Parameters[i], values[Parameters[i]]);
            }
            return StringMethods[name].ExecutionMethod2(baseVal, position, newCtx);
        }

        static RuntimeResult print(Position position, Context context)
        {
            string value = context.GetSymbol("value").ToString();
            string end = context.GetSymbol("end").ToString();
            Console.Write(value + end);
            return new RuntimeResult(new Values.StringValue(value + end));
        }

        static RuntimeResult input(Position position, Context context)
        {
            string prompt = context.GetSymbol("prompt").ToString();
            Console.Write(prompt);
            string input = Console.ReadLine();
            return new RuntimeResult(new Values.StringValue(input).SetPositionAndContext(position, context));
        }

        static RuntimeResult abs(Position position, Context context) {
            Values.Value value = context.GetSymbol("value");
            return value.Abs();
        }

        static RuntimeResult chr(Position position, Context context)
        {
            Values.Value value = context.GetSymbol("value");
            if (value.Type != Values.ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(position, "Can't convert " + value.Type.ToString().ToLower() + " to char", context));
            return new RuntimeResult(new Values.StringValue(Convert.ToChar(value.Data).ToString()).SetPositionAndContext(position, context));
        }

        static RuntimeResult toInt(Position position, Context context)
        {
            Values.Value value = context.GetSymbol("value");
            Values.Value baseVal = context.GetSymbol("fromBase");
            if (baseVal.Type != Values.ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(position, "Base value should be an integer", context));
            if (value.Type == Values.ValueType.FLOAT)
                return new RuntimeResult(new Values.IntegerValue(Convert.ToInt32((double)value.Data)).SetPositionAndContext(position, context));
            try
            {
                int val = Convert.ToInt32(value.Data.ToString(), (int)baseVal.Data);
                return new RuntimeResult(new Values.IntegerValue(val).SetPositionAndContext(position, context));
            }
            catch {
                return new RuntimeResult(new RuntimeError(position, "Input string cannot converted into an integer", context));
            }
        }

        static RuntimeResult toFloat(Position position, Context context)
        {
            Values.Value value = context.GetSymbol("value");
            try
            {
                double val = Convert.ToDouble(value.Data);
                return new RuntimeResult(new Values.FloatValue(val).SetPositionAndContext(position, context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(position, "Input string cannot converted into an integer", context));
            }
        }

        static RuntimeResult toStr(Position position, Context context)
        {
            Values.Value value = context.GetSymbol("value");
            return new RuntimeResult(new Values.StringValue(value.Data.ToString()).SetPositionAndContext(position, context));
        }

        static RuntimeResult split(Position position, Context context)
        {
            Values.Value text = context.GetSymbol("text");
            Values.Value seperator = context.GetSymbol("separator");
            List<Values.Value> elements = new List<Values.Value>();
            string[] array = text.Data.ToString().Split(seperator.Data.ToString());
            for (int i = 0; i < array.Length; i++)
                elements.Add(new Values.StringValue(array[i]).SetPositionAndContext(position, context));
            return new RuntimeResult(new Values.ListValue(elements).SetPositionAndContext(position,context));
        }

        static RuntimeResult len(Position position, Context context)
        {
            Values.Value value = context.GetSymbol("value");
            return new RuntimeResult(new Values.IntegerValue(value.GetElementCount()).SetPositionAndContext(position, context));
        }

        static RuntimeResult str_substr(Values.Value value, Position position, Context context) {
            Values.Value start = context.GetSymbol("startIndex");
            Values.Value length = context.GetSymbol("length");
            if (start.Type != Values.ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(position, "startIndex must be an integer. provided " + start.Type.ToString().ToLower(), context));
            if (length.Type != Values.ValueType.NULL && length.Type != Values.ValueType.INTEGER)
                return new RuntimeResult(new RuntimeError(position, "length must be an integer or null. provided " + start.Type.ToString().ToLower(), context));
            string str = value.Data.ToString();
            int i_start = Convert.ToInt32(start.Data);
            int i_len = str.Length - i_start;
            if (length.Type == Values.ValueType.INTEGER)
                i_len = Convert.ToInt32(length.Data);
            try
            {
                return new RuntimeResult(new Values.StringValue(str.Substring(i_start, i_len)).SetPositionAndContext(position, context));
            }
            catch { }
            return new RuntimeResult(new RuntimeError(position, "Index out of range.", context));
        }

        static RuntimeResult _typeof(Position position, Context context)
        {
            Values.Value value = context.GetSymbol("value");
            return new RuntimeResult(new Values.StringValue(value.Type.ToString().ToLower()).SetPositionAndContext(position, context));
        }

        static RuntimeResult complex(Position position, Context context)
        {
            Values.Value real = context.GetSymbol("real");
            Values.Value imag = context.GetSymbol("imag");
            if (real.Type == Values.ValueType.INTEGER || real.Type == Values.ValueType.FLOAT || real.Type == Values.ValueType.INT64)
            {
                if (imag.Type == Values.ValueType.INTEGER || imag.Type == Values.ValueType.FLOAT || imag.Type == Values.ValueType.INT64)
                {
                    return new RuntimeResult(new Values.ComplexValue(Convert.ToDouble(real.Data), Convert.ToDouble(imag.Data)).SetPositionAndContext(position, context));
                }
                else
                    return new RuntimeResult(new RuntimeError(position, "Imaginary part should be a float or integer", context));
            }
            else
                return new RuntimeResult(new RuntimeError(position, "Real part should be a float or integer", context));
        }

    }

    class BuiltinMethod { 
        public List<string> Parameters { get; set; }
        public Dictionary<string, Values.Value> DefaultValues { get; set; }
        public Func<Position,Context,RuntimeResult> ExecutionMethod { get; set; }
        public Func<Values.Value, Position, Context, RuntimeResult> ExecutionMethod2 { get; set; }
        public BuiltinMethod(List<string> parameters, Dictionary<string, Values.Value> defValues, Func<Position, Context, RuntimeResult> execMethod)
        {
            this.DefaultValues = defValues;
            this.Parameters = parameters;
            this.ExecutionMethod = execMethod;
        }
        public BuiltinMethod(List<string> parameters, Dictionary<string, Values.Value> defValues, Func<Values.Value, Position, Context, RuntimeResult> execMethod)
        {
            this.DefaultValues = defValues;
            this.Parameters = parameters;
            this.ExecutionMethod2 = execMethod;
        }
    }
}
