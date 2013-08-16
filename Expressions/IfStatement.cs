using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expressions
{
     public class IfStatement : Expression
    {

        private readonly Expression e1;
        private readonly Expression e2;
        private readonly Expression e3;

       

        public IfStatement(Expression e1, Expression e2, Expression e3)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.e3 = e3;
        }

        public override int Eval(RuntimeEnvironment env, FunctionEnvironment fEnv)
        {


            if (e1.Eval(env, fEnv) == 1) return e2.Eval(env, fEnv);
            else return e3.Eval(env, fEnv);


        }

        public override Type Check(TypeCheckingEnvironment env, FunctionEnvironment fEnv)
        {


            Type t1 = e1.Check(env, fEnv);
            Type t2 = e2.Check(env, fEnv);
            Type t3 = e3.Check(env, fEnv);

            if (t1 != Type.BooleanType) throw new InvalidOperationException("condition must be boolean");
            if (t2 != t3) throw new InvalidOperationException("the two types must be the same");
            return t2;

              
        }

        public override void Compile(CompilationEnvironment env, Generator gen) {
            var l1 = Label.Fresh();
            var l2 = Label.Fresh();

            e1.Compile(env, gen);
            gen.Emit(new IfZero(l1));
            e2.Compile(env, gen);           // inside if
            gen.Emit(new Goto(l2));
            //generate flash labels: Label.Fresh()
            gen.Label(l1);    // add labels to the stack machine  L1 -else 
            e3.Compile(env, gen);        // inside else

            gen.Label(l2);    // L2
        
        
        }

    }
    }

