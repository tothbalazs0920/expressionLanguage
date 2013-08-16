using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressions
{
    /// <summary>
    /// Programs abstract syntax. A Program consists of a term to be evaluated and a function environment in which it is evaluated
    /// </summary>
    public class Program
    {
        private readonly FunctionEnvironment _functionEnvironment;
        private readonly Expression _expression;

        public Program(IDictionary<string, FunctionDefinition> functions, Expression expression)
        {
            _functionEnvironment = new FunctionEnvironment(functions);
            _expression = expression;
        }

        public int Eval()
        {
            var runtimeEnvironment = new RuntimeEnvironment();
            return _expression.Eval(runtimeEnvironment, _functionEnvironment);
        }

        public Type Check()
        {
            var typeCheckingEnvironment = new TypeCheckingEnvironment();
            _functionEnvironment.Check(typeCheckingEnvironment, _functionEnvironment);
            return _expression.Check(typeCheckingEnvironment, _functionEnvironment);
        }

        public void Compile(Generator generator, string outputFile)
        {
            // Generate compiletime environment
            var labels = _functionEnvironment.GetFunctionNames().ToDictionary(funName => funName, funName => Label.Fresh());
            var compilationEnvironment = new CompilationEnvironment(labels);

            // Compile expression
            _expression.Compile(compilationEnvironment, generator);
            generator.Emit(Instruction.PrintI);
            generator.Emit(Instruction.Stop);

            // Compile functions
            foreach (var functionDefinition in _functionEnvironment.GetFunctions())
            {
                compilationEnvironment = new CompilationEnvironment(labels);
                functionDefinition.Compile(compilationEnvironment, generator);
            }

            //  Generate bytecode at and print to file
            generator.PrintCode();
            var bytecode = generator.ToBytecode();
            using (TextWriter writer = new StreamWriter(outputFile))
            {
                foreach (var b in bytecode)
                {
                    writer.Write(b);
                    writer.Write(" ");
                }
            }
        }
    }
}