using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    public class baseclass
    {
        private readonly string MSig = "A::_ctor";

        protected string CreateExecutable(string exp)
        {
            return @"class A{ def _ctor(){" + exp + "}}";
        }
        protected object Execute(string exp)
        {
            var src = CreateExecutable(exp);

            var p = MinidityCompiler.BuildProgram(src);
            var vm = new VM<MemStateProvider>();

            return vm.Execute(p.abi, p.instructions, MSig, int.MaxValue).ret;
        }
        protected ExeResult Execute2(string exp)
        {
            var src = CreateExecutable(exp);

            var p = MinidityCompiler.BuildProgram(src);
            var vm = new VM<MemStateProvider>();

            return vm.Execute(p.abi, p.instructions, MSig, int.MaxValue);
        }
    }
}
