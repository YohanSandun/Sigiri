using System;

namespace Sigiri
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) {
                Console.Write("-> ");
                string code = Console.ReadLine();

                Tokenizer tokenizer = new Tokenizer("<stdin>", code);
                TokenizerResult tokenizerResult = tokenizer.GenerateTokens();
                if (!tokenizerResult.HasError)
                {
                    foreach (Token item in tokenizerResult.Tokens)
                    {
                        Console.WriteLine(item);
                    }
                }else
                    Console.WriteLine(tokenizerResult.Error);
            }
        }

        
    }
}
