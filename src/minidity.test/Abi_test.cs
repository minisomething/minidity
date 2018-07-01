using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minivm;

namespace minidity.test
{
    [TestClass]
    public class Abi_test
    {
        [TestMethod]
        public void HasContracts()
        {
            var src = @"
class Foo {
    def a() { }
}
class Bar {
    def a() { }
}
class Zoo {
    def a() { }
}
";
            var p = MinidityCompiler.BuildProgram(src);

            Assert.AreEqual(true, p.abi.contracts.Any(x => x == "Foo"));
            Assert.AreEqual(true, p.abi.contracts.Any(x => x == "Bar"));
            Assert.AreEqual(true, p.abi.contracts.Any(x => x == "Zoo"));
        }

        [TestMethod]
        public void HasMethods()
        {
            var src = @"
class Foo {
    def a() { }
    def b() { }
    def c() { }
}
";
            var p = MinidityCompiler.BuildProgram(src);

            Assert.AreEqual(true, p.abi.methods.Any(x => x.signature == ABISignature.Method("Foo", "a")));
            Assert.AreEqual(true, p.abi.methods.Any(x => x.signature == ABISignature.Method("Foo", "b")));
            Assert.AreEqual(true, p.abi.methods.Any(x => x.signature == ABISignature.Method("Foo", "c")));
        }
    }
}
