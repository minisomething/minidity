using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    [TestClass]
    public class Expression_test : baseclass
    {
        [TestMethod]
        public void BasicOperation()
        {
            Assert.AreEqual(14.0 + 16.0, Execute("14 + 16;"));
            Assert.AreEqual(14.0 + 16.0 + 10.0, Execute("14 + 16 + 10;"));
            Assert.AreEqual(16.0 + 14.0, Execute("16 + 14;"));
            Assert.AreEqual(55.0 * 55.0, Execute("55 * 55;"));
            Assert.AreEqual(3.0 / 2.0, Execute("3 / 2;"));
        }

        [TestMethod]
        public void Paren()
        {
            Assert.AreEqual(2.0 * (3.0 + 1.0), Execute("2 * (3 + 1);"));
            Assert.AreEqual((2.0 + 1.0) * (3.0 + 1.0), Execute("(2 + 1) * (3 + 1);"));
        }
    }
}
