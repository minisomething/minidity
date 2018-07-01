using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class OperationNode : SyntaxNode
    {
        public string op { get; set; }
        public SyntaxNode left => children[0];
        public SyntaxNode right => children[1];

        public OperationNode(SyntaxNode parent)
            : base(parent)
        {
            capacity = 2;
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            left.Emit(ctx, emitter);
            right.Emit(ctx, emitter);

            switch (op)
            {
                case "+":
                    emitter.Emit(Opcode.Add);
                    break;
                case "-":
                    emitter.Emit(Opcode.Sub);
                    break;
                case "*":
                    emitter.Emit(Opcode.Mul);
                    break;
                case "/":
                    emitter.Emit(Opcode.Div);
                    break;

                case ">":
                    emitter.Emit(Opcode.G);
                    break;
                case "<":
                    emitter.Emit(Opcode.L);
                    break;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}({op})";
        }
    }
    public class AssignmentNode : SyntaxNode
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
                return children[1];
            }
        }

        public AssignmentNode(SyntaxNode parent)
            : base(parent)
        {
            capacity = 2;
        }
        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            value.Emit(ctx, emitter);

            if (ctx.currentClass.fields.Any(x => x.ident.ident == ident.ident))
                emitter.Emit(Opcode.Ststate, ABISignature.Field(ctx.currentClass.ident.ident,  ident.ident));
            else
                emitter.Emit(Opcode.Stloc, ident.ident);
        }
    }
}
