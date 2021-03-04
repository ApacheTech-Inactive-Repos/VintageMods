using System;
using JetBrains.Annotations;

namespace VintageMods.Core.Fluent.Attributes
{
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, 
        ImplicitUseTargetFlags.WithMembers)]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class FluentOptionAttribute : Attribute
    {
        public FluentOptionAttribute(string name, string description)
        {
            (Name, Description) = (name, description);
        }


        public FluentOptionAttribute(string name)
        {
            Name = name;
        }

        public FluentOptionAttribute()
        {
        }

        public string Name { get; set; }

        public string Description { get; set; } = "";
    }
}