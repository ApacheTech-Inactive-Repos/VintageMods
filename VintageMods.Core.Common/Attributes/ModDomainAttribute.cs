using System;
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ModDomainAttribute : Attribute
    {
        public string Domain { get; set; } = "vintagemods";
        public string RootFolder { get; set; } = "VintageMods";

        public ModDomainAttribute(string domain, string rootFolder)
        {
            Domain = domain;
            RootFolder = rootFolder;
        }

        public ModDomainAttribute(string domain)
        {
            Domain = domain;
        }

        public ModDomainAttribute()
        {

        }
    }
}