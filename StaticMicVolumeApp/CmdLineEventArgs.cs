using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticMicVolumeApp
{
    class CmdLineEventArgs : EventArgs
    {
        public bool Initialized { get { return true; } }
    }
}
