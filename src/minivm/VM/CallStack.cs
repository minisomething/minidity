using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    internal class CallStack
    {
        internal class CallData
        {
            public string signature;
            public Dictionary<string, object> locals = new Dictionary<string, object>();

            public int retPoint;
        }

        public int count => stack.Count;

        private ExeContext ctx;

        private Stack<CallData> stack = new Stack<CallData>();
        private Stack<object> argStack = new Stack<object>();

        public CallStack(ExeContext _ctx)
        {
            ctx = _ctx;
        }

        public void PushArg(object obj)
        {
            argStack.Push(obj);
        }
        public void PushCall(string signature)
        {
            var cd = new CallData()
            {
                signature = signature,
                retPoint = ctx.instructionCursor
            };

            stack.Push(cd);
        }
        public CallData Pop()
        {
            return stack.Pop();
        }

        public object GetLocal(string key)
        {
            var locals = stack.Peek().locals;

            if (locals.ContainsKey(key))
                return locals[key];
            
            throw new InvalidOperationException(key);
        }
        public void SetLocal(string key, object value)
        {
            var locals = stack.Peek().locals;

            locals[key] = value;
        }
    }
}
