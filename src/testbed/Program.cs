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
class Mrc20 {
  public totalSupply;
  
  public balances;
  
  def _ctor(_totalSupply) {
    totalSupply = _totalSupply;
    balances[tx.sender] = totalSupply;
  }
  
  def transfer(address, amount) {
    require(amount > 0);
    require(balances[address] > amount);
    
    balances[address] = balances[address] + amount;
    balances[tx.sender] = balances[tx.sender] - amount;
  }
  def transferFrom(from, to, amount) {
  }
  
  def balanceOf(address) {
    return balances[address];
  }
  
  def approve(spender, amount) {
  }
  def allowance(owner, spender) {
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
                ABISignature.Method("Mrc20", "_ctor"),
                new object[] { 1234 },
                1000, out _);

            ret = vm.Execute(program.abi, program.instructions,
                ABISignature.Method("Mrc20", "transfer"),
                new object[] { "qwer", 1000 },
                1000, out _);

            ret = vm.Execute(program.abi, program.instructions,
                ABISignature.Method("Mrc20", "balanceOf"),
                new object[] { },
                1000, out _);

            //Console.WriteLine(vm.stateProvider.GetState(ABISignature.Field("foo","global")));
            if (ret != null)
            {
                Console.WriteLine(ret.GetType());
                Console.WriteLine(ret);
            }
            else
                Console.WriteLine("RET NULL");
        }
    }
}

