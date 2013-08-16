namespace Expressions
{
    public class Constant : Expression
    {
        private readonly int _value;
        private readonly Type _type;

        public Constant(int value, Type type)
        {
            _value = value;
            _type = type;
        }

        public override int Eval(RuntimeEnvironment runtimeEnvironment, FunctionEnvironment functionEnvironment)
        {
            return _value;
        }

        public override Type Check(TypeCheckingEnvironment typeCheckingEnvironment, FunctionEnvironment functionEnvironment)
        {
            return _type;
        }

        public override void Compile(CompilationEnvironment compilationEnvironment, Generator generator)
        {
            generator.Emit(new CstI(_value));
        }
    }
}