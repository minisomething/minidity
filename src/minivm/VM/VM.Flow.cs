using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
        private Dictionary<string, int> callTable = new Dictionary<string, int>();

        private void InitFlow()
        {
            Handle(Opcode.Abort, () =>
            {
                halt = true;
            });

            HandleWithOperand(Opcode.Cmp, (double x, double op) =>
            {
                if (x == op) ctx.state.Push(0);
                else if (x > op) ctx.state.Push(1);
                else if (x < op) ctx.state.Push(-1);
            });
            Handle(Opcode.G, (double x, double y) =>
            {
                if (x < y) ctx.state.Push(1);
                else ctx.state.Push(0);
            });
            Handle(Opcode.L, (double x, double y) =>
            {
                if (x > y) ctx.state.Push(1);
                else ctx.state.Push(0);
            });

            HandleWithOperand(Opcode.Call, (op) => {
                var p = callTable[op]; ;
                if (p == -1) PerformInternalCall(op);
                else
                {
                    ctx.callStack.PushCall(op);
                    ctx.instructionCursor = p;
                }
            });
            Handle(Opcode.Ret, () =>
            {
                if (ctx.callStack.count == 1) halt = true;
                else
                {
                    var cd = ctx.callStack.Pop();
                    ctx.instructionCursor = cd.retPoint;
                }
            });

            HandleWithOperand(Opcode.Jmp, x => ctx.instructionCursor = x);
            HandleWithOperand(Opcode.JmpG, (x, op) =>
            {
                if (x >= 1) ctx.instructionCursor = op;
            });
            HandleWithOperand(Opcode.JmpL, (x, op) =>
            {
                if (x <= -1) ctx.instructionCursor = op;
            });
            HandleWithOperand(Opcode.JmpEq, (x, op) =>
            {
                if (x == 0) ctx.instructionCursor = op;
            });
        }

        private void BuildCalltableFromAbi(ABI abi)
        {
            if (abi == null) return;

            foreach (var method in abi.methods)
                callTable[method.signature] = method.entry;
        }
    }
}
