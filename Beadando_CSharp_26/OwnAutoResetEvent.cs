using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Beadando_CSharp_26
{
    internal class OwnAutoResetEvent : OwnEventWaitHandle
    {
        public OwnAutoResetEvent (bool initialState = false) : base(new AutoResetEvent(initialState))
        {
        }
    }
}
