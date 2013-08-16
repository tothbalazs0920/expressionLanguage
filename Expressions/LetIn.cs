using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expressions
{
    public class LetIn : Expression
    {
        private readonly string name;
        private readonly Expression e1;
        private readonly Expression e2;



        public LetIn(string name, Expression e1, Expression e2)
        {
            this.name = name;
            this.e1 = e1;
            this.e2 = e2;

        }

        public override int Eval(RuntimeEnvironment env, FunctionEnvironment fEnv)
        {
            int value = e1.Eval(env, fEnv);
            env.AllocateLocal(name);                        // It pushes a new Pair<String, Storage>(name, new Storage()) to the stack called locals. The Storage value is 0 or null the String name is the type the storage.value is the value.    

            Storage storage = env.GetVariable(name);     // this method returns a Storage object. The storage variable that we create here, is pointing to the same object as the other Storage variable in the Stack<Pair<String, Storage> locals.
            storage.Value = value;                      // The Storage has only a value field. If we modify the value of this Storage variable, the value of the object will be modofied.

            int result = e2.Eval(env, fEnv);
            env.Pop();

            return result;
        }

        public override Type Check(TypeCheckingEnvironment env, FunctionEnvironment fEnv)
        {
            Type t1 = e1.Check(env, fEnv);
            env.DeclareLocal(name, t1);
            Type t2 = e2.Check(env, fEnv);
            env.Pop();
            return t2;
        }


        public override void Compile(CompilationEnvironment env, Generator gen)
        {

            e1.Compile(env, gen);
            env.DeclareLocal(name);             // push the name variable to the stack<string> locals of CEnv

            //env.CompileVariable(gen, name); // get the array index of the variable name
            //gen.Emit(Instruction.LdI);      // load the variable name with the index which is on the top of the stack

            e2.Compile(env, gen);
            gen.Emit(Instruction.Swap);
            gen.Emit(new IncSp(-1));

            env.Pop();    // deallocate variable name

        }


    }

}
