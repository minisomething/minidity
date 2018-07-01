using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace minidity
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = @"
             class foo {
                public global;
                private pg;

                def _ctor() {
                    
                    global = ""QWER"";
                }

                def sum (a, b) {
                    ret a + b;
                }

                def sum2 (a, b) {
                    ret a + b;
                }
            }";

            var program = MinidityCompiler.BuildProgram(src);

            foreach (var i in program.instructions)
                Console.WriteLine(i.code + " / " + i.operand);

            Console.WriteLine();
            foreach (var i in program.abi.methods)
                Console.WriteLine(i.signature + " / "  + i.entry);
        }
    }
}