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
             class A{ 
public global_a;

def _ctor(){

c = (2 + 1 ) * (3 + 1);

f = 1;

n = 2;

global_a = 123;
if (f < n) {
    global_a = 4444;
}


Chain.transfer(""1111"", 1);

Console.print(global_a);

ret ""jhello"";
}

def bb(a,b) {
    Chain.transfer(""1111"", 1);
}

}";

            src = @"
class A{ 
def _ctor(){
;c = (2 + 1 ) * (3 + 1);;
}
}
    ";

            var program = MinidityCompiler.BuildProgram(src);
            var vm = new VM<MemStateProvider>(true);

            Console.WriteLine(BConv.ToBase64(program.abi, program.instructions));

            foreach (var i in program.abi.methods)
                Console.WriteLine($"ABI_METHOD : {i.signature}");
            foreach (var i in program.instructions)
                Console.WriteLine($"{i.code} / {i.operand}");

            var ret = vm.Execute(program.abi, program.instructions,
                ABISignature.Method("A", "_ctor"),
                1000, out _);

            Console.WriteLine(vm.stateProvider.GetState(ABISignature.Field("foo","global")));
            Console.WriteLine(ret);
        }
    }
}

