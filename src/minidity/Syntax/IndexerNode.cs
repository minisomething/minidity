using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class IndexerNode : SyntaxNode
    {
        public SyntaxNode key => children[0];
        public SyntaxNode index => children[1];

        public IndexerNode(SyntaxNode parent) :
            base(parent)
        {
            capacity = 2;
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            if (key is IdentNode keyIdent)
            {
                object idx = null;

                if (index is LiteralNode idxLiteral)
                    idx = idxLiteral.value;

                if (ctx.currentClass.fields.Any(x => x.ident.ident == keyIdent.ident)) {
                    emitter.Emit(Opcode.Ldstate, 
                        ABISignature.Dictionary(
                            ABISignature.Field(ctx.currentClass.ident.ident, keyIdent.ident),
                            idx));
                }
                else
                    emitter.Emit(Opcode.Ldloc, ABISignature.Dictionary(keyIdent.ident, idx));
            }
        }
    }
}
