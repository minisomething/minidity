using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    internal class VMState
    {
        private Stack<object> stack = new Stack<object>();

        public int count => stack.Count;

        public void Push(object o)
        {
            stack.Push(o);
        }
        public object Pop()
        {
            return stack.Pop();
        }
        public double PopDouble()
        {
            return Convert.ToDouble(Pop());
        }
    }
}
