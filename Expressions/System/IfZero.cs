namespace Expressions
{
    public class IfZero : JumpInstruction
    {
        public IfZero(string target)
            : base(Opcode.IfZero, target)
        {
        }
    }
}