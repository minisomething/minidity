using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    [TestClass]
    public class Assignment_test
    {
        private readonly string MSig = "A::_ctor";

        private string CreateExecutable(string exp)
        {
            return @"class A{ def _ctor(){" + exp + "}}";
        }
        private object Execute(string exp)
        {
            var src = CreateExecutable(exp);

            var p = MinidityCompiler.BuildProgram(src);
            var vm = new VM<MemStateProvider>();

            return vm.Execute(p.abi, p.instructions, MSig, int.MaxValue, out _);
        }

        [TestMethod]
        public void BasigAssignment()
        {
            Assert.AreEqual(1234.0, Execute("a = 1234; ret a;"));
        }

        [TestMethod]
        public void Operation()
        {
            Assert.AreEqual(2.0 * 3.0, Execute("a = 2 * 3; ret a;"));
        }
    }
}
