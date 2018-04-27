using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class IdentNode : SyntaxNode
    {
        public string ident { get; set; }

        public IdentNode(SyntaxNode parent)
            : base(parent)
        {
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            if (ctx.currentClass.fields.Any(x => x.ident.ident == ident))
                emitter.Emit(Opcode.Ldstate, ABISignature.Field(ctx.currentClass.ident.ident, ident));
            else
                emitter.Emit(Opcode.Ldloc, ident);
        }
        public override string ToString()
        {
            return base.ToString() + $"({ident})";
        }
    }
}
