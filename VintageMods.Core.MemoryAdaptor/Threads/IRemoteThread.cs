using System;
using System.Diagnostics;
using VintageMods.Core.MemoryAdaptor.Native.Types;

namespace VintageMods.Core.MemoryAdaptor.Threads
{
    public interface IRemoteThread : IDisposable
    {
        ThreadContext Context { get; set; }
        SafeMemoryHandle Handle { get; }
        int Id { get; }
        bool IsAlive { get; }
        bool IsMainThread { get; }
        bool IsSuspended { get; }
        bool IsTerminated { get; }
        ProcessThread Native { get; }

        T GetExitCode<T>();
        int GetHashCode();
        IntPtr GetRealSegmentAddress(SegmentRegisters segment);
        void Join();
        WaitValues Join(TimeSpan time);
        void Refresh();
        void Resume();
        IFrozenThread Suspend();
        void Terminate(int exitCode = 0);
    }
}