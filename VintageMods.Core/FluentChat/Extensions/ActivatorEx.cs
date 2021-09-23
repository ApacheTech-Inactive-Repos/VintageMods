using System;

namespace VintageMods.Core.FluentChat.Extensions
{
    public static class ActivatorEx
    {
        /// <summary>
        ///     Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of object to create.
        /// </typeparam>
        /// <param name="args">
        ///     An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        ///     If args is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>A reference to the newly created object.</returns>
        public static T CreateInstance<T>(params object[] args) where T : class
        {
            return (T) Activator.CreateInstance(typeof(T), args);
        }
    }
}