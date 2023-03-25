

namespace AnimatorPro
{
    /// <summary>Exposes a <see cref="Key"/> object that can be used for dictionaries and hash sets.</summary>
    /// <remarks>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/playing/states#keys">Keys</see>
    /// </remarks>
    /// https://website/animatorpro/api/AnimatorPro/IHasKey
    /// 
    public interface IHasKey
    {
        /************************************************************************************************************************/

        /// <summary>An identifier object that can be used for dictionaries and hash sets.</summary>
        object Key { get; }

        /************************************************************************************************************************/
    }
}

