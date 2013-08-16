namespace Expressions
{
    public enum Opcode
    {
        Label = -1, // Unused
        CstI, Add, Sub, Mul, Div, Mod, Eq, LT, Not,
        Dup, Swap, LdI, StI, GetBp, GetSp, IncSp,
        Goto, IfZero, IfNZero, Call, TCall, Ret,
        PrintI, PrintC, Read, LdArgs,
        Stop
    }
}