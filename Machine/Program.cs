/* A unified-stack abstract machine for imperative programs
   sestoft@dina.kvl.dk * 2001-03-21, 2007-03-15

   Compile this file with 

      csc Program.cs

   To execute a program file using this abstract machine, do:

      Program <programfile> <arg1> <arg2> ...

   or

      Program -trace <programfile> <arg1> <arg2> ...
 
   The Read instruction will attempt to read from file a.in if it exists.
*/

// Split into several files and upgraded to C# 5.0 by Rasmus Nielsen 2012-11-06

using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Machine
{
    class Program
    {
        public enum Opcode // Must match opcodes from Expressions.csproj...
        {
            Label = -1, // Unused
            CstI, Add, Sub, Mul, Div, Mod, Eq, LT, Not,
            Dup, Swap, LdI, StI, GetBp, GetSp, IncSp,
            Goto, IfZero, IfNZero, Call, TCall, Ret,
            PrintI, PrintC, Read, LdArgs,
            Stop
        }

        static void Main(string[] args)
        {
            var trace = args.Length > 0 && (args[0] == "-trace" || args[0] == "/trace");
            if (args.Length == 0 || trace && args.Length == 1)
            {
                Console.WriteLine("Usage: Program [-trace] <programfile> <arg1> ...\n");
            }
            else
            {
                Execute(args, trace);
            }
        }

        const int StackSize = 1000;

        static void Execute(string[] args, bool trace)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            var firstArg = trace ? 1 : 0;
            var p = Readfile(args[firstArg]);         // Read the program from file
            var s = new int[StackSize];               // The evaluation stack
            var inputs = MakeInputReader("a.in").GetEnumerator();
            var iargs = new int[args.Length - firstArg - 1];
            for (var i = firstArg + 1; i < args.Length; i++) // Commandline arguments
            {
                iargs[i - 1] = int.Parse(args[i]);
            }

            var stopwatch = Stopwatch.StartNew();
            ExecCode(p, s, iargs, inputs, trace);
            stopwatch.Stop();

            Console.Error.WriteLine("\nRan {0:n1} seconds", stopwatch.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// Read a stream of integers from a text file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IEnumerable<int> MakeInputReader(string filename)
        {
            var file = new FileInfo(filename);
            if (file.Exists)
            {
                var regex = new Regex("[ \\t]+");
                using (TextReader reader = file.OpenText())
                {
                    for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                        foreach (var s in regex.Split(line))
                        {
                            if (s != "")
                            {
                                yield return int.Parse(s);
                            }
                        }
                }
            }
        }

        /// <summary>
        ///  The machine: execute the code starting at p[pc] 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <param name="iargs"></param>
        /// <param name="inputs"></param>
        /// <param name="trace"></param>
        /// <returns></returns>
        static int ExecCode(int[] p, int[] s, int[] iargs, IEnumerator<int> inputs, bool trace)
        {
            var basePointer = -999;  // Base pointer, for accessing local
            var stackPointer = -1;    // Stack top pointer: current top of stack
            var programCounter = 0;     // Program counter: next instruction
            while (true)
            {
                if (trace)
                {
                    PrintSpPc(s, basePointer, stackPointer, p, programCounter);
                }
                switch ((Opcode)p[programCounter++])
                {
                    case Opcode.CstI:
                        s[stackPointer + 1] = p[programCounter++]; stackPointer++;
                        break;
                    case Opcode.Add:
                        s[stackPointer - 1] = s[stackPointer - 1] + s[stackPointer]; stackPointer--;
                        break;
                    case Opcode.Sub:
                        s[stackPointer - 1] = s[stackPointer - 1] - s[stackPointer]; stackPointer--;
                        break;
                    case Opcode.Mul:
                        s[stackPointer - 1] = s[stackPointer - 1] * s[stackPointer]; stackPointer--;
                        break;
                    case Opcode.Div:
                        s[stackPointer - 1] = s[stackPointer - 1] / s[stackPointer]; stackPointer--;
                        break;
                    case Opcode.Mod:
                        s[stackPointer - 1] = s[stackPointer - 1] % s[stackPointer]; stackPointer--;
                        break;
                    case Opcode.Eq:
                        s[stackPointer - 1] = (s[stackPointer - 1] == s[stackPointer] ? 1 : 0); stackPointer--;
                        break;
                    case Opcode.LT:
                        s[stackPointer - 1] = (s[stackPointer - 1] < s[stackPointer] ? 1 : 0); stackPointer--;
                        break;
                    case Opcode.Not:
                        s[stackPointer] = (s[stackPointer] == 0 ? 1 : 0);
                        break;
                    case Opcode.Dup:
                        s[stackPointer + 1] = s[stackPointer]; stackPointer++;
                        break;
                    case Opcode.Swap:
                        var tmp = s[stackPointer];
                        s[stackPointer] = s[stackPointer - 1];
                        s[stackPointer - 1] = tmp;
                        break;
                    case Opcode.LdI:                 // load indirect
                        s[stackPointer] = s[s[stackPointer]];
                        break;
                    case Opcode.StI:                 // store indirect, keep value on top
                        s[s[stackPointer - 1]] = s[stackPointer]; s[stackPointer - 1] = s[stackPointer]; stackPointer--;
                        break;
                    case Opcode.GetBp:
                        s[stackPointer + 1] = basePointer; stackPointer++;
                        break;
                    case Opcode.GetSp:
                        s[stackPointer + 1] = stackPointer; stackPointer++;
                        break;
                    case Opcode.IncSp:
                        stackPointer = stackPointer + p[programCounter++];
                        break;
                    case Opcode.Goto:
                        programCounter = p[programCounter];
                        break;
                    case Opcode.IfZero:
                        programCounter = (s[stackPointer--] == 0 ? p[programCounter] : programCounter + 1);
                        break;
                    case Opcode.IfNZero:
                        programCounter = (s[stackPointer--] != 0 ? p[programCounter] : programCounter + 1);
                        break;
                    case Opcode.Call:
                        {
                            var argc = p[programCounter++];
                            for (var i = 0; i < argc; i++) // Make room for return address
                                s[stackPointer - i + 2] = s[stackPointer - i]; // and old base pointer
                            s[stackPointer - argc + 1] = programCounter + 1;
                            stackPointer++;
                            s[stackPointer - argc + 1] = basePointer;
                            stackPointer++;
                            basePointer = stackPointer + 1 - argc;
                            programCounter = p[programCounter];
                        }
                        break;
                    case Opcode.TCall:
                        {
                            int argc = p[programCounter++];                // Number of new arguments
                            int pop = p[programCounter++];		   // Number of variables to discard
                            for (int i = argc - 1; i >= 0; i--)	   // Discard variables
                                s[stackPointer - i - pop] = s[stackPointer - i];
                            stackPointer = stackPointer - pop; programCounter = p[programCounter];
                        } break;
                    case Opcode.Ret:
                        {
                            int res = s[stackPointer];
                            stackPointer = stackPointer - p[programCounter]; basePointer = s[--stackPointer]; programCounter = s[--stackPointer];
                            s[stackPointer] = res;
                        } 
                        break;
                    case Opcode.PrintI:
                        Console.Write(s[stackPointer] + " "); 
                        break;
                    case Opcode.PrintC:
                        Console.Write((char)(s[stackPointer])); 
                        break;
                    case Opcode.LdArgs:
                        foreach (var x in iargs)
                        {
                            s[++stackPointer] = x;
                        }
                        break;
                    case Opcode.Read:
                        if (inputs.MoveNext())
                        {
                            s[++stackPointer] = inputs.Current;
                        }
                        else
                        {
                            throw new InvalidOperationException("No more input");
                        }
                        break;
                    case Opcode.Stop:
                        return stackPointer;
                    default:
                        throw new InvalidOperationException(string.Format("Illegal instruction {0} at address {1}", p[programCounter - 1], programCounter - 1));
                }
            }
        }

        /// <summary>
        /// Print the stack machine instruction at p[pc]
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        private static string InsName(int[] p, int pc)
        {
            switch ((Opcode)p[pc])
            {
                case Opcode.CstI: return "CST " + p[pc + 1];
                case Opcode.Add: return "ADD";
                case Opcode.Sub: return "SUB";
                case Opcode.Mul: return "MUL";
                case Opcode.Div: return "DIV";
                case Opcode.Mod: return "MOD";
                case Opcode.Eq: return "EQ";
                case Opcode.LT: return "LT";
                case Opcode.Not: return "NOT";
                case Opcode.Dup: return "DUP";
                case Opcode.Swap: return "SWAP";
                case Opcode.LdI: return "LDI";
                case Opcode.StI: return "StI";
                case Opcode.GetBp: return "GetBp";
                case Opcode.GetSp: return "GetSp";
                case Opcode.IncSp: return "IncSp " + p[pc + 1];
                case Opcode.Goto: return "GOTO " + p[pc + 1];
                case Opcode.IfZero: return "IfZero " + p[pc + 1];
                case Opcode.IfNZero: return "IfNZero " + p[pc + 1];
                case Opcode.Call: return "CALL " + p[pc + 1] + " " + p[pc + 2];
                case Opcode.TCall: return "TCall " + p[pc + 1] + " " + p[pc + 2] + " " + p[pc + 3];
                case Opcode.Ret: return "Ret " + p[pc + 1];
                case Opcode.PrintI: return "PrintI";
                case Opcode.PrintC: return "PrintC";
                case Opcode.LdArgs: return "LdArgs";
                case Opcode.Read: return "Read";
                case Opcode.Stop: return "Stop";
                default: return "<unknown>";
            }
        }

        /// <summary>
        ///  Print current stack and current instruction
        /// </summary>
        /// <param name="s"></param>
        /// <param name="bp"></param>
        /// <param name="sp"></param>
        /// <param name="p"></param>
        /// <param name="pc"></param>
        static void PrintSpPc(int[] s, int bp, int sp, int[] p, int pc)
        {
            Console.Write("[ ");
            for (var i = 0; i <= sp; i++)
            {
                Console.Write(s[i] + " ");
            }
            Console.Write("]");
            Console.WriteLine("{" + pc + ": " + InsName(p, pc) + "}");
        }

        /// <summary>
        /// Read instructions from a file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static int[] Readfile(string filename)
        {
            var rawprogram = new List<int>();
            rawprogram.AddRange(MakeInputReader(filename));
            return rawprogram.ToArray();
        }
    }
}
