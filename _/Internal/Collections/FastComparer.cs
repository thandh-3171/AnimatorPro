

using System.Collections.Generic;

namespace AnimatorPro
{
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which ignores overloaded equality operators so it is faster than
    /// <see cref="EqualityComparer{T}.Default"/> for types derived from <see cref="UnityEngine.Object"/>.
    /// </summary>
    /// <remarks>
    /// This class is used when <see cref="AnimancerPlayable.StateDictionary.ReferenceKeysOnly"/> is false.
    /// <para></para>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/playing/states#performance">Performance</see>
    /// </remarks>
    /// https://website/animatorpro/api/AnimatorPro/FastComparer
    /// 
    public sealed class FastComparer : IEqualityComparer<object>
    {
        /************************************************************************************************************************/

        /// <summary>Singleton instance.</summary>
        public static readonly FastComparer Instance = new FastComparer();

        /// <summary>Calls <see cref="object.Equals(object, object)"/>.</summary>
        bool IEqualityComparer<object>.Equals(object x, object y) => Equals(x, y);

        /// <summary>Calls <see cref="object.GetHashCode"/>.</summary>
        int IEqualityComparer<object>.GetHashCode(object obj) => obj.GetHashCode();

        /************************************************************************************************************************/
    }

    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> which uses <see cref="object.ReferenceEquals"/> to be even faster than
    /// <see cref="FastComparer"/>. Unfortunately this means it will not work for boxed value types (such as enums).
    /// </summary>
    /// <remarks>
    /// This class is used when <see cref="AnimancerPlayable.StateDictionary.ReferenceKeysOnly"/> is true.
    /// <para></para>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/playing/states#performance">Performance</see>
    /// </remarks>
    /// https://website/animatorpro/api/AnimatorPro/FastReferenceComparer
    /// 
    public sealed class FastReferenceComparer : IEqualityComparer<object>
    {
        /************************************************************************************************************************/

        /// <summary>Singleton instance.</summary>
        public static readonly FastReferenceComparer Instance = new FastReferenceComparer();

        /// <summary>Calls <see cref="object.ReferenceEquals"/>.</summary>
        bool IEqualityComparer<object>.Equals(object x, object y) => ReferenceEquals(x, y);

        /// <summary>Calls <see cref="object.GetHashCode"/>.</summary>
        int IEqualityComparer<object>.GetHashCode(object obj) => obj.GetHashCode();

        /************************************************************************************************************************/
    }
}

