using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class ForNode : SyntaxNode
    {
        public SyntaxNode init => children[0];
        public SyntaxNode cond => children[1];
        public SyntaxNode increment => children[2];
        public SyntaxNode body => children[3];

        public ForNode(SyntaxNode parent) :
            base(parent)
        {
            capacity = 4;
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            init.Emit(ctx, emitter);
            var cursor = emitter.cursor;
            body.Emit(ctx, emitter);
            increment?.Emit(ctx, emitter);
            cond.Emit(ctx, emitter);
            emitter.Emit(Opcode.JmpTrue, cursor);
        }
    }
}
