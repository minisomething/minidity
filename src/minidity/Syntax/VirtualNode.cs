using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class VirtualNode : SyntaxNode
    {
        public VirtualNode(SyntaxNode _parent) 
            : base(_parent)
        {
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            throw new InvalidOperationException("Unprocessed vnode: " + this);
        }
    }
}
