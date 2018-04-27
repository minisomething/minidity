using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class LiteralNode : SyntaxNode
    {
        public Type type { get; set; }
        public object value { get; set; }

        public LiteralNode(SyntaxNode parent)
            : base(parent)
        {
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            emitter.Emit(Opcode.Push, value);
        }
        public override string ToString()
        {
            return base.ToString() + $"({value})";
        }
    }
}
