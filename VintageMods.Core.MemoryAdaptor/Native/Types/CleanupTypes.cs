namespace VintageMods.Core.MemoryAdaptor.Native.Types
{
    /// <summary>
    ///     A list of type of clean-up available in calling conventions.
    /// </summary>
    public enum CleanupTypes
    {
        /// <summary>
        ///     The clean-up task is performed by the called function.
        /// </summary>
        Callee,

        /// <summary>
        ///     The clean-up task is performed by the caller function.
        /// </summary>
        Caller
    }
}