using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    // S-Expression
    public class Sexper
    {
        private List<SToken> stokens;
        private Stack<Token> stack;

        public static SToken[] SexpPrefix(Token[] _tokens)
        {
            var tokens = new List<Token>(_tokens);

            foreach (var t in tokens)
                Console.WriteLine(t.type + " / " + t.raw);

            tokens.Reverse();

            if (tokens[0].type == TokenType.Semicolon)
            {
                tokens.Add(new Token()
                {
                    raw = ";",
                    type = TokenType.Semicolon
                });
            }

            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.LeftParen)
                    tokens[i] = new Token()
                    {
                        type = TokenType.RightParen,
                        raw = ")"
                    };
                else if (tokens[i].type == TokenType.RightParen)
                    tokens[i] = new Token()
                    {
                        type = TokenType.LeftParen,
                        raw = "(",
                        priority = -1500
                    };
                else if (tokens[i].type == TokenType.RightBracket)
                {
                    tokens[i].type = TokenType.LeftBracket;
                    tokens[i].priority = -10000;
                    tokens[i].raw = "{";
                }
                else if (tokens[i].type == TokenType.LeftBracket)
                {
                    tokens[i].type = TokenType.RightBracket;
                    tokens[i].priority = -10000;
                    tokens[i].raw = "}";
                }
            }


            Console.WriteLine("BEGIN TT");
            foreach (var token in tokens)
                Console.Write(token.raw + " ");
            Console.WriteLine("END TT");

            // (1, 2)foo -> foo(1, 2)
            for (var i = 1; i < tokens.Count - 1; i++)
            {
                if (tokens[i].type == TokenType.RightParen &&
                   (tokens[i + 1].type == TokenType.Ident ||
                    tokens[i + 1].type == TokenType.Keyword))
                {
                    var depth = 0;
                    for (var j = i; j >= 0; j--)
                    {
                        if (tokens[j].type == TokenType.RightParen)
                            depth += 1;
                        else if (tokens[j].type == TokenType.LeftParen)
                        {
                            depth -= 1;

                            if (depth == 0)
                            {
                                var func = tokens[i + 1];
                                var paren = tokens[i];

                                tokens.RemoveAt(i + 1);
                                tokens.Insert(j, func);

                                break;
                            }
                        }
                    }

                    i++;
                }
            }

             Console.WriteLine("BEGIN TT");
             foreach (var token in tokens)
             Console.Write(token.raw + " ");
             Console.WriteLine("END TT");

            var sexp = Sexp(tokens);
            sexp.Reverse();

            return sexp.ToArray();
        }
        private static List<SToken> Sexp(List<Token> tokens)
        {
            return new Sexper()._Sexp(tokens);
        }

        public Sexper()
        {
            stokens = new List<SToken>();
            stack = new Stack<Token>();
        }
        private List<SToken> _Sexp(List<Token> tokens)
        { 
            var innerMethod = 2;
            var depth = 0;

            Console.WriteLine("------BEGIN SEXP-------");
            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];
                var nextToken = i + 1 == tokens.Count ? null : tokens[i + 1];

                Console.WriteLine(token.raw + " " + token.type.ToString());

                if (token.type == TokenType.Literal)
                {
                    stokens.Add(new SToken()
                    {
                        type = STokenType.Literal,
                        raw = token.raw
                    });
                }
                else if (token.type == TokenType.Ident)
                {
                    if (nextToken?.type == TokenType.LeftParen)
                    {
                        if (depth >= innerMethod)
                        {
                            stack.Push(token);

                            stokens.Add(new SToken()
                            {
                                type = STokenType.EndCall,
                                raw = "ENDCALL"
                            });
                        }
                        else
                        {
                            stack.Push(token);

                            stokens.Add(new SToken()
                            {
                                type = STokenType.EndParam,
                                raw = "ENDPARAM"
                            });
                        }
                    }
                    else
                    {
                        stokens.Add(new SToken()
                        {
                            type = STokenType.Ident,
                            raw = token.raw
                        });
                    }
                }
                else if (
                    token.type == TokenType.Semicolon ||
                    token.type == TokenType.Comma)
                {
                    Console.WriteLine("SEMICOLON " + token.priority);
                    while (stack.Count > 0)
                    {
                        Console.WriteLine($"COM  {stack.Peek().raw} / {stack.Peek().priority}");
                        if (stack.Peek().priority <= token.priority)
                            break;

                        var t = stack.Pop();
                        Console.WriteLine("ADD " + t.raw);
                        stokens.Add(new SToken()
                        {
                            type = STokenType.Operator,
                            raw = t.raw
                        });
                    }

                    stokens.Add(new SToken()
                    {
                        type = STokenType.Endl,
                        raw = token.raw
                    });
                }
                else if (token.type == TokenType.Operator)
                {
                    while (true)
                    {
                        if (stack.Count == 0)
                            break;

                        if (stack.Peek().priority <= token.priority)
                            break;

                        var t = stack.Pop();
                        stokens.Add(new SToken()
                        {
                            type = STokenType.Operator,
                            raw = t.raw
                        });

                    }
                    Console.WriteLine("ADD  " + token.raw);
                    stack.Push(token);
                }
                else if (token.type == TokenType.LeftBracket)
                {
                    depth++;

                    while (stack.Count > 0)
                    {
                        if ((stack.Peek().type == TokenType.LeftBracket))
                        {
                            stack.Pop();
                            break;
                        }

                        var t = stack.Pop();
                        stokens.Add(new SToken()
                        {
                            type = t.stype,
                            raw = t.raw
                        });
                    }

                    stokens.Add(new SToken()
                    {
                        type = STokenType.EndBlock,
                        raw = "}"
                    });
                    stack.Push(token);
                }
                else if (token.type == TokenType.RightBracket)
                {
                    depth--;

                    while (stack.Count > 0)
                    {
                        if ((stack.Peek().type == TokenType.LeftBracket))
                        {
                            //Console.WriteLine("END FLUSH");
                            stack.Pop();
                            break;
                        }

                        //Console.WriteLine("BEGIN FLUSH");

                        //Console.WriteLine("End Flush");

                        var t = stack.Pop();

                        //Console.WriteLine(t.type.ToString());

                        stokens.Add(new SToken()
                        {
                            type = t.stype,
                            raw = t.raw
                        });
                    }
                    stokens.Add(new SToken()
                    {
                        type = STokenType.BeginBlock,
                        raw = "{"
                    });
                }
                else if (token.type == TokenType.LeftParen)
                {
                    stack.Push(token);
                }
                else if (token.type == TokenType.RightParen)
                {
                    while (stack.Count > 0)
                    {
                        if ((stack.Peek().type == TokenType.LeftParen))
                        {
                            stack.Pop(); // LeftParen
                            var tt = stack.Pop();

                            if (depth >= innerMethod)
                            {
                                if (tt.raw == "if")
                                {
                                    stokens.Add(new SToken()
                                    {
                                        type = STokenType.If,
                                        raw = tt.raw
                                    });
                                }
                                else if (tt.type == TokenType.Ident)
                                {
                                    stokens.Add(new SToken()
                                    {
                                        type = STokenType.Call,
                                        raw = tt.raw
                                    });
                                }
                            }
                            else
                            {
                                stokens.Add(new SToken()
                                {
                                    type = STokenType.Param,
                                    raw = "BEGIN_PARAM"
                                });
                                stokens.Add(new SToken()
                                {
                                    type = STokenType.Ident,
                                    raw = tt.raw
                                });
                            }

                            break;
                        }

                        var t = stack.Pop();
                        stokens.Add(new SToken()
                        {
                            type = t.stype,
                            raw = t.raw
                        });
                    }
                }
                else if (token.type == TokenType.RetType)
                {
                }
                else if (token.type == TokenType.Keyword)
                {
                    while (true)
                    {
                        if (stack.Count == 0)
                            break;
                        if (stack.Peek().priority <= token.priority)
                            break;

                        var t = stack.Pop();
                        stokens.Add(new SToken()
                        {
                            type = t.stype,
                            raw = t.raw
                        });
                    }

                    switch (token.raw)
                    {
                        case "ret":
                            stokens.Add(new SToken()
                            {
                                type = STokenType.Ret,
                                raw = token.raw
                            });
                            break;
                        case "class":
                            stokens.Add(new SToken()
                            {
                                type = STokenType.Class,
                                raw = token.raw
                            });
                            break;
                        case "if":
                            if (nextToken?.type == TokenType.LeftParen)
                            {
                                if (depth >= innerMethod)
                                    stack.Push(token);
                            }
                            else
                            {
                                stokens.Add(new SToken()
                                {
                                    type = STokenType.If,
                                    raw = token.raw
                                });
                            }

                            
                            break;
                        case "else":
                            stokens.Add(new SToken()
                            {
                                type = STokenType.Else,
                                raw = token.raw
                            });
                            break;
                        case "def":
                            stokens.Add(new SToken()
                            {
                                type = STokenType.Method,
                                raw = token.raw
                            });
                            break;
                        case "public":
                            stokens.Add(new SToken()
                            {
                                type = STokenType.Public,
                                raw = token.raw
                            });
                            break;
                        case "private":
                            stokens.Add(new SToken()
                            {
                                type = STokenType.Private,
                                raw = token.raw
                            });
                            break;
                    }
                }
            }
            Console.WriteLine("---------END SEXP--------");
            //Console.WriteLine();

            return stokens;
        }
    }
}
