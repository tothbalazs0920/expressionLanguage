using System;
using System.Collections.Generic;

namespace Expressions
{
    /// <summary>
    /// Abstract syntax for function definitions.
    /// </summary>
    public class FunctionDefinition
    {
        private readonly String fName;
        private readonly List<Tuple<string, Type>> args;
        private readonly Expression body;
        public readonly Type returnType;

        public FunctionDefinition(Type returnType, String fName, List<Tuple<string, Type>> args, Expression body)
        {
            this.args = args;
            this.body = body;
            this.returnType = returnType;
            this.fName = fName;
        }

        public void Check(TypeCheckingEnvironment env, FunctionEnvironment fEnv)
        {
            foreach (var arg in args)
            {
                env.DeclareLocal(arg.Item1, arg.Item2);
            }
            Type t = body.Check(env, fEnv);
            foreach (var arg in args)
            {
                env.Pop();
            }
            if (t != returnType)
            {
                throw new InvalidOperationException("Body of " + fName + " returns " + t + ", " + returnType + " expected");
            }
        }


        public int Eval(RuntimeEnvironment env, FunctionEnvironment fenv, int[] values)
        {

            if (args.Count > 0)
            {
                int index = 0;
                foreach (Tuple<string, Type> tuple in args)
                {
                    
                    env.AllocateLocal(tuple.Item1);
                    env.GetVariable(tuple.Item1).Value = values[index++];
                }
                int v = body.Eval(env, fenv);
                foreach (Tuple<string, Type> tuple in args) { env.Pop(); }
                return v;
            }
            else
            {
                int v = body.Eval(env, fenv);
                return v;
            }
        }
       
        public bool CheckArgType(Type[] argTypes)
        {

            for (var i = 0; i < args.Count; i++)
            {
                if (args[i].Item2 != argTypes[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void Compile(CompilationEnvironment env, Generator gen)
        {

            foreach (Tuple<String, Type> arg in args)
            {
                env.DeclareLocal(arg.Item1);
            }

            gen.Label(env.GetFunctionLabel(fName));
            body.Compile(env, gen);
            gen.Emit(new Return(args.Count));
            /*
            env.DeclareLocal(formArg.Fst); // Formal argument name points to top of stack
            gen.Label(env.getFunctionLabel(fName));
            body.Compile(env, gen);
            gen.Emit(new RET(1));
            */
        }


    }
}