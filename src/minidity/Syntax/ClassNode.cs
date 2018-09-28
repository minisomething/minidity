using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minidity
{
    public class ClassNode : SyntaxNode
    {
        public IdentNode ident => (IdentNode)children[0];
        public FieldDeclationNode[] fields => _fields.ToArray();
        public MethodNode[] methods => _methods.ToArray();

        private List<EventDeclationNode> _events;
        private List<FieldDeclationNode> _fields;
        private List<MethodNode> _methods;

        public ClassNode(SyntaxNode parent) :
            base(parent)
        {
            _fields = new List<FieldDeclationNode>();
            _methods = new List<MethodNode>();
            _events = new List<EventDeclationNode>();
        }

        public void AddEvent(EventDeclationNode ev)
        {
            _events.Add(ev);
        }
        public void AddField(FieldDeclationNode field)
        {
            _fields.Add(field);
        }
        public void AddMethod(MethodNode method)
        {
            _methods.Add(method);
        }

        public override void Emit(BuildContext ctx, Emitter emitter)
        {
            ctx.SetClass(this);

            emitter.EmitContract(ident.ident);

            for (var i = 1; i < children.Count; i++)
                children[i].Emit(ctx, emitter);
        }

        public override string ToString()
        {
            return base.ToString() + $"({ident.ident})";
        }
    }
}
