using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Sigiri
{
    class Program
    {
        public static string FileDirectory { get; set; }
        public static string BaseDirectory { get; set; }
        public static string LibraryExt { get; set; } = ".dll";

        public static void countStr(string str, string val, int start, int cnt) {
           
        }

        static void Main(string[] args)
        {
            DetectLibraryExt();
            BaseDirectory = AppContext.BaseDirectory;
            FileDirectory = AppContext.BaseDirectory;
            string fileName = "<std_input>";
            string contextName = "<program>";
            Context baseContext = new Context(contextName);

            if (args.Length > 0)
            {
                if (args[0].Equals("help"))
                    Console.WriteLine("Display help");
                else if (args[0].Equals("version"))
                    Console.WriteLine("Display version");
                else
                {
                    fileName = args[0];
                    FileDirectory = fileName.Substring(0, fileName.LastIndexOf('\\') + 1);
                    string code = File.ReadAllText(fileName).Replace("\r\n", "\n");
                    Execute(code, fileName, baseContext, false);
                }
            }
            else
            {
                string message = "Sigiri 1.0.0 ";
                message += Environment.Is64BitProcess ? "64bit" : "32bit";
                message += " [Compiled:2020 Jan 17, 13:16:00] running on " + RuntimeInformation.OSDescription;
                message += Environment.Is64BitOperatingSystem ? " x64" : " x86";
                message += "\nType help for more information.";
                Console.Write(message + "\n");

                while (true)
                {
                    Console.Write("-> ");
                    string code = Console.ReadLine();
                    Execute(code, fileName, baseContext, true);
                }
            }
        }

        static void DetectLibraryExt() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                LibraryExt = ".dll";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                LibraryExt = ".so";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                LibraryExt = ".dylib";
        }

        private static void Execute(string code, string fname, Context context, bool inlineOutput) {
            Tokenizer tokenizer = new Tokenizer(fname, code);
            TokenizerResult tokenizerResult = tokenizer.GenerateTokens();
            if (!tokenizerResult.HasError)
            {
                Parser parser = new Parser(tokenizerResult.Tokens);
                ParserResult parserResult = parser.Parse();
                if (!parserResult.HasError)
                {
                    Interpreter interpreter = new Interpreter();
                    RuntimeResult runtimeResult = interpreter.Visit(parserResult.Node, context);
                    if (!runtimeResult.HasError)
                    {
                        if (inlineOutput)
                            Console.WriteLine(runtimeResult.Value);
                    }
                    else
                        Console.WriteLine(runtimeResult.Error);
                }
                else
                    Console.WriteLine(parserResult.Error);
            }
            else
                Console.WriteLine(tokenizerResult.Error);
        }
        
    }
}
