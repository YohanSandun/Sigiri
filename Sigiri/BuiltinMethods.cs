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

            BuiltinMethodList.Add("round", new BuiltinMethod(new List<string>() { "value", "digits" }, new Dictionary<string, Values.Value>() {
                { "value", null },
                { "digits", new Values.IntegerValue(0) }
            }, round));

            BuiltinMethodList.Add("bytes", new BuiltinMethod(new List<string>() { "value", "encoding" }, new Dictionary<string, Values.Value>() {
                { "value", null },
                { "encoding", new Values.StringValue("utf8") }
            }, bytes));

            /* Methods belongs to string objects (not avaiable in public) */
            StringMethods.Add("subString", new BuiltinMethod(new List<string>() { "startIndex", "length" }, new Dictionary<string, Values.Value>() {
                { "startIndex", new Values.IntegerValue(0) },
                { "length", new Values.NullValue() }
            }, str_substr));

            StringMethods.Add("center", new BuiltinMethod(new List<string>() { "width", "fillChar" }, new Dictionary<string, Values.Value>() {
                { "width", null },
                { "fillChar", new Values.StringValue(" ") }
            }, str_center));

            StringMethods.Add("count", new BuiltinMethod(new List<string>() { "str", "start", "count" }, new Dictionary<string, Values.Value>() {
                { "str", null },
                { "start", new Values.IntegerValue(0) },
                { "count", new Values.NullValue() }
            }, str_count));

            StringMethods.Add("encode", new BuiltinMethod(new List<string>() { "encoding" }, new Dictionary<string, Values.Value>() {
                { "encoding", new Values.StringValue("utf8") }
            }, encode));

            StringMethods.Add("index", new BuiltinMethod(new List<string>() { "str", "start", "count" }, new Dictionary<string, Values.Value>() {
                { "str", null },
                { "start", new Values.IntegerValue(0) },
                { "count", new Values.NullValue() }
            }, indexOf));

            StringMethods.Add("lastIndex", new BuiltinMethod(new List<string>() { "str", "start", "count" }, new Dictionary<string, Values.Value>() {
                { "str", null },
                { "start", new Values.NullValue() },
                { "count", new Values.NullValue() }
            }, lastIndex));

            StringMethods.Add("insert", new BuiltinMethod(new List<string>() { "str", "index" }, new Dictionary<string, Values.Value>() {
                { "str", null },
                { "index", null },
            }, str_insert));

            StringMethods.Add("padLeft", new BuiltinMethod(new List<string>() { "width", "fillChar" }, new Dictionary<string, Values.Value>() {
                { "width", null },
                { "fillChar", new Values.StringValue(" ") },
            }, str_padLeft));

            StringMethods.Add("padRight", new BuiltinMethod(new List<string>() { "width", "fillChar" }, new Dictionary<string, Values.Value>() {
                { "width", null },
                { "fillChar", new Values.StringValue(" ") },
            }, str_padRight));


            StringMethods.Add("replace", new BuiltinMethod(new List<string>() { "old", "new", "ignoreCase" }, new Dictionary<string, Values.Value>() {
                { "old", null },
                { "new", null },
                { "ignoreCase", new Values.IntegerValue(false) },
            }, str_replace));

            StringMethods.Add("split", new BuiltinMethod(new List<string>() { "value", "count" }, new Dictionary<string, Values.Value>() {
                { "value", null },
                { "count", new Values.NullValue() }
            }, str_split));
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

        static RuntimeResult round(Position position, Context context) {
            Values.Value value = context.GetSymbol("value");
            Values.Value digits = context.GetSymbol("digits");
            if (value.Type != Values.ValueType.INTEGER && value.Type != Values.ValueType.INT64 && value.Type != Values.ValueType.FLOAT)
                return new RuntimeResult(new RuntimeError(position, "Value must be a numeric value", context));
            if (digits.Type != Values.ValueType.INTEGER && digits.Type != Values.ValueType.INT64)
                return new RuntimeResult(new RuntimeError(position, "Digits must be a integer value", context));
            double result = Math.Round(Convert.ToDouble(value.Data), Convert.ToInt32(digits.Data));
            if (Math.Floor(result) != result)
                return new RuntimeResult(new Values.FloatValue(result).SetPositionAndContext(position, context));
            return new RuntimeResult(new Values.IntegerValue(Convert.ToInt32(result)).SetPositionAndContext(position, context));
        }

        static RuntimeResult bytes(Position position, Context context)
        {
            Values.Value value = context.GetSymbol("value");
            Values.Value encoding = context.GetSymbol("encoding");
            if (encoding.Type != Values.ValueType.STRING)
                return new RuntimeResult(new RuntimeError(position, "Encoding must be a string value", context));
            if (value.Type == Values.ValueType.STRING)
            {
                string str = value.Data.ToString();
                string enc = encoding.Data.ToString();
                switch (enc.ToLower()) {
                    case "utf-8":
                    case "utf8":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.UTF8.GetBytes(str)).SetPositionAndContext(position, context));
                    case "ascii":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.ASCII.GetBytes(str)).SetPositionAndContext(position, context));
                    case "utf-7":
                    case "utf7":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.UTF7.GetBytes(str)).SetPositionAndContext(position, context));
                    case "utf-32":
                    case "utf32":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.UTF32.GetBytes(str)).SetPositionAndContext(position, context));
                    case "bigendian":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.BigEndianUnicode.GetBytes(str)).SetPositionAndContext(position, context));
                    case "unicode":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.Unicode.GetBytes(str)).SetPositionAndContext(position, context));
                    default:
                        return new RuntimeResult(new RuntimeError(position, "Unknown encoding '"+enc+"'", context)); 
                }
            }
            else if (value.Type == Values.ValueType.LIST)
            {
                Values.ByteArrayValue barray = ((Values.ListValue)value).ToByteArray();
                if (barray == null)
                    return new RuntimeResult(new RuntimeError(position, "Error while converting the list into a byte array", context));
                barray.SetPositionAndContext(position, context);
                return new RuntimeResult(barray);
            }
            return new RuntimeResult(new RuntimeError(position, "Value must be a string or list", context));
        }

        #region String Methods
        static RuntimeResult str_substr(Values.Value value, Position position, Context context)
        {
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

        static RuntimeResult str_center(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value widthVal = context.GetSymbol("width");
            Values.Value fill = context.GetSymbol("fillChar");
            if (fill.Type != Values.ValueType.STRING)
                return new RuntimeResult(new RuntimeError(position, "Expected a char for fillChar", context));
            if(fill.Data.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a valid char for fillChar", context));
            char fillChar = fill.Data.ToString()[0];
            if (widthVal.Type != Values.ValueType.INTEGER && widthVal.Type != Values.ValueType.INT64)
                return new RuntimeResult(new RuntimeError(position, "Expected an integer for width", context));
            int width = Convert.ToInt32(widthVal.Data);
            int spaces = width - str.Length;
            int padLeft = spaces / 2 + str.Length;
            string newStr = str.PadLeft(padLeft, fillChar).PadRight(width, fillChar);
            return new RuntimeResult(new Values.StringValue(newStr).SetPositionAndContext(position, context));
        }
        static RuntimeResult str_count(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value strVal = context.GetSymbol("str");
            Values.Value startVal = context.GetSymbol("start");
            Values.Value countVal = context.GetSymbol("count");

            if (strVal.Type != Values.ValueType.STRING)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for search", context));
            if (strVal.Data.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a valid char for fillChar", context));
            string subStr = strVal.Data.ToString();
            if (startVal.Type != Values.ValueType.INTEGER && startVal.Type != Values.ValueType.INT64)
                return new RuntimeResult(new RuntimeError(position, "Expected an integer for start index", context));
            int start = Convert.ToInt32(startVal.Data);
            if (start >= str.Length)
                return new RuntimeResult(new RuntimeError(position, "Index out of range!", context));
            int count = str.Length-start;
            //todo throw an error for count is out of range
            if (countVal.Type != Values.ValueType.NULL)
            {
                if (countVal.Type != Values.ValueType.INTEGER && countVal.Type != Values.ValueType.INT64)
                    return new RuntimeResult(new RuntimeError(position, "Expected an integer for count", context));
                count = Convert.ToInt32(countVal.Data);
            }
            return new RuntimeResult(new Values.StringValue(Util.CountSubstring(str, subStr, start, count)).SetPositionAndContext(position, context));
        }

        static RuntimeResult encode(Values.Value value, Position position, Context context)
        {
            Values.Value encoding = context.GetSymbol("encoding");
            if (encoding.Type != Values.ValueType.STRING)
                return new RuntimeResult(new RuntimeError(position, "Encoding must be a string value", context));
            if (value.Type == Values.ValueType.STRING)
            {
                string str = value.Data.ToString();
                string enc = encoding.Data.ToString();
                switch (enc.ToLower())
                {
                    case "utf-8":
                    case "utf8":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.UTF8.GetBytes(str)).SetPositionAndContext(position, context));
                    case "ascii":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.ASCII.GetBytes(str)).SetPositionAndContext(position, context));
                    case "utf-7":
                    case "utf7":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.UTF7.GetBytes(str)).SetPositionAndContext(position, context));
                    case "utf-32":
                    case "utf32":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.UTF32.GetBytes(str)).SetPositionAndContext(position, context));
                    case "bigendian":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.BigEndianUnicode.GetBytes(str)).SetPositionAndContext(position, context));
                    case "unicode":
                        return new RuntimeResult(new Values.ByteArrayValue(System.Text.Encoding.Unicode.GetBytes(str)).SetPositionAndContext(position, context));
                    default:
                        return new RuntimeResult(new RuntimeError(position, "Unknown encoding '" + enc + "'", context));
                }
            }
            else if (value.Type == Values.ValueType.LIST)
            {
                Values.ByteArrayValue barray = ((Values.ListValue)value).ToByteArray();
                if (barray == null)
                    return new RuntimeResult(new RuntimeError(position, "Error while converting the list into a byte array", context));
                barray.SetPositionAndContext(position, context);
                return new RuntimeResult(barray);
            }
            return new RuntimeResult(new RuntimeError(position, "Value must be a string or list", context));
        }

        static RuntimeResult indexOf(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value strVal = context.GetSymbol("str");
            Values.Value startVal = context.GetSymbol("start");
            Values.Value countVal = context.GetSymbol("count");

            if (strVal.Type != Values.ValueType.STRING)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for search", context));
            if (strVal.Data.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for search", context));
            string subStr = strVal.Data.ToString();
            if (startVal.Type != Values.ValueType.INTEGER && startVal.Type != Values.ValueType.INT64)
                return new RuntimeResult(new RuntimeError(position, "Expected an integer for start index", context));
            int start = Convert.ToInt32(startVal.Data);
            if (start >= str.Length)
                return new RuntimeResult(new RuntimeError(position, "Index out of range!", context));
            int count = str.Length-start;
            //todo throw an error for count is out of range
            if (countVal.Type != Values.ValueType.NULL)
            {
                if (countVal.Type != Values.ValueType.INTEGER && countVal.Type != Values.ValueType.INT64)
                    return new RuntimeResult(new RuntimeError(position, "Expected an integer for count", context));
                count = Convert.ToInt32(countVal.Data);
            }
            try
            {
                return new RuntimeResult(new Values.IntegerValue(str.IndexOf(subStr, start, count)).SetPositionAndContext(position, context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(position, "Index out of range!", context));
            } 
        }
        static RuntimeResult lastIndex(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value strVal = context.GetSymbol("str");
            Values.Value startVal = context.GetSymbol("start");
            Values.Value countVal = context.GetSymbol("count");

            if (strVal.Type != Values.ValueType.STRING)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for search", context));
            if (strVal.Data.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for search", context));
            string subStr = strVal.Data.ToString();
            if (startVal.Type != Values.ValueType.INTEGER && startVal.Type != Values.ValueType.INT64 && startVal.Type != Values.ValueType.NULL)
                return new RuntimeResult(new RuntimeError(position, "Expected an integer for start index", context));
            int start;
            if (startVal.Type == Values.ValueType.NULL)
                start = str.Length - 1;
            else
                start = Convert.ToInt32(startVal.Data); 
            if (start >= str.Length)
                return new RuntimeResult(new RuntimeError(position, "Index out of range!", context));
            int count = start + 1;
            //todo throw an error for count is out of range
            if (countVal.Type != Values.ValueType.NULL)
            {
                if (countVal.Type != Values.ValueType.INTEGER && countVal.Type != Values.ValueType.INT64)
                    return new RuntimeResult(new RuntimeError(position, "Expected an integer for count", context));
                count = Convert.ToInt32(countVal.Data);
            }
            try
            {
                return new RuntimeResult(new Values.IntegerValue(str.LastIndexOf(subStr, start, count)).SetPositionAndContext(position, context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(position, "Index out of range!", context));
            }
        }

        static RuntimeResult str_insert(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value strVal = context.GetSymbol("str");
            Values.Value idxVal = context.GetSymbol("index");;

            if (strVal.Data.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for search", context));
            string subStr = strVal.ToString();
            if (idxVal.Type != Values.ValueType.INTEGER && idxVal.Type != Values.ValueType.INT64)
                return new RuntimeResult(new RuntimeError(position, "Expected an integer for index", context));
            int index = Convert.ToInt32(idxVal.Data);
            if (index > str.Length || index < 0)
                return new RuntimeResult(new RuntimeError(position, "Index out of range!", context));
            try
            {
                return new RuntimeResult(new Values.StringValue(str.Substring(0, index) + subStr + str.Substring(index)).SetPositionAndContext(position, context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(position, "Index out of range!", context));
            }
        }
        static RuntimeResult str_padLeft(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value widthVal = context.GetSymbol("width");
            Values.Value fillVal = context.GetSymbol("fillChar"); ;

            if (fillVal.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for fillChar", context));
            string fill = fillVal.ToString();
            if (widthVal.Type != Values.ValueType.INTEGER && widthVal.Type != Values.ValueType.INT64)
                return new RuntimeResult(new RuntimeError(position, "Expected an integer for width", context));
            int width = Convert.ToInt32(widthVal.Data);
            Console.WriteLine(fill[0] + " " + width);
            try
            {
                return new RuntimeResult(new Values.StringValue(str.PadLeft(width, fill[0])).SetPositionAndContext(position, context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(position, "Error while padding the string!", context));
            }
        }
        static RuntimeResult str_padRight(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value widthVal = context.GetSymbol("width");
            Values.Value fillVal = context.GetSymbol("fillChar"); ;

            if (fillVal.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for fillChar", context));
            string fill = fillVal.ToString();
            if (widthVal.Type != Values.ValueType.INTEGER && widthVal.Type != Values.ValueType.INT64)
                return new RuntimeResult(new RuntimeError(position, "Expected an integer for width", context));
            int width = Convert.ToInt32(widthVal.Data);
            try
            {
                return new RuntimeResult(new Values.StringValue(str.PadRight(width, fill[0])).SetPositionAndContext(position, context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(position, "Error while padding the string!", context));
            }
        }
        static RuntimeResult str_replace(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value oldVal = context.GetSymbol("old");
            Values.Value newVal = context.GetSymbol("new");
            Values.Value ignoreCase = context.GetSymbol("ignoreCase");

            if (oldVal.ToString().Length == 0)
                return new RuntimeResult(new RuntimeError(position, "Expected a string for fillChar", context));
            string newStr = newVal.ToString();
            string oldStr = oldVal.ToString();
            try
            {
                return new RuntimeResult(new Values.StringValue(str.Replace(oldStr, newStr, ignoreCase.GetAsBoolean(), System.Globalization.CultureInfo.CurrentCulture)).SetPositionAndContext(position, context));
            }
            catch
            {
                return new RuntimeResult(new RuntimeError(position, "Error while padding the string!", context));
            }
        }
        static RuntimeResult str_split(Values.Value value, Position position, Context context)
        {
            string str = value.ToString();
            Values.Value valVal = context.GetSymbol("value");
            Values.Value cntVal = context.GetSymbol("count");

            if (cntVal.Type != Values.ValueType.INT64 && cntVal.Type != Values.ValueType.INTEGER && cntVal.Type != Values.ValueType.NULL)
                return new RuntimeResult(new RuntimeError(position, "Expected an interger for count", context));
            if (valVal.Type == Values.ValueType.STRING || valVal.Type == Values.ValueType.CLASS)
            {
                string val = valVal.ToString();
                string[] array;
                if (cntVal.Type == Values.ValueType.NULL)
                    array = str.Split(val);
                else
                    array = str.Split(val, Convert.ToInt32(cntVal.Data));
                return new RuntimeResult(Values.ListValue.FromArray(array));
            }
            else if (valVal.Type == Values.ValueType.LIST) {
                string[] seps = ((Values.ListValue)valVal).ToStringArray();
                string[] array;
                if (cntVal.Type == Values.ValueType.NULL)
                    array = str.Split(seps, StringSplitOptions.None);
                else
                    array = str.Split(seps, Convert.ToInt32(cntVal.Data), StringSplitOptions.None);
                return new RuntimeResult(Values.ListValue.FromArray(array));
            }
            return new RuntimeResult(new RuntimeError(position, "Error while splitting the string!", context));
        }
        #endregion
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
