## VintageMods Core Framework

### - VintageMods.Core.Common

Contains features used for universal mods; Harmony based reflection; common functionality used within other parts of the core framework.

### - VintageMods.Core.Client

Contains features used for client-side mods, as well as methods for interacting with the core client API.

### - VintageMods.Core.Server

Contains features used for server-side mods, as well as methods for interacting with the core server API.

### - VintageMods.Core.Threading

Contains a ClientSystem that can be injected so that asynchronous tasks can be carried out; helper methods that aid in concurrency, and task queuing; Harmony patches to ensure calls to the core API are thread-safe.

### - VintageMods.Core.FileIO

Contains methods for reading and writing to files that are on the file system, or embedded within the mod's assembly.

### - VintageMods.Core.FluentChat

The FluentChat Framework makes chat commands easy to implement. Contains base classes to inherit from, and Attributes to annotate classes and methods with, to make fully featured, thread-safe, asynchronous commands.

### - VintageMods.Core.Maths

Contains mathematical functions; camera interpolation styles;  extensions to base game mathematical constructs.

### - VintageMods.Core.MemoryAdaptor

A full port of ProcessSharp, into .NET Standard 2.0. Raw memory editing library that allows a very fine-grained approach to modding. AOB Scanning; Managed and Unmanaged library injection; direct memory manipulation; create and manage code caves; monitor loaded modules. Pretty much anything that Cheat Engine can do, this can do.

### - VintageMods.Core.Legacy

obsolete classes that are being phased out of production, and orphaned files from refactoring. Should only be included within development mods, where end-of-life features are available.