using System;
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ModDomainAttribute : Attribute
    {
        public string Domain { get; }
        public string RootFolder { get; }

        public ModDomainAttribute(string domain, string rootFolder)
        {
            Domain = domain;
            RootFolder = rootFolder;
        }

        public ModDomainAttribute(string domain)
        {
            Domain = domain;
            RootFolder = domain;
        }
    }
}