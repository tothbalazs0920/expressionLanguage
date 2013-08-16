using System.Collections.Generic;

namespace Expressions
{
    /// <summary>
    /// Pseudo-instruction for generating fresh labels
    /// </summary>
    public class Label : Instruction
    {  
        private readonly string _name;
        private static int _last; 

        public override int Size
        {
            get { return 0; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Label(string name)
            : base(Opcode.Label)
        {
            _name = name;
        }

        public override void ToBytecode(IDictionary<string, int> labels, List<int> bytecode)
        {
            // No bytecode for a label
        }

        public static string Fresh()
        {
            _last++;
            return "L" + _last;
        }

        public override string ToString()
        {
            return _name + ":";
        }
    }
}