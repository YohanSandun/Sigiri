using System;
using System.IO;

namespace Sigiri
{
    class Program
    {

        static void Main(string[] args)
        {
            Context baseContext = new Context("<module>");

            while (true) {
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
