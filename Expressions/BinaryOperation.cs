using System;

namespace Expressions
{
    public class BinaryOperation : Expression
    {
        private readonly Operator _op;
        private readonly Expression _e1, _e2;

        public BinaryOperation(Operator op, Expression e1, Expression e2)
        {
            _op = op;
            _e1 = e1;
            _e2 = e2;
        }

        public override int Eval(RuntimeEnvironment runtimeEnvironment, FunctionEnvironment functionEnvironment)
        {
            var v1 = _e1.Eval(runtimeEnvironment, functionEnvironment);
            var v2 = _e2.Eval(runtimeEnvironment, functionEnvironment);

            switch (_op)
            {
                case Operator.Add:
                    return v1 + v2;
                case Operator.Div:
                    return v1 / v2;
                case Operator.Mul:
                    return v1 * v2;
                case Operator.Sub:
                    return v1 - v2;
                case Operator.Eq:
                    return v1 == v2 ? 1 : 0;
                case Operator.Ne:
                    return v1 != v2 ? 1 : 0;
                case Operator.Lt:
                    return v1 < v2 ? 1 : 0;
                case Operator.Le:
                    return v1 <= v2 ? 1 : 0;
                case Operator.Gt:
                    return v1 > v2 ? 1 : 0;
                case Operator.Ge:
                    return v1 >= v2 ? 1 : 0;
                case Operator.And:
                    return v1 == 0 ? 0 : v2;
                case Operator.Or:
                    return v1 == 0 ? v2 : 1;
                case Operator.Mod:      //Balazs
                    return v1 % v2;
                default:
                    throw new InvalidOperationException(string.Format("Unknown binary operator: {0}", _op));
            }
        }

        public override Type Check(TypeCheckingEnvironment typeCheckingEnvironment, FunctionEnvironment functionEnvironment)
        {
            var t1 = _e1.Check(typeCheckingEnvironment, functionEnvironment);
            var t2 = _e2.Check(typeCheckingEnvironment, functionEnvironment);

            switch (_op)
            {
                case Operator.Add:
                case Operator.Div:
                case Operator.Mul:
                case Operator.Sub:
                    if (t1 == Type.IntegerType && t2 == Type.IntegerType)
                    {
                        return Type.IntegerType;
                    }
                    throw new InvalidOperationException("Arguments to + - * / must be int");
                case Operator.Mod:          //Balazs
                    if (t1 == Type.IntegerType && t2 == Type.IntegerType)
                        return Type.IntegerType;
                    else
                        throw new InvalidOperationException("Arguments to + - * / % must be int");
                case Operator.Eq:
                case Operator.Ge:
                case Operator.Gt:
                case Operator.Le:
                case Operator.Lt:
                case Operator.Ne:
                    if (t1 == Type.IntegerType && t2 == Type.IntegerType)
                    {
                        return Type.BooleanType;
                    }
                    throw new InvalidOperationException("Arguments to == >= > <= < != must be int");
                case Operator.Or:
                case Operator.And:
                    if (t1 == Type.BooleanType && t2 == Type.BooleanType)
                    {
                        return Type.BooleanType;
                    }
                    throw new InvalidOperationException("Arguments to & must be bool");
                default: 
                    throw new InvalidOperationException(string.Format("Unknown binary operator: {0}", _op));
            }
        }

        public override void Compile(CompilationEnvironment compilationEnvironment, Generator generator)
        {
            _e1.Compile(compilationEnvironment, generator);
            compilationEnvironment.PushTemporary();
            _e2.Compile(compilationEnvironment, generator);

            switch (_op)
            {
                case Operator.Mod:
                    generator.Emit(Instruction.Mod);
                    break;
                case Operator.Add:
                    generator.Emit(Instruction.Add);
                    break;
                case Operator.Div:
                    generator.Emit(Instruction.Div);
                    break;
                case Operator.Mul:
                    generator.Emit(Instruction.Mul);
                    break;
                case Operator.Sub:
                    generator.Emit(Instruction.Sub);
                    break;
                case Operator.Eq:
                    generator.Emit(Instruction.Eq);
                    break;
                case Operator.Ne:
                    generator.Emit(Instruction.Eq);
                    generator.Emit(Instruction.Not);
                    break;
                case Operator.Ge:
                    generator.Emit(Instruction.LT);
                    generator.Emit(Instruction.Not);
                    break;
                case Operator.Gt:
                    generator.Emit(Instruction.Swap);
                    generator.Emit(Instruction.LT);
                    break;
                case Operator.Le:
                    generator.Emit(Instruction.Swap);
                    generator.Emit(Instruction.LT);
                    generator.Emit(Instruction.Not);
                    break;
                case Operator.Lt:
                    generator.Emit(Instruction.LT);
                    break;
                case Operator.And:
                    generator.Emit(Instruction.Mul);
                    break;
                case Operator.Or:
                    generator.Emit(Instruction.Add);
                    generator.Emit(new CstI(0));
                    generator.Emit(Instruction.Eq);
                    generator.Emit(Instruction.Not);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown binary operator: {0}", _op));
            }
            compilationEnvironment.PopTemporary();
        }
    }
}