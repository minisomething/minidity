using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    // Opcode.Callthis
    public class CallMethodNode : SyntaxNode
    {
        public IdentNode ident => (IdentNode)children[0];
        public IdentNode retType => (IdentNode)children[1];
        public List<SyntaxNode> args => children.GetRange(1, children.Count - 1);

        public CallMethodNode(SyntaxNode parent)
            : base(parent)
        {
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            foreach (var arg in args)
                arg.Emit(ctx, emitter);

            if (ident.ident.Contains("."))
                emitter.Emit(Opcode.Call, ident.ident);
            else
                emitter.Emit(Opcode.Call, ABISignature.Method(ctx.currentClass.ident.ident, ident.ident));
        }
    }
}
