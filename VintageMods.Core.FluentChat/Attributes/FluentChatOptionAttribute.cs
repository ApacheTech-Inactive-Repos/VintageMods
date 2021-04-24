using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Core.FluentChat.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class FluentChatOptionAttribute : Attribute
    {
        public FluentChatOptionAttribute(string name, string description)
        {
            (Name, Description) = (name, description);
        }


        public FluentChatOptionAttribute(string name)
        {
            Name = name;
        }

        public FluentChatOptionAttribute()
        {
        }

        public string Name { get; set; }

        public string Description { get; set; } = "";
    }
}
