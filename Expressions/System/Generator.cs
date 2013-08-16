using System;
using System.Collections.Generic;

namespace Expressions
{
    /// <summary>
    /// Code generation 
    /// </summary>
    public class Generator
    {
        private readonly IList<Instruction> _instructions;

        public Generator()
        {
            _instructions = new List<Instruction>();
        }

        public void Emit(Instruction instr)
        {
            _instructions.Add(instr);
        }

        public void Label(string label)
        {
            _instructions.Add(new Label(label));
        }

        public int[] ToBytecode()
        {
            // Pass 1: Build mapping from labels to absolute addresses
            var labels = new Dictionary<string, int>();
            var address = 0;
            foreach (var instr in _instructions)
            {
                if (instr is Label)
                {
                    labels.Add((instr as Label).Name, address);
                }
                else
                {
                    address += instr.Size;
                }
            }
            // Pass 2: Use mapping to convert symbolic code to bytes
            var bytecode = new List<int>();
            foreach (var instruction in _instructions)
            {
                instruction.ToBytecode(labels, bytecode);
            }
            return bytecode.ToArray();
        }

        public void PrintCode()
        {
            var address = 0;
            foreach (var instr in _instructions)
            {
                Console.WriteLine("{0,5} {1}", address, instr);
                address += instr.Size;
            }
        }
    }
}