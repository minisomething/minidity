using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public class Method
    {
        public string signature;
        public int entry;
    }

    public class ABI
    {
        public string[] contracts;
        public Method[] methods;
    }

    public class ABISignature
    {
        public static string Method(string contract, string method)
        {
            return contract + "::" + method;
        }
        public static string Field(string contract, string field)
        {
            return contract + "::" + field;
        }
    }
}
