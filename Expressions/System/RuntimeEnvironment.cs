using System;
using System.Collections.Generic;

namespace Expressions
{
    /// <summary>
    /// Runtime environments
    /// Map a variable name to a Storage object that can hold an int
    /// The environment is a stack because of nested scopes
    /// </summary>
    public class RuntimeEnvironment
    {
        private readonly Stack<Tuple<string, Storage>> _locals;

        public RuntimeEnvironment()
        {
            _locals = new Stack<Tuple<string, Storage>>();
        }

        // Find variable in innermost local scope
        public Storage GetVariable(string name)
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

        // Allocate variable
        public void AllocateLocal(string name)
        {
            _locals.Push(Tuple.Create(name, new Storage()));
        }

        public void Pop()
        {
            _locals.Pop();
        }
    }
}