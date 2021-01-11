using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class BuiltinMethods
    {
        public static Dictionary<string, int[]> Methods = new Dictionary<string, int[]>()
        {
            { "print", new int[] { 0, 1 } },
            { "input", new int[] { 0, 1 } },
            { "write", new int[] { 0, 1 } }
        };

        public static RuntimeResult Execute(string name, List<(string, Values.Value)> args, Position position, Context context) {
            switch (name)
            {
                case "print":
                    return print(name, args, position, context);
                case "input":
                    return input(name, args, position, context);
                case "write":
                    return write(name, args, position, context);
            }
            return new RuntimeResult(new RuntimeError(position, "Builtin method '" + name + "' not found!", context));
        }

        static bool ValidateArgumentCount(string name, int argCnt) {
            int[] originalArgCounts = Methods[name];
            for (int i = 0; i < originalArgCounts.Length; i++)
            {
                if (argCnt == originalArgCounts[i])
                    return true;
            }
            return false;
        }

        static RuntimeResult print(string name, List<(string, Values.Value)> args, Position position, Context context) {
            if (!ValidateArgumentCount(name, args.Count))
                return new RuntimeResult(new RuntimeError(position, "Argument count mismatch for builtin method '" + name + "'", context));
            string str = "\n";
            if (args.Count == 1)
                str = args[0].Item2.Data.ToString() + "\n";
            Console.Write(str);
            return new RuntimeResult(new Values.StringValue(str).SetPositionAndContext(position, context));
        }

        static RuntimeResult write(string name, List<(string, Values.Value)> args, Position position, Context context)
        {
            if (!ValidateArgumentCount(name, args.Count))
                return new RuntimeResult(new RuntimeError(position, "Argument count mismatch for builtin method '" + name + "'", context));
            string str = "";
            if (args.Count == 1)
                str = args[0].Item2.ToString();
            Console.Write(str);
            return new RuntimeResult(new Values.StringValue(str).SetPositionAndContext(position, context));
        }

        static RuntimeResult input(string name, List<(string, Values.Value)> args, Position position, Context context)
        {
            if (!ValidateArgumentCount(name, args.Count))
                return new RuntimeResult(new RuntimeError(position, "Argument count mismatch for builtin method '" + name + "'", context));
            string str = "";
            if (args.Count == 1)
                str = args[0].Item2.ToString();
            Console.Write(str);
            string input = Console.ReadLine();
            return new RuntimeResult(new Values.StringValue(input).SetPositionAndContext(position, context));
        }
    }
}
