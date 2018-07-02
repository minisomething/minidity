using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    [TestClass]
    public class Variable_test : baseclass
    {
        [TestMethod]
        public void LocalVariable()
        {
            Assert.AreEqual(44, Execute("a = 44; ret a;"));

            Assert.AreEqual("zuzu", Execute("a = \"zuzu\"; ret a;"));
        }

        [TestMethod]
        public void AssignTwice()
        {
            Assert.AreEqual(12, Execute("a = 44; a = 12; ret a;"));
        }

        [TestMethod]
        public void Swap()
        {
            Assert.AreEqual(12, Execute(@"
a = 44; b = 12;
tmp = a;
a = b;
b = tmp;
ret a;
"));
        }
    }
}
