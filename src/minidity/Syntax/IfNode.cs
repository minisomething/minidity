using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class IfNode : SyntaxNode
    {
        public SyntaxNode condition => children[0];
        public SyntaxNode trueBody => children[1];
        public SyntaxNode falseBody => children.Count >= 3 ? children[2] : null;

        public IfNode(SyntaxNode parent) :
            base(parent)
        {
            capacity = 2;
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            condition.Emit(ctx, emitter);
            var jmpEq = emitter.Emit(Opcode.JmpEq, 0);
            trueBody.Emit(ctx, emitter);
            jmpEq.SetOperand(emitter.cursor);
            falseBody?.Emit(ctx, emitter);
        }
    }
}
