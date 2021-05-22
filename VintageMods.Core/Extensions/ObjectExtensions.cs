using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

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
    public static class ObjectExtensions
    {
        private static readonly ConditionalWeakTable<object, object> ExtendedData = new();

        /// <summary>
        ///     Gets a dynamic collection of properties associated with an object instance, with a lifetime scoped to the lifetime of the object
        /// </summary>
        /// <param name="obj">The object the properties are associated with.</param>
        /// <returns>A dynamic collection of properties associated with an object instance.</returns>
        public static dynamic DynamicProperties(this object obj) => ExtendedData.GetValue(obj, _ => new ExpandoObject());

        
        public static T As<T>(this object obj)
        {
            var typeCode = Type.GetTypeCode(typeof(T));
            return typeCode switch
            {
                TypeCode.Boolean => (T) obj,
                TypeCode.Byte => (T) obj,
                TypeCode.Char => (T) obj,
                TypeCode.Decimal => (T) obj,
                TypeCode.Double => (T) obj,
                TypeCode.Int16 => (T) obj,
                TypeCode.Int32 => (T) obj,
                TypeCode.Int64 => (T) obj,
                TypeCode.Object => (T) obj,
                TypeCode.SByte => (T) obj,
                TypeCode.Single => (T) obj,
                TypeCode.String => (T) obj,
                TypeCode.UInt16 => (T) obj,
                TypeCode.UInt32 => (T) obj,
                TypeCode.UInt64 => (T) obj,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}