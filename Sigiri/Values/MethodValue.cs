using System.Collections.Generic;

namespace Sigiri.Values
{
    class MethodValue : Value
    {
        public string Name { get; set; }
        public Node Body { get; set; }
        public List<string> Parameters { get; set; }
        public Dictionary<string, Node> DefaultValues { get; set; }
        public MethodValue(string name, List<string> parameters, Node body, Dictionary<string, Node> defaultValues) : base(ValueType.METHOD)
        {
            this.Name = name;
            this.Parameters = parameters;
            this.Body = body;
            this.DefaultValues = defaultValues;
        }

        public RuntimeResult Execute(List<(string, Value)> args, Interpreter interpreter) {
            Context context = new Context(this.Name, this.Context);
            if (Parameters.Count != args.Count)
            {
                if (DefaultValues.Count == 0)
                    return new RuntimeResult(new RuntimeError(Position, "Argument count mismatch for '" + Name + "'", this.Context));
                Dictionary<string, Value> values = new Dictionary<string, Value>();
                for (int i = 0; i < Parameters.Count; i++)
                {
                    if (DefaultValues.ContainsKey(Parameters[i]))
                    {
                        RuntimeResult result = interpreter.Visit(DefaultValues[Parameters[i]], this.Context);
                        if (result.HasError) return result;
                        values.Add(Parameters[i], result.Value);
                    }
                    else
                        values.Add(Parameters[i], null);
                }
                for (int i = 0; i < args.Count; i++)
                {
                    if (values.ContainsKey(args[i].Item1))
                    {
                        values[args[i].Item1] = args[i].Item2;
                    }
                    else if (args[i].Item1.Equals("")) {
                        values[Parameters[i]] = args[i].Item2;
                    }
                    else
                        return new RuntimeResult(new RuntimeError(Position, "Unknown parameter name '" + args[i].Item1 + "'", this.Context));
                }
                for (int i = 0; i < Parameters.Count; i++)
                {
                    if (values[Parameters[i]] == null)
                        return new RuntimeResult(new RuntimeError(Position, "Arguments mismatch for '" + Name + "'", this.Context));
                    context.AddSymbol(Parameters[i], values[Parameters[i]]);
                }
            }
            else
            {
                for (int i = 0; i < Parameters.Count; i++)
                    context.AddSymbol(Parameters[i], args[i].Item2);
            }
            return interpreter.Visit(Body, context);
        }

        public override string ToString()
        {
            return "<method:" + Name + ">";
        }
    }
}
