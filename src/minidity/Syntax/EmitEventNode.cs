using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class EmitEventNode : SyntaxNode
    {
        public IdentNode ident => (IdentNode)children[0];
        public List<SyntaxNode> args => children.GetRange(1, children.Count - 1);

        public EmitEventNode(SyntaxNode parent)
            : base(parent)
        {
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            emitter.Emit(Opcode.Push, ident.ident);

            foreach (var arg in args)
                arg.Emit(ctx, emitter);

            emitter.Emit(Opcode.EmitEvent, 1 + args.Count);
        }
    }
}
