using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public enum TokenType
    {
        None,
        Ident,
        Quotes,
        Literal,
        Keyword,
        Operator,
        LeftParen, RightParen,
        LeftBracket, RightBracket,
        Colon,
        Semicolon,
        Comma,
        RetType
    }

    public enum LiteralType
    {
        String,
        Integer,
        Double
    }

    public enum STokenType
    {
        None,

        Endl,

        Call,
        EndCall,
        Ident,
        Literal,

        Class,
        Method,

        Public, Private,

        Param,
        EndParam,

        BeginBlock,
        EndBlock,

        Ret,
        RetType,
        If,
        Else,

        Operator,
    }
}
