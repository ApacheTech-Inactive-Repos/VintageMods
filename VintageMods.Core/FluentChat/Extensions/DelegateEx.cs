using System;
using System.Reflection;

namespace VintageMods.Core.FluentChat.Extensions
{
    public static class DelegateEx
    {
        /// <summary>
        ///     Creates a delegate of the specified type that represents the specified static or instance method, with the
        ///     specified first argument.
        /// </summary>
        /// <typeparam name="TDelegate">The <see cref="T:System.Type"></see> of delegate to create.</typeparam>
        /// <param name="instance">
        ///     The object to which the delegate is bound, or null to treat method as static (Shared in Visual
        ///     Basic).
        /// </param>
        /// <param name="method">
        ///     The <see cref="T:System.Reflection.MethodInfo"></see> describing the static or instance method the
        ///     delegate is to represent.
        /// </param>
        /// <returns>A delegate of the specified type that represents the specified static or instance method.</returns>
        public static TDelegate CreateDelegate<TDelegate>(object instance, MethodInfo method) where TDelegate : Delegate
        {
            return (TDelegate) Delegate.CreateDelegate(typeof(TDelegate), instance, method);
        }
    }
}