using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class ParamsNode : SyntaxNode
    {
        public ParamsNode(SyntaxNode parent) :
            base(parent)
        {
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            if (children.Count == 0) return;

            for (var i = children.Count - 1; i >= 0; i--)
            {
                var ident = (IdentNode)children[i];

                emitter.Emit(Opcode.Stloc, ident.ident);
            }
        }
    }
    public class ParamNode : SyntaxNode
    {
        public IdentNode ident => (IdentNode)children[0];

        public ParamNode(SyntaxNode parent) :
            base(parent)
        {
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            Console.Write($"object {ident.ident}");
        }
    }
    public class RetNode : SyntaxNode
    {
        public SyntaxNode value => children.Count != 0 ? children[0] : null;

        public RetNode(SyntaxNode parent) :
            base(parent)
        {
            capacity = 1;
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            value?.Emit(ctx, emitter);

            emitter.Emit(Opcode.Ret);
        }
    }

    public class MethodNode : SyntaxNode
    {
        public IdentNode ident => (IdentNode)children[0];
        public ParamsNode parameters => (ParamsNode)children[1];

        public MethodNode(SyntaxNode parent) :
            base(parent)
        {
            capacity = 3;
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            ctx.SetMethod(this);
            ctx.currentClass.AddMethod(this);

            emitter.EmitSignature(ABISignature.Method(ctx.currentClass.ident.ident, ident.ident));
            parameters.Emit(ctx, emitter);

            for (var i = 2; i < children.Count; i++)
                children[i].Emit(ctx, emitter);

            // PADDING
            emitter.Emit(Opcode.Ret);
            emitter.Emit(Opcode.Nop);
        }
        public override string ToString()
        {
            return base.ToString() + $"({ident.ident})";
        }
    }
}
