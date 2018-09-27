using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public partial class VM<T>
        where T : IStateProvider, new()
    {
        public IStateProvider stateProvider { get; private set; }

        private Dictionary<Opcode, Action> processor = new Dictionary<Opcode, Action>();
        private ExeContext ctx;

        private bool halt = false;
        private bool verbose = false;

        public VM(bool _verbose = false)
        {
            verbose = _verbose;
            stateProvider = new T();

            InitInternalCall();
            InitOperator();
            InitStack();
            InitDebug();
            InitFlow();
            InitVariable();
        }

        public object Execute(byte[] raw, string methodSignature, object[] args, int gasLimit, out int gasUsed)
        {
            (var abi, var insts) = BConv.FromBytes(raw);
            return Execute(abi, insts, methodSignature, args, gasLimit, out gasUsed);
        }
        public object Execute(
            ABI abi, Instruction[] instructions,
            string methodSignature,
            int gasLimit, out int gasUsed)
        {
            return Execute(abi, instructions,
                methodSignature, new object[] { },
                gasLimit, out gasUsed);
        }
        public object Execute(
            ABI abi, Instruction[] instructions,
            string methodSignature, object[] args,
            int gasLimit, out int gasUsed)
        {
            BuildCalltableFromAbi(abi);

            gasUsed = 0;
            halt = false;
            ctx = new ExeContext(instructions);
            var targetMethod = abi.methods
                .FirstOrDefault(x => x.signature == methodSignature);
            if (targetMethod == null)
                throw new ArgumentException("Method not exists in the given abi.");

            ctx.instructionCursor = targetMethod.entry;

            // LdArgs
            foreach (var arg in args)
                ctx.state.Push(arg);
            ctx.callStack.PushCall(methodSignature);

            if (verbose) {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   --------EXECUTE----------");
            }
            while (halt == false)
            {
                if (gasLimit <= 0)
                    throw new InvalidOperationException("exceed gas limit");

                var inst = instructions[ctx.instructionCursor++];
                ctx.current = inst;

                if (verbose)
                    Console.WriteLine($" - {inst.code, -6} | {inst.operand, 17} | {inst.operand?.GetType()}");

                var gp = GasTable.GetGasPrice(inst.code);
                gasLimit -= gp;
                gasUsed += gp;

                if (inst.code == Opcode.Nop) ;
                else
                    processor[inst.code].Invoke();

                if (halt)
                    Console.WriteLine("HALT");
            }

            if (ctx.state.count == 0)
                return null;
            else return ctx.state.Pop();
        }

        private void Register(Opcode code, Action cb)
        {
            processor[code] = cb;
        }
        private void Handle(Opcode code, Action cb)
        {
            Register(code, () =>
            {
                cb();
            });
        }
        private void HandleO(Opcode code, Action<object> cb)
        {
            Register(code, () =>
            {
                cb(ctx.state.Pop());
            });
        }
        private void Handle(Opcode code, Action<double> cb)
        {
            Register(code, () =>
            {
                cb(ctx.state.PopDouble());
            });
        }
        private void Handle(Opcode code, Action<double, double> cb)
        {
            Register(code, () =>
            {
                var a = ctx.state.PopDouble();
                var b = ctx.state.PopDouble();
                cb(a, b);
            });
        }
        private void HandleWithOperand(Opcode code, Action<double, double> cb)
        {
            Register(code, () =>
            {
                cb(ctx.state.PopDouble(), Convert.ToDouble(ctx.current.operand));
            });
        }
        private void HandleWithOperand(Opcode code, Action<object, string> cb)
        {
            Register(code, () =>
            {
                cb(ctx.state.Pop(), (string)(ctx.current.operand));
            });
        }
        private void HandleWithOperand(Opcode code, Action<string> cb)
        {
            Register(code, () =>
            {
                cb((string)(ctx.current.operand));
            });
        }
        private void HandleWithOperandO(Opcode code, Action<object> cb)
        {
            Register(code, () =>
            {
                cb(ctx.current.operand);
            });
        }
        private void HandleWithOperand(Opcode code, Action<int> cb)
        {
            Register(code, () =>
            {
                cb((int)(ctx.current.operand));
            });
        }
        private void HandleWithOperand(Opcode code, Action<double, int> cb)
        {
            Register(code, () =>
            {
                cb(ctx.state.PopDouble(), (int)(ctx.current.operand));
            });
        }
    }
}
