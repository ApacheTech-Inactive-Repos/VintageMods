using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VintageMods.Core.MemoryAdaptor.Native.Types;
using VintageMods.Core.MemoryAdaptor.Utilities;

// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.MemoryAdaptor.Extensions
{
    public static class ProcessExtensions
    {
        public static IList<ProcessThread> GetThreads(this Process process)
        {
            return process.Threads.Cast<ProcessThread>().ToList();
        }

        public static IList<ProcessModule> GetModules(this Process process)
        {
            return process.Modules.Cast<ProcessModule>().ToList();
        }

        public static SafeMemoryHandle Open(this Process process, ProcessAccessFlags processAccessFlags = ProcessAccessFlags.AllAccess)
        {
            return MemoryHelper.OpenProcess(processAccessFlags, process.Id);
        }

        public static string GetVersionInfo(this Process process)
        {
            return
                $"{process.MainModule?.FileVersionInfo.FileDescription} {process.MainModule?.FileVersionInfo.FileMajorPart}.{process.MainModule?.FileVersionInfo.FileMinorPart}.{process.MainModule?.FileVersionInfo.FileBuildPart} {process.MainModule?.FileVersionInfo.FilePrivatePart}";
        }

    }
}
