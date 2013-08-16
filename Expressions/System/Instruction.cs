using System.Collections.Generic;

namespace Expressions
{
    public abstract class Instruction
    {
        private readonly Opcode _opcode;

        public static readonly Instruction
            Add = new SimpleInstruction(Opcode.Add),
            Sub = new SimpleInstruction(Opcode.Sub),
            Mul = new SimpleInstruction(Opcode.Mul),
            Div = new SimpleInstruction(Opcode.Div),
            Mod = new SimpleInstruction(Opcode.Mod),
            Eq = new SimpleInstruction(Opcode.Eq),
            LT = new SimpleInstruction(Opcode.LT),
            Not = new SimpleInstruction(Opcode.Not),
            Dup = new SimpleInstruction(Opcode.Dup),
            Swap = new SimpleInstruction(Opcode.Swap),
            LdI = new SimpleInstruction(Opcode.LdI),
            StI = new SimpleInstruction(Opcode.StI),
            GetBp = new SimpleInstruction(Opcode.GetBp),
            GetSp = new SimpleInstruction(Opcode.GetSp),
            PrintC = new SimpleInstruction(Opcode.PrintC),
            PrintI = new SimpleInstruction(Opcode.PrintI),
            Read = new SimpleInstruction(Opcode.Read),
            LdArgs = new SimpleInstruction(Opcode.LdArgs),
            Stop = new SimpleInstruction(Opcode.Stop);

        public Opcode Opcode
        {
            get { return _opcode; }
        }

        public abstract int Size { get; }

        protected Instruction(Opcode opcode)
        {
            _opcode = opcode;
        }

        public abstract void ToBytecode(IDictionary<string, int> labels, List<int> bytecode);

        public override string ToString()
        {
            return _opcode.ToString();
        }
    }
}