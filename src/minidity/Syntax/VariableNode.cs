using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class VariableDeclationNode : SyntaxNode
    {
        public IdentNode ident
        {
            get
            {
                return (IdentNode)children[0];
            }
        }
        public SyntaxNode value
        {
            get
            {
                return children.Count >= 2 ? children[1] : null;
            }
        }

        public VariableDeclationNode(SyntaxNode parent)
            : base(parent)
        {
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            value?.Emit(ctx, emitter);

            //emitter.Emit(Opcode.Ldloc, ident);
            Console.WriteLine("stloc." + ident.ident);
        }

        public override string ToString()
        {
            return base.ToString() + $"({ident})";
        }
    }

    public class FieldDeclationNode : VariableDeclationNode
    {
        public FieldDeclationNode(SyntaxNode parent)
            : base(parent)
        {
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            value?.Emit(ctx, emitter);
            //emitter.Emit(Opcode.Ldstate,
            //    ABISignature.Field(ctx.currentClass.ident.ident, ident.ident));
        }
    }
}
