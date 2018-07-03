using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace minidity
{
    public class Lexer
    {
        private static Dictionary<string, Tuple<TokenType, int>> table { get; set; }

        static Lexer()
        {
            table = new Dictionary<string, Tuple<TokenType, int>>();

            AddToken("\r\n", TokenType.None);
            AddToken(" ", TokenType.None);
            AddToken("	", TokenType.None);

            AddToken("->", TokenType.Keyword);

            AddToken("==", TokenType.Operator, 1);
            AddToken("=", TokenType.Operator);

            AddToken("+", TokenType.Operator, 2);
            AddToken("-", TokenType.Operator, 2);
            AddToken("*", TokenType.Operator, 4);
            AddToken("/", TokenType.Operator, 4);

            AddToken(">", TokenType.Operator, 1);
            AddToken("<", TokenType.Operator, 1);

            //AddToken("\"", TokenType.Quotes);

            AddToken(",", TokenType.Comma, -1000);
            AddToken(";", TokenType.Semicolon, -9999);
            AddToken("if", TokenType.Keyword);
            AddToken("else", TokenType.Keyword);
            AddToken("void", TokenType.Keyword);
            AddToken("var", TokenType.Keyword);
            AddToken("ret", TokenType.Keyword);
            AddToken("class", TokenType.Keyword);
            AddToken("def", TokenType.Keyword);
            AddToken("public", TokenType.Keyword);
            AddToken("private", TokenType.Keyword);
            AddToken("(", TokenType.LeftParen, -2000);
            AddToken(")", TokenType.RightParen);

            AddToken("{", TokenType.LeftBracket, -10000);
            AddToken("}", TokenType.RightBracket);

            AddToken("[", TokenType.LeftSquareBracket);
            AddToken("]", TokenType.RightSquareBracket, -1999);
        }
        private static void AddToken(string token, TokenType type, int priority = 0)
        {
            table.Add(token, new Tuple<TokenType, int>(type, priority));
        }

        private static TokenType GetTokenType(string token)
        {
            if (token.First() == '\"' && token.Last() == '\"')
                return TokenType.Literal;

            if (double.TryParse(token, out _))
                return TokenType.Literal;
            if (int.TryParse(token, out _))
                return TokenType.Literal;

            var ident = new Regex("[a-zA-Z_]+[a-zA-Z0-9_]*");
            if (ident.IsMatch(token))
                return TokenType.Ident;

            if (table.ContainsKey(token))
                return table[token].Item1;

            return TokenType.None;
        }

        private class LexerState
        {
            public string src;
            public int offset, cur;
            public int line;
        }

        private LexerState state;
        private List<Token> tokens;

        public static Token[] Parse(string src)
        {
            return new Lexer().ParseAll(src);
        }
        public static Token ParseOne(string str)
        {
            return new Lexer().ParseSingle(str);
        }

        private Lexer()
        {
            tokens = new List<Token>();
            state = new LexerState();
            state.line = 1;
        }
        private Token[] ParseAll(string src)
        {
            var current = new Token();
            src += " "; // padding
            state.src = src;

            while (state.cur != src.Length)
            {
                var found = false;

                foreach (var pair in table)
                {
                    if (state.cur + pair.Key.Length >= src.Length)
                        continue;
                    var candidate = src.Substring(state.cur, pair.Key.Length);
                    if (candidate == pair.Key)
                    {
                        Flush();
                        current.raw = candidate;
                        current.type = pair.Value.Item1;
                        current.priority = pair.Value.Item2;
                        AppendToken(current);
                        current = new Token();

                        state.cur += pair.Key.Length;
                        state.offset = state.cur;

                        found = true;
                        break;
                    }
                }

                if (found == false)
                    state.cur++;
            }

            return tokens.ToArray();
        }
        private Token ParseSingle(string str)
        {
            if (str.First() == '\"' && str.Last() == '\"')
            {
                return new Token()
                {
                    raw = str.Substring(1, str.Length - 2),
                    literalType = LiteralType.String,
                    type = TokenType.Literal
                };
            }

            var ident = new Regex("[a-zA-Z_]+[a-zA-Z0-9_]*");
            if (ident.IsMatch(str))
            {
                return new Token()
                {
                    raw = str,
                    type = TokenType.Ident
                };
            }
            // INT
            else if (int.TryParse(str, out _))
            {
                //if (UssValidator.IsValidInt(str) == false)
                //    throw new UssInvalidTokenException(str);

                return new Token()
                {
                    raw = str,
                    literalType = LiteralType.Integer,
                    type = TokenType.Literal
                };
            }
            // DOUBLE
            else if (double.TryParse(str, out _))
            {
                //if (UssValidator.IsValidFloat(str) == false)
                //    throw new UssInvalidTokenException(str);

                return new Token()
                {
                    raw = str,
                    literalType = LiteralType.Double,
                    type = TokenType.Literal
                };
            }
            // ID
            else
            {
                /*
                return new UssToken()
                {
                    body = str,
                    type = UssTokenType.Id
                };
                */
                return new Token() { type = TokenType.None };
            }
        }
        private bool Flush()
        {
            if (state.cur == state.offset)
                return false;

            var prev = ParseSingle(
                state.src.Substring(state.offset, (state.cur) - state.offset));
            if (prev != null)
            {
                AppendToken(prev);
                return true;
            }
            return false;
        }

        private void AppendToken(Token token)
        {
            if (token.type == TokenType.None) return;

            tokens.Add(InjectState(token));
        }
        private Token InjectState(Token token)
        {
            token.line = state.line;
            return token;
        }
    }
}
