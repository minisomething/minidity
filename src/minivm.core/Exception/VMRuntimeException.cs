using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public class VMRuntimeException : Exception
    {
        public VMRuntimeException(string msg) : base(msg)
        {
        }
    }
}
