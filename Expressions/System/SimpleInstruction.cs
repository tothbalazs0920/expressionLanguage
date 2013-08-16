using System.Collections.Generic;

namespace Expressions
{
    public class SimpleInstruction : Instruction
    {
        public SimpleInstruction(Opcode opcode) : base(opcode) { }

        public override int Size
        {
            get { return 1; }
        }

        public override void ToBytecode(IDictionary<string, int> labels, List<int> bytecode)
        {
            bytecode.Add((int)Opcode);
        }

        public override string ToString()
        {
            return Opcode.ToString();
        }
    }
}