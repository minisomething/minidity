using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    [TestClass]
    public class If_test : baseclass
    {
        [TestMethod]
        public void BasicIf()
        {
            Assert.AreEqual(1, Execute("if (3 > 2) ret 1; ret 2;"));
            Assert.AreEqual(2, Execute("if (2 > 3) ret 1; ret 2;"));
        }

        [TestMethod]
        public void SingleValue()
        {
            Assert.AreEqual(1, Execute("if (2) ret 1; ret 2;"));
            Assert.AreEqual(2, Execute("if (0) ret 1; ret 2;"));
        }

        [TestMethod]
        public void IfWithBracket()
        {
            Assert.AreEqual(1, Execute("if (2) { ret 1; } ret 2;"));
            Assert.AreEqual(2, Execute("if (0) { ret 1; } ret 2;"));
        }
    }
}
