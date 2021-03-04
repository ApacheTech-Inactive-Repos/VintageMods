using System;

namespace VintageMods.Core.Fluent.Extensions
{
    public static class ActivatorEx
    {
        public static T CreateInstance<T>(params object[] args) where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}