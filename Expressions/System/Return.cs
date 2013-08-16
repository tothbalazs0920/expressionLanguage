namespace Expressions
{
    public class Return : IntegerArgumentInstruction
    {
        public Return(int argument) : base(Opcode.Ret, argument) { }
    }
}
