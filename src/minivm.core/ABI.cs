using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    [Serializable]
    public class Method
    {
        /// <summary>
        /// Method signature such as `FMATH::SUM`
        /// </summary>
        public string signature;
        /// <summary>
        /// Instruction offset
        /// </summary>
        public int entry;
    }

    [Serializable]
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
        public static string Dictionary(string ident, object literal)
        {
            if (literal == null)
                return ident + "^null";
            return ident + $"^{literal.GetType().Name}_{literal.ToString()}";
        }
    }
}
