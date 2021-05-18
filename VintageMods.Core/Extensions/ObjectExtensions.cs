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
            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Object:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return (T)obj;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}