using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Beadando_CSharp_26
{
    /// <summary>
    /// Az OwnAutoResetEvent osztály egy egyszerű wrapper az AutoResetEvent osztály körül, amely lehetővé teszi a szálak közötti események kezelését.
    /// </summary>
    internal class OwnAutoResetEvent : OwnEventWaitHandle
    {
        public OwnAutoResetEvent (bool initialState = false) : base(new AutoResetEvent(initialState))
        {
        }
    }
}
