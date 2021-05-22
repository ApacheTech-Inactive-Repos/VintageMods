using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Core.FluentChat.Attributes
{
    /// <summary>
    ///     Marks a method as an available, named option within a FluentChat command.
    ///     This attribute can be used multiple times on the same method, denoting aliases that can be used to perform the same task.
    ///     This class cannot be inherited.
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class FluentChatOptionAttribute : Attribute
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatOptionAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public FluentChatOptionAttribute(string name, string description)
        {
            (Name, Description) = (name, description);
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatOptionAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FluentChatOptionAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatOptionAttribute"/> class.
        /// </summary>
        public FluentChatOptionAttribute() { }

        /// <summary>
        ///     Gets or sets the name of the chat command option.
        /// </summary>
        /// <value>The phrase users must type into chat, in order to invoke this command option.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description of the chat command option. If blank, the canonical entry within the localisation strings files is used.
        /// </summary>
        /// <value>The description of the chat command option, to be displayed to the user.</value>
        public string Description { get; set; } = "";
    }
}
