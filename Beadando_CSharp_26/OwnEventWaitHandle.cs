using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Beadando_CSharp_26
{
    internal class OwnEventWaitHandle
    {
        private EventWaitHandle _handle;

        public OwnEventWaitHandle(EventWaitHandle handle)
        {
            _handle = handle;
        }

        public void EventWait()
        {
            _handle.WaitOne();
        }

        public void EventSet()
        {
            _handle.Set();
        }
        public void EventClose()
        {
            _handle.Close();
        }

    }
}
