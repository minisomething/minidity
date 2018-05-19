using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
        private void InitInternalCall()
        {
            callTable["Math.pow"] = -1;
            callTable["Console.print"] = -1;
            callTable["Chain.getBlockNo"] = -1;
            callTable["Chain.transfer"] = -1;
        }

        private void PerformInternalCall(string signature)
        {
            if (signature == "Math.pow")
            {
                var a = ctx.state.PopDouble();
                var b = ctx.state.PopDouble();

                ctx.state.Push(MMath.Pow(b, a));
            }
            else if (signature == "Chain.getBlockNo")
            {
                ctx.state.Push(stateProvider.blockNo);
            }
            else if (signature == "Chain.transfer")
            {
                var a = (string)ctx.state.Pop();
                var b = (udouble)ctx.state.Pop();

                stateProvider.Transfer(a, b);
            }
            else if (signature == "Console.print")
            {
                var a = ctx.state.Pop();

                Console.WriteLine("PRINT: " + a);
            }
        }
    }
}
