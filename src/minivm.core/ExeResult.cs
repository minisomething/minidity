using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public class ExeResult
    {
        /// <summary>
        /// Events emiited during this execution.
        /// </summary>
        public EventData[] events;
        /// <summary>
        /// Actual consumed gas amount for exeucution.
        /// </summary>
        public int gasUsed;

        /// <summary>
        /// Return value (last stack object)
        /// </summary>
        public object ret;
    }
}
