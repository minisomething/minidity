using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using LLVMSharp;

namespace minivm
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int ContractEntry();

    /// <summary>
    /// Experimental and UNIMPLEMENTED
    /// </summary>
    public class JitVM
    {
        private LLVMValueRef gasRef;
        private LLVMBasicBlockRef checkGasFunc;
        private LLVMBasicBlockRef outOfGasBlock;
        private LLVMValueRef func;

        private BasicBlock[] CreateBlocks(LLVMValueRef func, Instruction[] instructions)
        {
            var blocks = new List<BasicBlock>();
            var offset = 0;

            for(int i = 0; i < instructions.Length; i++)
            {
                var inst = instructions[i];

                if (i == instructions.Length-1 ||
                    inst.code == Opcode.Jmp ||
                    inst.code == Opcode.JmpEq || 
                    inst.code == Opcode.JmpG ||
                    inst.code == Opcode.JmpGE ||
                    inst.code == Opcode.JmpL ||
                    inst.code == Opcode.JmpLE)
                {
                    var block = new BasicBlock(
                        func, instructions.Skip(offset).Take(i - offset + 1).ToArray());
                    blocks.Add(block);

                    offset = i + 1;
                }
            }

            return blocks.ToArray();
        }
        private void CompileBlock(LLVMBuilderRef builder, BasicBlock block)
        {
            LLVM.PositionBuilderAtEnd(builder, block.block);

            var stack = new Stack<LLVMValueRef>();

            foreach (var inst in block.instructions)
            {
                Console.WriteLine(inst.code);

                var price = GasTable.GetGasPrice(inst.code);
                LLVM.BuildCall(builder, checkGasFunc,
                    new LLVMValueRef[] { gasRef, GetLLVMValue(price) },
                    "");

                if (inst.code == Opcode.Add)
                {
                    var a = stack.Pop();
                    var b = stack.Pop();
                    stack.Push(LLVM.BuildAdd(builder, a, b, ""));
                }
                else if (inst.code == Opcode.Sub)
                {
                    var a = stack.Pop();
                    var b = stack.Pop();
                    stack.Push(LLVM.BuildSub(builder, a, b, ""));
                }
                else if (inst.code == Opcode.Mul)
                {
                    var a = stack.Pop();
                    var b = stack.Pop();
                    stack.Push(LLVM.BuildMul(builder, a, b, ""));
                }
                else if (inst.code == Opcode.Div)
                {
                    var a = stack.Pop();
                    var b = stack.Pop();

                    var divByZero = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, b, GetLLVMValue(0), "");
                    b = LLVM.BuildSelect(builder, divByZero, GetLLVMValue(1), b, "");
                    var ret = LLVM.BuildUDiv(builder, a, b, "");
                    ret = LLVM.BuildSelect(builder, divByZero, GetLLVMValue(0), ret, "");
                    stack.Push(ret);
                }

                else if (inst.code == Opcode.Push)
                    stack.Push(GetLLVMValue(inst.operand));
                else if (inst.code == Opcode.Pop)
                    stack.Pop();

                else if (inst.code == Opcode.Jmp)
                {

                }

                else if (inst.code == Opcode.Ret)
                {
                    var v = stack.Pop();
                    
                    LLVM.BuildRet(builder, LLVM.BuildLoad(builder, gasRef, "ldgas"));
                }
            }
        }

        private LLVMValueRef CreateCheckGasFunc(LLVMBuilderRef builder, LLVMModuleRef mod)
        {
            var param_types = new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int32Type(), 0), LLVM.Int32Type() };
            var ret_type = LLVM.FunctionType(LLVM.VoidType(), param_types, false);
            var func = LLVM.AddFunction(mod, "checkGas", ret_type);
            var entry = LLVM.AppendBasicBlock(func, "entry");

            LLVM.PositionBuilderAtEnd(builder, entry);
            var price = LLVM.GetParam(func, 0);
            var gas = LLVM.BuildLoad(builder, LLVM.GetParam(func, 0), "ld.gas");
            var sub = LLVM.BuildSub(builder, gas, LLVM.GetParam(func, 1), "subGas");
            LLVM.BuildStore(builder, sub, LLVM.GetParam(func, 0));

            var thenBlock = LLVM.AppendBasicBlock(func, "then");
            var elseBlock = LLVM.AppendBasicBlock(func, "else");
            var ifGasExceed = LLVM.BuildICmp(
                builder, LLVMIntPredicate.LLVMIntSLE, gas, GetLLVMValue(0), "if(gas <= 0");
            LLVM.BuildCondBr(builder, ifGasExceed, thenBlock, elseBlock);

            LLVM.PositionBuilderAtEnd(builder, thenBlock);
            LLVM.BuildBr(builder, outOfGasBlock);
            //LLVM.BuildRetVoid(builder);

            LLVM.PositionBuilderAtEnd(builder, elseBlock);
            LLVM.BuildRetVoid(builder);

            return func;
        }

        public object Execute(ABI abi, byte[] instruction, int gasLimit, out int gasUsed)
        {
            return Execute(abi, BConv.FromBytes(instruction), gasLimit, out gasUsed);
        }
        public object Execute(ABI abi, Instruction[] instructions, int gasLimit, out int gasUsed)
        {
            var Success = new LLVMBool(0);
            var mod = LLVM.ModuleCreateWithName("jitvm");

            var builder = LLVM.CreateBuilder();

            var param_types = new LLVMTypeRef[] { };
            var ret_type = LLVM.FunctionType(LLVM.Int32Type(), param_types, false);
            func = LLVM.AddFunction(mod, "func", ret_type);
            var entry = LLVM.AppendBasicBlock(func, "entry");

            gasUsed = 0;

            outOfGasBlock = LLVM.AppendBasicBlock(func, "outOfGas");
            LLVM.PositionBuilderAtEnd(builder, outOfGasBlock);
            LLVM.BuildRet(builder, GetLLVMValue(-1));

            LLVM.PositionBuilderAtEnd(builder, entry);
            gasRef = LLVM.BuildAlloca(builder, LLVM.Int32Type(), "alloc.gas");
            LLVM.BuildStore(builder, GetLLVMValue(gasLimit), gasRef);

            checkGasFunc = CreateCheckGasFunc(builder, mod);

            var blocks = CreateBlocks(func, instructions);
            foreach (var block in blocks)
                CompileBlock(builder, block);

            LLVM.PositionBuilderAtEnd(builder, entry);
            LLVM.BuildBr(builder, blocks.First().block);

            if (LLVM.VerifyModule(mod, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error) != Success)
            {
                Console.WriteLine($"Error: {error}");
                return null;
            }

            LLVM.LinkInMCJIT();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86AsmParser();
            LLVM.InitializeX86AsmPrinter();

            LLVMMCJITCompilerOptions options = new LLVMMCJITCompilerOptions { NoFramePointerElim = 1 };
            LLVM.InitializeMCJITCompilerOptions(options);
            if (LLVM.CreateMCJITCompilerForModule(out var engine, mod, options, out error) != Success)
            {
                Console.WriteLine($"Error: {error}");
                return null ;
            }

            var managedMethod = (ContractEntry)Marshal.GetDelegateForFunctionPointer(
                LLVM.GetPointerToGlobal(engine, func), typeof(ContractEntry));
            int result = managedMethod();

            Console.WriteLine(result);

            return result;
        }

        private LLVMValueRef GetLLVMValue(object o)
        {
            if (o is LLVMValueRef lvv)
                return lvv;
            
            if (o is int i)
                return LLVM.ConstInt(LLVM.Int32Type(), (ulong)i, true);

            if (o is double v)
                return LLVM.ConstReal(LLVM.DoubleType(), v);
            
            if (o is string s)
                return LLVM.ConstString(s, (uint)s.Length, false);

            throw new ArgumentException(nameof(o));
        }
    }
}
