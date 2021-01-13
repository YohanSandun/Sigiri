using System;
using System.IO;
using System.Reflection;

namespace Sigiri
{
    class Program
    {
      

        static void Main(string[] args)
        {
            //Assembly assembly = Assembly.LoadFrom(AppContext.BaseDirectory + "\\system.dll");
            //Type type = assembly.GetType("system.math");
            //FieldInfo[] infos = type.GetFields();
            //for (int i = 0; i < infos.Length; i++)
            //{
            //    Console.WriteLine(infos[i].Name + infos[i].GetValue(null));
            //}
            //MethodInfo methodInfo = type.GetMethod("acos");
            ////Console.WriteLine(methodInfo.Invoke(null, new object[] { 0 }));
            //return;

            //double d = 12.4;
            //decimal f = 1111;
            //Console.WriteLine(d.GetType().Name);
            //Console.WriteLine(f.GetType().FullName);
            //return;

            Context baseContext = new Context("<module>");

            while (true) {
                //Console.WriteLine(Math.Atan(2));
                bool inlineOutput = true;
                Console.Write("-> ");
                string code = Console.ReadLine();
                if (code.Equals("f"))
                {
                    code = File.ReadAllText("e:\\test.txt").Replace("\r\n", "\n");
                    inlineOutput = false;
                }
                Tokenizer tokenizer = new Tokenizer("<std_input>", code);
                TokenizerResult tokenizerResult = tokenizer.GenerateTokens();
                if (!tokenizerResult.HasError)
                {
                    Parser parser = new Parser(tokenizerResult.Tokens);
                    ParserResult parserResult = parser.Parse();
                    if (!parserResult.HasError) {
                        Interpreter interpreter = new Interpreter();
                        RuntimeResult runtimeResult = interpreter.Visit(parserResult.Node, baseContext);
                        if (!runtimeResult.HasError)
                        {
                            if (inlineOutput)
                                Console.WriteLine(runtimeResult.Value);
                        }
                        else
                            Console.WriteLine(runtimeResult.Error);
                    } else
                        Console.WriteLine(parserResult.Error);
                }else
                    Console.WriteLine(tokenizerResult.Error);
            }
        }

        
    }
}
