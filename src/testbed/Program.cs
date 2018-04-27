using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using minidity;
using minivm;

namespace testbed
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = @"
             class A{ def _ctor(){
Console.print(bb(2,3));
Console.print(Chain.getBlockNo());

}

def bb(a,b) {
    ret Math.pow(a, b);
}

}";

            var program = MinidityCompiler.Compile(src);
            var vm = new VM<MemStateProvider>();

            Console.WriteLine(BConv.ToBase64(program.instructions));

            foreach (var i in program.abi.methods)
                Console.WriteLine($"ABI_METHOD : {i.signature}");
            foreach (var i in program.instructions)
                Console.WriteLine($"{i.code} / {i.operand}");

            var ret = vm.Execute(program.abi, program.instructions, 1000, out _);

            Console.WriteLine(vm.stateProvider.GetState(ABISignature.Field("foo","global")));
            Console.WriteLine(ret);
        }
    }
}

