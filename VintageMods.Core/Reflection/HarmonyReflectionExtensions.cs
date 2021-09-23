using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using VintageMods.Core.Extensions;

namespace VintageMods.Core.Reflection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class HarmonyReflectionExtensions
    {
        #region Fields

        public static T GetField<T>(this object instance, string fieldName)
        {
            return (T)AccessTools.Field(instance.GetType(), fieldName).GetValue(instance);
        }

        public static T[] GetFields<T>(this object instance)
        {
            var declaredFields =
                AccessTools.GetDeclaredFields(instance.GetType())?.Where(t => t.FieldType == typeof(T));
            return declaredFields?.Select(val => instance.GetField<T>(val.Name)).ToArray();
        }

        public static void SetField(this object instance, string fieldName, object setVal)
        {
            AccessTools.Field(instance.GetType(), fieldName).SetValue(instance, setVal);
        }


        #endregion

        #region Properties

        public static T GetProperty<T>(this object instance, string propertyName)
        {
            return (T)AccessTools.Property(instance.GetType(), propertyName).GetValue(instance);
        }

        public static void SetProperty(this object instance, string propertyName, object setVal)
        {
            AccessTools.Property(instance.GetType(), propertyName).SetValue(instance, setVal);
        }

        #endregion

        #region Methods

        public static T CallMethod<T>(this object instance, string method, params object[] args)
        {
            return (T)AccessTools.Method(instance.GetType(), method).Invoke(instance, args);
        }

        public static void CallMethod(this object instance, string method, params object[] args)
        {
            AccessTools.Method(instance.GetType(), method)?.Invoke(instance, args);
        }

        public static void CallMethod(this object instance, string method)
        {
            AccessTools.Method(instance.GetType(), method)?.Invoke(instance, null);
        }

        public static MethodInfo GetMethod(this object instance, string method, Type[] parameters = null, Type[] generics = null)
        {
            return AccessTools.Method(instance.GetType(), method, parameters, generics);
        }

        #endregion

        #region Classes

        public static object CreateInstance(this Type type)
        {
            return AccessTools.CreateInstance(type);
        }

        [CanBeNull]
        public static Type GetClassType(this Assembly assembly, string className)
        {
            var ts = AccessTools.GetTypesFromAssembly(assembly);
            return ts.FirstOrNull(t => t.Name == className);
        }

        #endregion

        #region Objects

        /// <summary>
        ///     Makes a deep copy of any object.
        /// </summary>
        /// <typeparam name="T">The type of the instance that should be created; for legacy reasons, this must be a class or interface.</typeparam>
        /// <param name="source">The original object.</param>
        /// <returns>A copy of the original object but of type <typeparamref name="T"/></returns>
        public static T DeepClone<T>(this T source) where T : class
        {
            return AccessTools.MakeDeepCopy<T>(source);
        }

        #endregion
    }
}