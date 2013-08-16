// Abstract syntax, interpretation and compilation 
// for arithmetic and logical expressions
// sestoft@itu.dk 2007-03-12, 2010-02-08
// Extended with functions by Rasmus Mogelberg 2011-10-20
// Split into multiple files and proper naming as to C# 5.0 conventions by Rasmus Nielsen 2012-11-06

using System;

namespace Expressions
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                var scanner = new Scanner(args[0]);
                var parser = new Parser(scanner);
                parser.Parse();
                switch (args[1])
                {
                    case "run":
                        Console.WriteLine(parser.program.Eval());
                        return;
                    case "check":
                        parser.program.Check();
                        return;
                    case "compile":
                        var gen = new Generator();
                        const string outputFile = "a.out";
                        parser.program.Compile(gen, outputFile);
                        return;
                }
            }
            Console.WriteLine("Usage: Program <expression.txt> [run|check|compile]");
        }
    }
}