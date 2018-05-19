using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public class MemStateProvider : IStateProvider
    {
        public int blockNo => 1;

        private Dictionary<string, object> data = new Dictionary<string, object>();

        public object GetState(string key)
        {
            if (data.ContainsKey(key))
                return data[key];
            return 0;
        }
        public void SetState(string key, object value)
        {
            data[key] = value;
        }

        public void Transfer(string receiverAddress, double amount)
        {
        }
    }
}
