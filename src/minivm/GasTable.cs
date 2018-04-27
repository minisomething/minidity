using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public class GasTable
    {
        public static int GetGasPrice(Opcode opcode)
        {
            if (opcode == Opcode.Ststate ||
                opcode == Opcode.Ldstate)
                return 10;

            return 1;
        }
    }
}
