using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class BlockNode : SyntaxNode
    {
        public BlockNode(SyntaxNode parent) :
            base(parent)
        {
        }
    }
}
