using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
        private Dictionary<string, Action> internalCalls;

        private void InitInternalCall()
        {
            callTable["Math.pow"] = -1;
            callTable["Console.print"] = -1;
            callTable["Chain.getBlockNo"] = -1;
            callTable["Chain.transfer"] = -1;

            callTable["tx.sender"] = -1;
            callTable["tx.value"] = -1;

            callTable["now"] = -1;
        }

        private void _RegisterInternalCall(string signature, Action callback)
        {
            if (string.IsNullOrEmpty(signature))
                throw new ArgumentException(nameof(signature));

            callTable[signature] = -1;
            internalCalls[signature] = callback;
        }
        public void RegisterInteranlCall(string signature, Action callback)
        {
            _RegisterInternalCall(signature, callback);
        }
        public void RegisterInternalCall<T1>(string signature, Action<T1> callback)
        {
            _RegisterInternalCall(signature, () =>
            {
                callback((T1)ctx.state.Pop());
            });
        }
        public void RegisterInternalCall<T1, T2>(string signature, Action<T1, T2> callback)
        {
            _RegisterInternalCall(signature, () =>
            {
                var a = (T2)ctx.state.Pop();
                var b = (T1)ctx.state.Pop();
                callback(b, a);
            });
        }
        public void RegisterInternalCall<T1, T2, T3>(string signature, Action<T1, T2, T3> callback)
        {
            _RegisterInternalCall(signature, () =>
            {
                var a = (T3)ctx.state.Pop();
                var b = (T2)ctx.state.Pop();
                var c = (T1)ctx.state.Pop();
                callback(c, b, a);
            });
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
                var amount = (udouble)ctx.state.PopDouble();
                var receiverAddress = (string)ctx.state.Pop();

                stateProvider.Transfer(receiverAddress, amount);
            }
            else if (signature == "Console.print")
            {
                var a = ctx.state.Pop();
                Console.WriteLine("PRINT: " + a);
            }
            else if (signature == "tx.sender")
            {
                ctx.state.Push(stateProvider.tx.senderAddress);
            }
            else if (signature == "tx.value")
            {
                ctx.state.Push(stateProvider.tx.value);
            }

            else if (signature == "now")
            {
                // [FIXME] FileTime -> UnixTime
                ctx.state.Push(DateTime.Now.ToFileTimeUtc());
            }
        }
    }
}
