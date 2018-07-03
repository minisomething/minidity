using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    [TestClass]
    public class require_test : baseclass
    {
        [TestMethod]
        public void BasicRequire()
        {
            Assert.AreEqual(1, Execute("require(1); ret 1;"));
            Assert.AreEqual(null, Execute("require(0); ret 1;"));
        }

        [TestMethod]
        public void RequireWithExpression()
        {
            Assert.AreEqual(1, Execute("require(1 > 0); ret 1;"));
            Assert.AreEqual(null, Execute("require(0 > 1); ret 1;"));
        }

        [TestMethod]
        public void WithVariable()
        {
            Assert.AreEqual(1, Execute("a = 1; require(a); ret 1;"));
            Assert.AreEqual(null, Execute("a = 0; require(a); ret 1;"));
        }
    }
}
