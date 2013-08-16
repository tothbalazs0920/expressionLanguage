namespace Expressions
{
    public class CstI : IntegerArgumentInstruction
    {
        public CstI(int argument)
            : base(Opcode.CstI, argument)
        {
        }
    }
}