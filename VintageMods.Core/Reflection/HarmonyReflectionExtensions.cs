using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

namespace VintageMods.Core.Common.Reflection
{
    public static class HarmonyReflectionExtensions
    {
        public static T GetField<T>(this object instance, string fieldname)
        {
            return (T) AccessTools.Field(instance.GetType(), fieldname).GetValue(instance);
        }

        public static T GetProperty<T>(this object instance, string fieldname)
        {
            return (T) AccessTools.Property(instance.GetType(), fieldname).GetValue(instance);
        }

        public static T CallMethod<T>(this object instance, string method, params object[] args)
        {
            return (T) AccessTools.Method(instance.GetType(), method).Invoke(instance, args);
        }

        public static void CallMethod(this object instance, string method, params object[] args)
        {
            AccessTools.Method(instance.GetType(), method)?.Invoke(instance, args);
        }

        public static void CallMethod(this object instance, string method)
        {
            AccessTools.Method(instance.GetType(), method)?.Invoke(instance, null);
        }

        public static object CreateInstance(this Type type)
        {
            return AccessTools.CreateInstance(type);
        }

        public static T[] GetFields<T>(this object instance)
        {
            var declaredFields =
                AccessTools.GetDeclaredFields(instance.GetType())?.Where(t => t.FieldType == typeof(T));
            return declaredFields?.Select(val => instance.GetField<T>(val.Name)).ToArray();
        }

        public static void SetField(this object instance, string fieldname, object setVal)
        {
            AccessTools.Field(instance.GetType(), fieldname).SetValue(instance, setVal);
        }

        public static Type GetClassType(this Assembly assembly, string className)
        {
            var ts = AccessTools.GetTypesFromAssembly(assembly);
            return ts.First(t => t.Name == className);
        }

        public static MethodInfo GetMethod(this object instance, string method)
        {
            return AccessTools.Method(instance.GetType(), method);
        }
    }
}