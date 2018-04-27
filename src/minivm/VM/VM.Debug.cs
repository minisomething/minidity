using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
    {
        private void InitDebug()
        {
            HandleO(Opcode.Print, x => Console.WriteLine(x));
        }
    }
}
