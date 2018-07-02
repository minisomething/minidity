using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class Token
    {
        public TokenType type { get; set; }
        public LiteralType literalType { get; set; }
        public STokenType stype { get; set; }

        public string raw { get; set; }
        public int priority { get; set; }

        public int line;

        public Token()
        {
            stype = STokenType.Operator;
        }
    }

    public class SToken
    {
        public STokenType type { get; set; }
        public LiteralType literalType { get; set; }

        public string raw { get; set; }
    }
}
