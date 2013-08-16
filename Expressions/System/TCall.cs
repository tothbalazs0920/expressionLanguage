using System.Collections.Generic;

namespace Expressions
{
    public class TCall : Instruction
    {
        private readonly int _argCount;
        private readonly int _slideBy;
        private readonly string _target;

        public override int Size
        {
            get { return 4; }
        }

        public int ArgCount
        {
            get { return _argCount; }
        }

        public int SlideBy
        {
            get { return _slideBy; }
        }

        public string Target
        {
            get { return _target; }
        }

        public TCall(int argCount, int slideBy, string target)
            : base(Opcode.TCall)
        {
            _argCount = argCount;
            _slideBy = slideBy;
            _target = target;
        }

        public override void ToBytecode(IDictionary<string, int> labels, List<int> bytecode)
        {
            bytecode.Add((int)Opcode);
            bytecode.Add(_argCount);
            bytecode.Add(_slideBy);
            bytecode.Add(labels[_target]);
        }
    }
}