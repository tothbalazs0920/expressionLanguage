namespace Expressions
{
    public class Variable : Expression
    {
        private readonly string _name;

        public Variable(string name)
        {
            _name = name;
        }

        public override int Eval(RuntimeEnvironment runtimeEnvironment, FunctionEnvironment functionEnvironment)
        {
            return runtimeEnvironment.GetVariable(_name).Value;
        }

        public override Type Check(TypeCheckingEnvironment typeCheckingEnvironment, FunctionEnvironment functionEnvironment)
        {
            return typeCheckingEnvironment.GetVariable(_name);
        }

        public override void Compile(CompilationEnvironment compilationEnvironment, Generator generator)
        {
            compilationEnvironment.CompileVariable(generator, _name);
            generator.Emit(Instruction.LdI);
        }
    }
}