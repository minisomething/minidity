using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class RootNode : SyntaxNode
    {
        public RootNode(SyntaxNode parent)
            : base(parent)
        {
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            foreach (var child in children)
                child.Emit(ctx, emitter);
        }
    }
}
