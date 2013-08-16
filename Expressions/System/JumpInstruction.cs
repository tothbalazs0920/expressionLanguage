using System.Collections.Generic;

namespace Expressions
{
    public class JumpInstruction : Instruction
    {
        private readonly string _target;

        public override int Size
        {
            get { return 2; }
        }

        public string Target
        {
            get { return _target; }
        }

        public JumpInstruction(Opcode opcode, string target)
            : base(opcode)
        {
            _target = target;
        }

        public override void ToBytecode(IDictionary<string, int> labels, List<int> bytecode)
        {
            bytecode.Add((int)Opcode);
            bytecode.Add(labels[_target]);
        }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), _target);
        }
    }
}