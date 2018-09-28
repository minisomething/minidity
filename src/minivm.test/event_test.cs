using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using minidity;

namespace minivm.test
{
    [TestClass]
    public class Event_test : baseclass
    {
        [TestMethod]
        public void Emit()
        {
            Assert.AreEqual(true, HasEvent(Execute2("$a(1, 2)"), "a", 1, 2));
            Assert.AreEqual(true, HasEvent(Execute2("$a(1)"), "a", 1));
            Assert.AreEqual(true, HasEvent(Execute2("$a()"), "a"));
        }

        private bool HasEvent(ExeResult e, string name, params object[] expectedArgs)
        {
            foreach (var ev in e.events)
            {
                if (ev.ident != name) continue;

                if (Enumerable.SequenceEqual(ev.args, expectedArgs))
                    return true;
            }
            return false;
        }
    }
}
