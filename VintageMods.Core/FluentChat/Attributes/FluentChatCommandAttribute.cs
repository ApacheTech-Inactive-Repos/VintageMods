using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Core.FluentChat.Attributes
{
    /// <summary>
    ///     Marks a class as a chat command handler for a VintageMods mod. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class FluentChatCommandAttribute : Attribute
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatCommandAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="syntaxMessage">The syntax message.</param>
        public FluentChatCommandAttribute(string name, string description, string syntaxMessage)
        {
            (Name, Description, SyntaxMessage) = (name, description, syntaxMessage);
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatCommandAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public FluentChatCommandAttribute(string name, string description)
        {
            (Name, Description) = (name, description);
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatCommandAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FluentChatCommandAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatCommandAttribute" /> class.
        /// </summary>
        public FluentChatCommandAttribute()
        {
        }

        /// <summary>
        ///     Gets or sets the name of the chat command.
        /// </summary>
        /// <value>The phrase users must type into chat, in order to invoke this command.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description of the chat command. If blank, the canonical entry within the localisation strings
        ///     files is used.
        /// </summary>
        /// <value>The description of the chat command, to be displayed to the user.</value>
        public string Description { get; set; } = "";

        /// <summary>
        ///     Gets or sets the syntax message of the chat command. If blank, the canonical entry within the localisation strings
        ///     files is used.
        /// </summary>
        /// <value>A message that shows the user what the valid syntax for this command is.</value>
        public string SyntaxMessage { get; set; } = "";
    }
}