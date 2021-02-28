using System;
using System.Collections.Generic;
using System.Linq;

#region Resharper Clean Up
    // ReSharper disable ClassNeverInstantiated.Global
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable NonReadonlyMemberInGetHashCode
#endregion

namespace VintageMods.WaypointExtensions.Model
{
    /// <summary>
    ///     A model of all information required to set a Waypoint.
    /// </summary>
    public sealed class WaypointInfoModel : IEquatable<WaypointInfoModel>
    {
        /// <summary>
        ///     Gets the list of syntax arguments that are valid for this waypoint type.
        /// </summary>
        /// <value>The list of syntax arguments that are valid for this waypoint type.</value>
        public List<string> Syntax { get; set; } = new List<string>();

        /// <summary>
        ///     Gets the icon to use for the waypoint.
        /// </summary>
        /// <value>The icon to use for the waypoint.</value>
        public string Icon { get; set; }

        /// <summary>
        ///     Gets the colour to use for the waypoint marker.
        /// </summary>
        /// <value>The colour to use for the waypoint marker.</value>
        public string Colour { get; set; }

        /// <summary>
        ///     Gets the default title of the waypoint marker.
        /// </summary>
        /// <value>The default title of the waypoint marker.</value>
        public string DefaultTitle { get; set; }

        #region Implementation of IEquatable Contract

        /// <summary>
        ///     Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as WaypointInfoModel);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <remarks>
        ///     Guideline: The integer returned by GetHashCode should never change
        ///     Ideally, the hash code of a mutable object should be computed from only fields which cannot
        ///     mutate, and therefore the hash value of an object is the same for its entire lifetime.
        ///     However, this is only an ideal-situation guideline; the actual rule is:
        ///     Rule: The integer returned by GetHashCode must never change while the object is
        ///     contained in a data structure that depends on the hash code remaining stable
        ///     It is permissible, though dangerous, to make an object whose hash code value can mutate as
        ///     the fields of the object mutate.If you have such an object and you put it in a hash table then
        ///     the code which mutates the object and the code which maintains the hash table are required to have
        ///     some agreed-upon protocol that ensures that the object is not mutated while it is in the hash table.
        ///     What that protocol looks like is up to you.
        ///     If an object's hash code can mutate while it is in the hash table then clearly the Contains method stops working.
        ///     You put the object in bucket #5, you mutate it, and when you ask the set whether it contains the mutated object,
        ///     it looks in bucket #74 and doesn't find it.
        ///     Remember, objects can be put into hash tables in ways that you didn't expect. A lot of the LINQ sequence operators
        ///     use hash tables internally. Don't go dangerously mutating objects while enumerating a LINQ query that returns them!
        /// </remarks>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Syntax != null ? Syntax.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Icon != null ? Icon.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Colour != null ? Colour.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DefaultTitle != null ? DefaultTitle.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        ///     Determines whether the specified <see cref="WaypointInfoModel" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="WaypointInfoModel" /> is equal to this instance; otherwise,
        ///     <c>false</c>.
        /// </returns>
        public bool Equals(WaypointInfoModel other)
        {
            return other != null && Syntax.Intersect(other.Syntax).Any();
        }

        #endregion
    }
}