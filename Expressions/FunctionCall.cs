using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressions
{
    /// <summary>
    /// Abstract syntax for a function call expression
    /// </summary>
    public class FunctionCall : Expression
    {
        private readonly String fName;
        private readonly List<Expression> _args;

        public FunctionCall(String fName, List<Expression> args)
        {
            this.fName = fName;
            _args = args;
        }

      
      public override Type Check(TypeCheckingEnvironment env, FunctionEnvironment fenv){
	  
	  Type[] parameterTypes = new Type[_args.Count];
	  int index = 0;
	  
	  FunctionDefinition fDef = fenv.GetFunction(fName);
	  
	  foreach (Expression expression in _args) {
	    parameterTypes[index++] = expression.Check(env, fenv);
	  }
	  
	  if (fDef.CheckArgType(parameterTypes))
		return fDef.returnType;
	  else 
		throw new InvalidOperationException("Type error in call of function " + fName);
	}


     public override int Eval(RuntimeEnvironment env, FunctionEnvironment fenv)
        {
            int[] values;
            if (_args.Count > 0)
            {
                values = new int[_args.Count];
                int index = 0;
                foreach (Expression e in _args)
                {
                    values[index++] = e.Eval(env, fenv);
                }
            }
            else
            {
                values = new int[0];
            }
            FunctionDefinition fDef = fenv.GetFunction(fName);
            int result = fDef.Eval(env, fenv, values);
            return result;
        }

        public override void Compile(CompilationEnvironment env, Generator gen)
        {
            foreach (Expression arg in _args)
            {
                arg.Compile(env, gen);
                env.PushTemporary();
            }

            String fLabel = env.GetFunctionLabel(fName);
            gen.Emit(new Call(_args.Count, fLabel));

            foreach (var arg in _args)
            {
                env.PopTemporary();
            }
            /*
            arg.Compile(env, gen);
            String fLabel = env.getFunctionLabel(fName);
            gen.Emit(new CALL(1, fLabel));
              */
        }
    }

}