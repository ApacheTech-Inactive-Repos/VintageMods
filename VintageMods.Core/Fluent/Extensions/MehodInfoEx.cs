using System;
using System.Reflection;

namespace VintageMods.Core.Fluent.Extensions
{
    public static class DelegateEx
    {
        public static TDelegate CreateDelegate<TDelegate>(object instance, MethodInfo method) where TDelegate : Delegate
        {
            var newDelegate = (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), instance, method);
            return newDelegate;
        }
    }
}
