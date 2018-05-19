using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minivm
{
    public interface IStateProvider
    {
        int blockNo { get; }

        object GetState(string key);
        void SetState(string key, object value);

        void Transfer(string receiverAddress, double value);
    }
}
