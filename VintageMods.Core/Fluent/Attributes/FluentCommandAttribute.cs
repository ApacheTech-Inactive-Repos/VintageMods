using System;
using JetBrains.Annotations;

namespace VintageMods.Core.Fluent.Attributes
{
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class FluentCommandAttribute : Attribute
    {
        public FluentCommandAttribute(string name, string description, string syntaxMessage)
        {
            (Name, Description, SyntaxMessage) = (name, description, syntaxMessage);
        }


        public FluentCommandAttribute(string name, string description)
        {
            (Name, Description) = (name, description);
        }


        public FluentCommandAttribute(string name)
        {
            Name = name;
        }

        public FluentCommandAttribute()
        {
        }

        public string Name { get; set; }

        public string Description { get; set; } = "";

        public string SyntaxMessage { get; set; } = "";
    }
}