using System;
using VintageMods.Core.MemoryAdaptor.Windows.Mouse;

namespace VintageMods.Core.MemoryAdaptor.Windows
{
    public abstract class HookEventArgs : EventArgs
    {
        protected HookEventType EventType { get; set; }
    }
}