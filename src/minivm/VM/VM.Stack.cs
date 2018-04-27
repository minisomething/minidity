using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
        private void InitStack()
        {
            HandleWithOperandO(Opcode.Push, x => ctx.state.Push(x));
            Handle(Opcode.Pop, () => ctx.state.Pop());
        }
    }
}
