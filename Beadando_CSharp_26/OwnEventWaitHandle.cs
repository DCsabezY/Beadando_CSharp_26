using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Beadando_CSharp_26
{
    /// <summary>
    /// Az EventWaitHandle osztály egy szálak várakozását vezérlő eszköz, amely lehetővé teszi a szálak közötti kommunikációt és koordinációt. 
    /// Az OwnEventWaitHandle osztály egy egyszerű wrapper az EventWaitHandle osztály körül, amely lehetővé teszi a szálak közötti események kezelését.
    /// </summary>
    internal class OwnEventWaitHandle
    {
        private EventWaitHandle _handle;

        public OwnEventWaitHandle(EventWaitHandle handle)
        {
            _handle = handle;
        }
        //Az EventWait() metódus blokkolja a hívó szálat, amíg az esemény be nem állítódik.
        public void EventWait()
        {
            _handle.WaitOne();
        }

        //Az EventSet() metódus pedig beállítja az eseményt, így feloldva a blokkolást.
        public void EventSet()
        {
            _handle.Set();
        }

        //Az EventClose() metódus pedig felszabadítja az erőforrásokat, amelyeket az EventWaitHandle használ.
        public void EventClose()
        {
            _handle.Close();
        }

    }
}
