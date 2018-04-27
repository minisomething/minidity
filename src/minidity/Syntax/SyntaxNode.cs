using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class SyntaxNode
    {
        public SyntaxNode parent { get; set; }
        public List<SyntaxNode> children { get; set; }
        public int capacity { get; set; }

        public bool isComplete
        {
            get
            {
                return capacity > 0 ? children.Count >= capacity : false;
            }
        }
        public SyntaxNode nearestIncompletedParent
        {
            get
            {
                var current = parent;
                if (current == null)
                    return this;

                while (true)
                {
                    if (current is RootNode)
                        return current;

                    if (current.isComplete)
                        current = current.parent;
                    else
                        return current;
                }
            }
        }

        public SyntaxNode(SyntaxNode _parent)
        {
            parent = _parent;
            capacity = -1;
            children = new List<SyntaxNode>();
        }

        public void Print(int depth = 0)
        {
            for (var i = 0; i < depth; i++)
                Console.Write("  ");
            Console.WriteLine(this.ToString());
            foreach (var child in children)
                child.Print(depth + 1);
        }
        public SyntaxNode Append(SyntaxNode child)
        {
            children.Add(child);

            if (isComplete)
                return nearestIncompletedParent;
            return this;
        }
        public virtual void Emit(BuildContext ctx, Emitter emitter)
        {
            foreach (var child in children)
                child.Emit(ctx, emitter);
        }
    }
}
