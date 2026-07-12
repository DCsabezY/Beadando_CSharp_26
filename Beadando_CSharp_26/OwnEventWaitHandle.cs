using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Beadando_CSharp_26
{
    // Wrapper az EventWaitHandle köré, szálak közti jelzéshez.
    internal class OwnEventWaitHandle
    {
        private EventWaitHandle _handle;

        public OwnEventWaitHandle(EventWaitHandle handle)
        {
            _handle = handle;
        }

        // Blokkolja a hívó szálat, amíg valaki nem jelez a Set használatával.
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