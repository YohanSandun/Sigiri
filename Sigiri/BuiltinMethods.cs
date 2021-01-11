using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class BuiltinMethods
    {

        public static Dictionary<string, BuiltinMethod> BuiltinMethodList = new Dictionary<string, BuiltinMethod>();
        static bool initialized = false;
        public static void InitializeBuiltinMethods() {
            if (initialized) return;
            initialized = true;
            BuiltinMethodList.Add("print", new BuiltinMethod(new List<string>() { "value", "end"},new Dictionary<string, Values.Value>() {
                { "value", new Values.StringValue("")},
                { "end",  new Values.StringValue("\n")},
            }, print));

            BuiltinMethodList.Add("input", new BuiltinMethod(new List<string>() { "prompt" },new Dictionary<string, Values.Value>() {
                { "prompt", new Values.StringValue("")}
            }, input));

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
    }

    class BuiltinMethod { 
        public List<string> Parameters { get; set; }
        public Dictionary<string, Values.Value> DefaultValues { get; set; }
        public Func<Position,Context,RuntimeResult> ExecutionMethod { get; set; }
        public BuiltinMethod(List<string> parameters, Dictionary<string, Values.Value> defValues, Func<Position, Context, RuntimeResult> execMethod)
        {
            this.DefaultValues = defValues;
            this.Parameters = parameters;
            this.ExecutionMethod = execMethod;
        }
    }
}
