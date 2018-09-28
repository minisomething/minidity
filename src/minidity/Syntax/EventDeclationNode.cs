using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class EventDeclationNode : VariableDeclationNode
    {
        public EventDeclationNode(SyntaxNode parent)
            : base(parent)
        {
            // TODO
            capacity = 1;
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            value?.Emit(ctx, emitter);
            //emitter.Emit(Opcode.Ldstate,
            //    ABISignature.Field(ctx.currentClass.ident.ident, ident.ident));
        }
    }
}
