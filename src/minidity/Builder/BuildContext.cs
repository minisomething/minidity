using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class BuildContext
    {
        public ClassNode currentClass { get; private set; }
        public MethodNode currentMethod { get; private set; }

        public void SetClass(ClassNode node)
        {
            currentClass = node;
        }
        public void SetMethod(MethodNode node)
        {
            currentMethod = node;
        }
    }
}
