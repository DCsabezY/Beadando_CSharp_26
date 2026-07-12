using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Beadando_CSharp_26
{
    // Wrapper az AutoResetEvent köré.
    internal class OwnAutoResetEvent : OwnEventWaitHandle
    {
        public OwnAutoResetEvent (bool initialState = false) : base(new AutoResetEvent(initialState))
        {
        }
    }
}
