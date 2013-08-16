using System;
using System.Collections.Generic;

namespace Expressions
{
    /// <summary>
    /// Type checking environments
    /// Map a variable name to a Type
    /// The environment is a stack because of a nested scopes
    /// </summary>
    public class TypeCheckingEnvironment
    {
        private readonly Stack<Tuple<string, Type>> _locals;

        public TypeCheckingEnvironment()
        {
            _locals = new Stack<Tuple<string, Type>>();
        }

        public void Push()
        {
            _locals.Push(new Tuple<string, Type>(null, null));
        }

        public void Pop()
        {
            _locals.Pop();
        }

        public void DeclareLocal(string name, Type type)
        {
            _locals.Push(new Tuple<string, Type>(name, type));
        }

        public Type GetVariable(string name)
        {
            foreach (var variable in _locals)
            {
                if (variable.Item1 == name)
                {
                    return variable.Item2;
                }
            }
            throw new InvalidOperationException("Unbound variable: " + name);
        }
    }
}