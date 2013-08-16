namespace Expressions
{
    public class Goto : JumpInstruction
    {
        public Goto(string target)
            : base(Opcode.Goto, target)
        {
        }
    }
}