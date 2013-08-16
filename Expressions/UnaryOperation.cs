using System;

namespace Expressions
{
    public class UnaryOperation : Expression
    {
        private readonly Operator _op;
        private readonly Expression _expression;

        public UnaryOperation(Operator op, Expression expression)
        {
            _op = op;
            _expression = expression;
        }

        public override int Eval(RuntimeEnvironment runtimeEnvironment, FunctionEnvironment functionEnvironment)
        {
            var value = _expression.Eval(runtimeEnvironment, functionEnvironment);
            switch (_op)
            {
                case Operator.Not:
                    return value == 0 ? 1 : 0;
                case Operator.Neg:
                    return -value;
                default:
                    throw new InvalidOperationException(string.Format("Unknown unary operator: {0}", _op));
            }
        }

        public override Type Check(TypeCheckingEnvironment typeCheckingEnvironment, FunctionEnvironment functionEnvironment)
        {
            var t1 = _expression.Check(typeCheckingEnvironment, functionEnvironment);
            switch (_op)
            {
                case Operator.Neg:
                    if (t1 == Type.IntegerType)
                    {
                        return Type.IntegerType;
                    }
                    throw new InvalidOperationException("Argument to - must be int");
                case Operator.Not:
                    if (t1 == Type.BooleanType)
                    {
                        return Type.BooleanType;
                    }
                    throw new InvalidOperationException("Argument to ! must be bool");
                default:
                    throw new InvalidOperationException(string.Format("Unknown unary operator: {0}", _op));
            }
        }

        public override void Compile(CompilationEnvironment compilationEnvironment, Generator generator)
        {
            _expression.Compile(compilationEnvironment, generator);
            switch (_op)
            {
                case Operator.Not:
                    generator.Emit(Instruction.Not);
                    break;
                case Operator.Neg:
                    generator.Emit(new CstI(0));
                    generator.Emit(Instruction.Swap);
                    generator.Emit(Instruction.Sub);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown unary operator: {0}", _op));
            }
        }
    }
}