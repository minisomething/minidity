using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
        private void InitEvent()
        {
            HandleWithOperandO(Opcode.EmitEvent, (op) =>
            {
                var numArgs = (int)op;
                var args = new List<object>();

                for (int i = 0; i < numArgs - 1; i++)
                    args.Add(ctx.state.Pop());
                var ident = (string)ctx.state.Pop();

                args.Reverse();
                ctx.events.Add(new EventData()
                {
                    ident = ident,
                    args = args.ToArray()
                });
            });
        }
    }
}
