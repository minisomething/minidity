using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
		private void InitOperator()
        {
            Handle(Opcode.Add, (x, y) => ctx.state.Push(y + x));
            Handle(Opcode.Sub, (x, y) => ctx.state.Push(y - x));
            Handle(Opcode.Mul, (x, y) => ctx.state.Push(y * x));
            Handle(Opcode.Div, (x, y) => ctx.state.Push(y / x));

            Handle(Opcode.Inc, x => ctx.state.Push(x + 1));
            Handle(Opcode.Dec, x => ctx.state.Push(x - 1));
        }
    }
}
