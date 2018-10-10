using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    /// <summary>
    /// Transforms virtual nodes into real nodes.
    /// </summary>
    class VNodeTransformer
    {
        public static void Transform(RootNode root)
        {
            SyntaxNode _root = root;
            _Transform(ref _root);
        }

        private static void _Transform(ref SyntaxNode node)
        {
            if (node is StandaloneOperatorNode so)
            {
                node = new AssignmentNode(node.parent);
                node.Append(so.left);
                var plus = new OperationNode(node);
                node.Append(plus);

                plus.Append(so.left);
                if (so.op == "++")
                    plus.op = "+";
                else if (so.op == "--")
                    plus.op = "-";

                var one = new LiteralNode(plus) { value = 1 };
                plus.Append(one);
            }

            for (int i = 0; i < node.children.Count; i++)
            {
                var child = node.children[i];
                _Transform(ref child);
                node.children[i] = child;
            }
        }
    }
}

