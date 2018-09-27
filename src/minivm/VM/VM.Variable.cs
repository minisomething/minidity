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
            HandleWithOperand(Opcode.Ststate2, (op) =>
            {
                var dictKey = ctx.state.Pop().ToString();
                var value = ctx.state.Pop();
                stateProvider.SetState(ABISignature.Dictionary(op, dictKey), value);
            });
            HandleWithOperand(Opcode.Ldstate, (op) => ctx.state.Push(stateProvider.GetState(op)));
            HandleWithOperand(Opcode.Ldstate2, (op) => {
                var dictKey = ctx.state.Pop().ToString();

                ctx.state.Push(stateProvider.GetState(ABISignature.Dictionary(op, dictKey)));
            });
        }
    }
}
