using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace VintageMods.Core.Extensions
{
    /// <summary>
    ///     Dynamically associates properties to an instance of an object.
    /// </summary>
    /// <example>
    ///     var jan = new Person("Jan");
    ///     jan.Age = 24; // regular property of the person object;
    ///     jan.DynamicProperties().NumberOfDrinkingBuddies = 27; // not originally scoped to the person object.
    ///     if (jan.Age &lt; jan.DynamicProperties().NumberOfDrinkingBuddies)
    ///     Console.WriteLine("Jan drinks too much");
    /// </example>
    /// <remarks>
    ///     If you get 'Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create' you should reference Microsoft.CSharp
    /// </remarks>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ObjectExtensions
    {
        private static readonly ConditionalWeakTable<object, object> ExtendedData = new();

        /// <summary>
        ///     Gets a dynamic collection of properties associated with an object instance, with a lifetime scoped to the lifetime of the object
        /// </summary>
        /// <param name="obj">The object the properties are associated with.</param>
        /// <returns>A dynamic collection of properties associated with an object instance.</returns>
        public static dynamic DynamicProperties(this object obj) => ExtendedData.GetValue(obj, _ => new ExpandoObject());


        /// <summary>
        ///     Dynamically casts the object instance to a specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to cast to.</typeparam>
        /// <param name="obj">The instance to cast.</param>
        /// <returns>An instance of Type <typeparamref name="T"/>.</returns>
        public static T As<T>(this object obj)
        {
            return Type.GetTypeCode(typeof(T)) is TypeCode.DateTime or TypeCode.DBNull or TypeCode.Empty
                ? throw new ArgumentOutOfRangeException(nameof(T), "Objects of this TypeCode cannot be cast to dynamically.")
                : (T)obj;
        }
    }
}