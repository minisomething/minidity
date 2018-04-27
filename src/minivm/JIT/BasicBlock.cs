using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LLVMSharp;

namespace minivm
{
    internal class BasicBlock
    {
        public Instruction[] instructions;
        public LLVMBasicBlockRef block;

        public BasicBlock(LLVMValueRef func, Instruction[] _instructions)
        {
            instructions = _instructions;

            block = LLVM.AppendBasicBlock(func, "_zs");
        }
    }
}
