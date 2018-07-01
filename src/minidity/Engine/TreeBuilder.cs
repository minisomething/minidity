using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class TreeBuilder
    {
        public static RootNode Build(SToken[] tokens)
        {
            var root = new RootNode(null);
            ClassNode currentClass = null;
            SyntaxNode current = root;

            SToken prevToken = null;
            foreach (var token in tokens)
            {
                //Console.WriteLine($"{current.GetType()} : " + token.raw + " " + token.type.ToString());
                Console.WriteLine(token.raw + " / " + token.type);

                if (token.type == STokenType.Operator)
                {
                    SyntaxNode node = null;

                    if (token.raw == "=")
                        node = new AssignmentNode(current);
                    else
                    {
                        node = new OperationNode(current)
                        {
                            op = token.raw
                        };
                    }

                    current.Append(node);
                    current = node;
                }
                else if (token.type == STokenType.Endl)
                {
                    //current = current.parent; 
                }
                else if (token.type == STokenType.Call)
                {
                    var node = new CallMethodNode(current);
                    node.Append(new IdentNode(node)
                    {
                        ident = token.raw
                    });
                    current.Append(node);
                    current = node;
                }
                else if (token.type == STokenType.EndCall)
                {
                    current = current.nearestIncompletedParent;
                }
                else if (token.type == STokenType.Ident)
                {
                    var node = new IdentNode(current);
                    node.ident = token.raw;
                    current = current.Append(node);
                }
                else if (token.type == STokenType.Literal)
                {
                    var node = new LiteralNode(current);
                    node.value = token.raw;
                    current = current.Append(node);
                }
                else if (token.type == STokenType.BeginBlock)
                {
                    var node = new BlockNode(current);
                    current.Append(node);
                    current = node;
                }
                else if (token.type == STokenType.EndBlock)
                {
                    current = current.nearestIncompletedParent;
                }
                else if (token.type == STokenType.Ret)
                {
                    var node = new RetNode(current);
                    current.Append(node);
                    current = node;
                }
                else if (token.type == STokenType.Class)
                {
                    var node = new ClassNode(current);
                    current.Append(node);
                    current = node;

                    currentClass = node;
                }
                else if (
                    token.type == STokenType.Public ||
                    token.type == STokenType.Private)
                {
                    var node = new FieldDeclationNode(current);
                    current.Append(node);
                    current = node;

                    currentClass.AddField(node);
                }
                else if (token.type == STokenType.Method)
                {
                    var node = new MethodNode(current);
                    current.Append(node);
                    current = node;
                }
                else if (token.type == STokenType.If)
                {
                    var node = new IfNode(current);
                    current.Append(node);
                    current = node;
                }
                else if (token.type == STokenType.Else)
                {
                    // Get back to last node
                    current = current.children.Last();
                }
                else if (token.type == STokenType.Param)
                {
                    var node = new ParamsNode(current);
                    current.Append(node);
                    current = node;
                }
                else if (token.type == STokenType.EndParam)
                {
                    current = current.nearestIncompletedParent;
                }

                prevToken = token;
            }

            return root;
        }
    }
}
