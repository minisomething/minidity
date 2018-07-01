using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
        private void InitVariable()
        {
            HandleWithOperand(Opcode.Stloc, (x, op) => ctx.callStack.SetLocal(op, x));
            HandleWithOperand(Opcode.Ldloc, (op) => ctx.state.Push(ctx.callStack.GetLocal(op)));

            HandleWithOperand(Opcode.Ststate, (x, op) => stateProvider.SetState(op, x));
            HandleWithOperand(Opcode.Ldstate, (op) => ctx.state.Push(stateProvider.GetState(op)));
        }
    }
}
