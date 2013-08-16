using System.Collections.Generic;

namespace Expressions
{
    public class IntegerArgumentInstruction : Instruction
    {
        private readonly int _argument;

        public override int Size
        {
            get { return 2; }
        }

        public int Argument
        {
            get { return _argument; }
        }

        public IntegerArgumentInstruction(Opcode opcode, int argument)
            : base(opcode)
        {
            _argument = argument;
        }

        public override void ToBytecode(IDictionary<string, int> labels, List<int> bytecode)
        {
            bytecode.Add((int)Opcode);
            bytecode.Add(_argument);
        }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), _argument);
        }
    }
}