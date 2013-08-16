using System;
using System.Collections.Generic;

namespace Expressions
{
    /// <summary>
    /// Function environment. Keeps track of the functions defined in a given context. 
    /// </summary>
    public class FunctionEnvironment
    {
        private readonly IDictionary<string, FunctionDefinition> _functions;

        public FunctionEnvironment(IDictionary<string, FunctionDefinition> functions)
        {
            _functions = functions;
        }

        public void Check(TypeCheckingEnvironment typeCheckingEnvironment, FunctionEnvironment functionEnvironment)
        {
            foreach (var f in _functions.Values)
            {
                f.Check(typeCheckingEnvironment, functionEnvironment);
            }
        }

        public FunctionDefinition GetFunction(string name)
        {
            if (_functions.ContainsKey(name))
            {
                return _functions[name];
            }
            throw new Exception("Undefined Function " + name);
        }

        public ICollection<FunctionDefinition> GetFunctions()
        {
            return new List<FunctionDefinition>(_functions.Values);
        }

        public ICollection<string> GetFunctionNames()
        {
            return new List<string>(_functions.Keys);
        }
    }
}