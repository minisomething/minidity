using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    internal class ExeContext
    {
        public Instruction current;

        public Instruction[] instructions;
        public int instructionCursor;

        public VMState state;
        public CallStack callStack;

        public ExeContext(Instruction[] _instructions)
        {
            instructions = _instructions;

            state = new VMState();
            callStack = new CallStack(this);
        }
    }
}
