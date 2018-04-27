using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    [Serializable]
    public class Instruction
    {
        public Opcode code;
        public object operand;

        public void SetOperand(int n)
        {
            operand = n;
        }
    }
}
