using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class MinidityProgram
    {
        public Instruction[] instructions;
        public ABI abi;
    }
    public class MinidityCompiler
    {
        public static RootNode Compile(string src)
        {
            var root =
                TreeBuilder.Build(
                    Sexper.SexpPrefix(
                        Lexer.Parse(src)));

            return root;
        }

        /// <summary>
        /// Builds program and ABI from given SyntaxRoot.
        /// </summary>
        /// <param name="root">Syntax Root</param>
        public static MinidityProgram BuildProgram(RootNode root)
        {
            var emitter = new Emitter();
            var ctx = new BuildContext();
            root.Emit(ctx, emitter);

            return new MinidityProgram()
            {
                abi = emitter.GetABI(),
                instructions = emitter.GetInstructions()
            };
        }
        /// <summary>
        /// Builds program and ABI from given code.
        /// </summary>
        /// <param name="src">Minidity code</param>
        public static MinidityProgram BuildProgram(string src)
        {
            var root = Compile(src);

            root.Print();
            return BuildProgram(root);
        }
    }
}
