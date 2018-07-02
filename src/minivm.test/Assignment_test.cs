using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    [TestClass]
    public class Assignment_test : baseclass
    {
        [TestMethod]
        public void BasigAssignment()
        {
            Assert.AreEqual(1234, Execute("a = 1234; ret a;"));
        }

        [TestMethod]
        public void Operation()
        {
            Assert.AreEqual(2.0 * 3.0, Execute("a = 2 * 3; ret a;"));
        }

        [TestMethod]
        public void ValueToValue()
        {
            Assert.AreEqual(2.0 * 3.0, Execute("a = 2 * 3; b = a; ret b;"));
        }
    }
}
