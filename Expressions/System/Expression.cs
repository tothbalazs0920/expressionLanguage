namespace Expressions
{
    /// <summary>
    /// Expression abstract syntax
    /// </summary>
    public abstract class Expression
    {
        abstract public int Eval(RuntimeEnvironment runtimeEnvironment, FunctionEnvironment functionEnvironment);
        abstract public Type Check(TypeCheckingEnvironment typeCheckingEnvironment, FunctionEnvironment functionEnvironment);
        abstract public void Compile(CompilationEnvironment compilationEnvironment, Generator generator);
    }
}