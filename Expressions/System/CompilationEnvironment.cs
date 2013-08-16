using System;
using System.Collections.Generic;

namespace Expressions
{
    /// <summary>
    /// Compilation environments
    /// An implicit map from string to offset (distance from stack top)
    /// The environment is a stack because of nested scopes
    /// </summary>
    public class CompilationEnvironment
    {
        private readonly Stack<string> _locals;
        private readonly IDictionary<string, string> _labels;

        public CompilationEnvironment(IDictionary<string, string> labels)
        {
            if (labels == null)
            {
                throw new ArgumentNullException("labels");
            }
            _locals = new Stack<string>();
            _labels = labels;
        }

        public void Pop()
        {
            _locals.Pop();
        }

        public void DeclareLocal(string name)
        {
            _locals.Push(name);
        }

        public void PushTemporary()
        {
            _locals.Push("_ temporary _");
        }

        public void PopTemporary()
        {
            var s = _locals.Pop();
            if (s != "_ temporary _")
            {
                throw new InvalidOperationException("Internal problem: popping non-temporary");
            }
        }

        public void CompileVariable(Generator gen, string name)
        {
            var offset = 0;
            foreach (var variableName in _locals)
            {
                if (variableName == name)
                {
                    gen.Emit(Instruction.GetSp);
                    gen.Emit(new CstI(offset));
                    gen.Emit(Instruction.Sub);
                    return;
                }
                offset++;
            }
            throw new InvalidOperationException("Undeclared variable: " + name);
        }

        public string GetFunctionLabel(string funName)
        {
            if (_labels.ContainsKey(funName))
            {
                return _labels[funName];
            }
            throw new InvalidOperationException("Internal error: Undefined function " + funName);
        }
    }
}