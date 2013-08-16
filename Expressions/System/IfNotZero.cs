namespace Expressions
{
    public class IfNotZero : JumpInstruction
    {
        public IfNotZero(string target)
            : base(Opcode.IfNZero, target)
        {
        }
    }
}