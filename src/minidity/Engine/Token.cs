using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class Token
    {
        /// <summary>
        /// Lex token type
        /// </summary>
        public TokenType type { get; set; }
        /// <summary>
        /// Literal type (Only valid if type is .Literal)
        /// </summary>
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
        /// <summary>
        /// Sexp token type
        /// </summary>
        public STokenType type { get; set; }
        /// <summary>
        /// Literal type (Only valid if type is .Literal)
        /// </summary>
        public LiteralType literalType { get; set; }

        public string raw { get; set; }
    }
}
