﻿namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     The structure of the Process Environment Block.
    /// </summary>
    /// <remarks>
    ///     Tested on Windows 7 x64, 2013-03-10
    ///     Source: http://blog.rewolf.pl/blog/?p=573#.UTyBo1fJL6p
    /// </remarks>
    public enum PebStructure
    {
        InheritedAddressSpace = 0x0,
        ReadImageFileExecOptions = 0x1,

        /// <summary>
        ///     Gets if the process is being debugged.
        /// </summary>
        BeingDebugged = 0x2,
        SpareBool = 0x3,
        Mutant = 0x4,
        ImageBaseAddress = 0x8,
        Ldr = 0xC,
        ProcessParameters = 0x10,
        SubSystemData = 0x14,
        ProcessHeap = 0x18,
        FastPebLock = 0x1C,
        FastPebLockRoutine = 0x20,
        FastPebUnlockRoutine = 0x24,
        EnvironmentUpdateCount = 0x28,
        KernelCallbackTable = 0x2C,
        SystemReserved = 0x30,
        AtlThunkSListPtr32 = 0x34,
        FreeList = 0x38,
        TlsExpansionCounter = 0x3C,
        TlsBitmap = 0x40,

        /// <summary>
        ///     Length: 8 bytes.
        /// </summary>
        TlsBitmapBits = 0x44,
        ReadOnlySharedMemoryBase = 0x4C,
        ReadOnlySharedMemoryHeap = 0x50,
        ReadOnlyStaticServerData = 0x54,
        AnsiCodePageData = 0x58,
        OemCodePageData = 0x5C,
        UnicodeCaseTableData = 0x60,
        NumberOfProcessors = 0x64,

        /// <summary>
        ///     Length: 8 bytes.
        /// </summary>
        NtGlobalFlag = 0x68,

        /// <summary>
        ///     Length: 8 bytes (LARGE_INTEGER type).
        /// </summary>
        CriticalSectionTimeout = 0x70,
        HeapSegmentReserve = 0x78,
        HeapSegmentCommit = 0x7C,
        HeapDeCommitTotalFreeThreshold = 0x80,
        HeapDeCommitFreeBlockThreshold = 0x84,
        NumberOfHeaps = 0x88,
        MaximumNumberOfHeaps = 0x8C,
        ProcessHeaps = 0x90,
        GdiSharedHandleTable = 0x94,
        ProcessStarterHelper = 0x98,
        GdiDcAttributeList = 0x9C,
        LoaderLock = 0xA0,
        OsMajorVersion = 0xA4,
        OsMinorVersion = 0xA8,

        /// <summary>
        ///     Length: 2 bytes.
        /// </summary>
        OsBuildNumber = 0xAC,

        /// <summary>
        ///     Length: 2 bytes.
        /// </summary>
        OsCsdVersion = 0xAE,
        OsPlatformId = 0xB0,
        ImageSubsystem = 0xB4,
        ImageSubsystemMajorVersion = 0xB8,
        ImageSubsystemMinorVersion = 0xBC,
        ImageProcessAffinityMask = 0xC0,

        /// <summary>
        ///     Length: 0x88 bytes (0x22 * sizeof(IntPtr)).
        /// </summary>
        GdiHandleBuffer = 0xC4,
        PostProcessInitRoutine = 0x14C,
        TlsExpansionBitmap = 0x150,

        /// <summary>
        ///     Length: 0x80 bytes (0x20 * sizeof(IntPtr))
        /// </summary>
        TlsExpansionBitmapBits = 0x154,
        SessionId = 0x1D4,

        /// <summary>
        ///     Length: 8 bytes (LARGE_INTEGER type).
        /// </summary>
        AppCompatFlags = 0x1D8,

        /// <summary>
        ///     Length: 8 bytes (LARGE_INTEGER type).
        /// </summary>
        AppCompatFlagsUser = 0x1E0,
        ShimData = 0x1E8,
        AppCompatInfo = 0x1EC,

        /// <summary>
        ///     Length: 8 bytes (UNICODE_STRING type).
        /// </summary>
        CsdVersion = 0x1F0,
        ActivationContextData = 0x1F8,
        ProcessAssemblyStorageMap = 0x1FC,
        SystemDefaultActivationContextData = 0x200,
        SystemAssemblyStorageMap = 0x204,
        MinimumStackCommit = 0x208
    }
}