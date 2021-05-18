using System;

// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.Attributes
{
    /// <summary>
    ///     Assembly wide metadata, for VintageMods mods.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ModDomainAttribute : Attribute
    {
        /// <summary>
        ///     Gets the domain name for this mod. Used for localisation.
        /// </summary>
        /// <value>The localisation domain for this mod.</value>
        public string Domain { get; }

        /// <summary>
        ///     Gets the name of the folder used to store mod information.
        /// </summary>
        /// <value>The folder, within %AppData%\VintagestoryData\ModData\VintageMods, used to store files for this mod.</value>
        public string RootFolder { get; }
        
        /// <summary>
        ///     Initialises a new instance of the <see cref="ModDomainAttribute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="rootFolder">The root folder.</param>
        public ModDomainAttribute(string domain, string rootFolder)
        {
            Domain = domain;
            RootFolder = rootFolder;
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModDomainAttribute"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        public ModDomainAttribute(string domain)
        {
            Domain = domain;
            RootFolder = domain;
        }
    }
}