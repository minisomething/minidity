using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minivm;

namespace minidity
{
    public class Emitter
    {
        public int cursor => instructions.Count;

        private List<Instruction> instructions = new List<Instruction>();
        private List<string> contracts = new List<string>();
        private Dictionary<string, int> signatures = new Dictionary<string, int>();

        public Instruction Emit(Opcode opcode, object operand = null)
        {
            var inst = new Instruction()
            {
                code = opcode,
                operand = operand
            };
            instructions.Add(inst);

            return inst;
        }
        public void EmitContract(string ident)
        {
            contracts.Add(ident);
        }
        public void EmitSignature(string sign)
        {
            signatures.Add(sign, cursor);
        }

        public Instruction[] GetInstructions()
        {
            return instructions.ToArray();
        }
        public ABI GetABI()
        {
            return new ABI()
            {
                contracts = contracts.ToArray(),
                methods = signatures
                    .Select(x => new Method()
                    {
                        signature = x.Key,
                        entry = x.Value
                    }).ToArray()
            };
        }
    }
}
