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
        public static MinidityProgram Compile(string src)
        {
            var root =
                TreeBuilder.Build(
                    Sexper.SexpPrefix(
                        Lexer.Parse(src)));

            root.Print();
            var emitter = new Emitter();
            var ctx = new BuildContext();
            root.Emit(ctx, emitter);

            return new MinidityProgram()
            {
                abi = emitter.GetABI(),
                instructions = emitter.GetInstructions()
            };
        }
    }
}
