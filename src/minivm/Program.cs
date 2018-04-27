using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    class Program
    {
        static void Main(string[] args)
        {
            var inst = new Instruction[]
            {
                new Instruction() { code = Opcode.Push, operand = 5 },
                new Instruction() { code = Opcode.Stloc, operand = "i" },
                new Instruction() { code = Opcode.Push, operand = 1 },
                new Instruction() { code = Opcode.Push, operand = 1 },
                new Instruction() { code = Opcode.Add },
                new Instruction() { code = Opcode.Ldloc, operand = "i" },
                new Instruction() { code = Opcode.Dec },
                new Instruction() { code = Opcode.Stloc, operand = "i" },
                new Instruction() { code = Opcode.Ldloc, operand = "i" },
                new Instruction() { code = Opcode.Cmp, operand = 0 }, 
                new Instruction() { code = Opcode.JmpG, operand = 2 },
            };

            var vm = new VM<MemStateProvider>();
            Console.WriteLine(vm.Execute(null, inst, 1000, out _));

            try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw e;
            }
        }
    }
}
