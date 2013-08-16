using System.Collections.Generic;

namespace Expressions
{
    public class Call : Instruction
    {
        private readonly int _argCount;
        private readonly string _target;

        public override int Size
        {
            get { return 3; }
        }

        public int ArgCount
        {
            get { return _argCount; }
        }

        public string Target
        {
            get { return _target; }
        }

        public Call(int argCount, string target)
            : base(Opcode.Call)
        {
            _argCount = argCount;
            _target = target;
        }

        public override void ToBytecode(IDictionary<string, int> labels, List<int> bytecode)
        {
            bytecode.Add((int)Opcode);
            bytecode.Add(_argCount);
            bytecode.Add(labels[_target]);
        }

        public override string ToString()
        {
            return string.Join(" ", base.ToString(), _argCount, _target);
        }
    }
}