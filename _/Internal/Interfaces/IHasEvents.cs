

namespace AnimatorPro
{
    /// <summary>An object which has an <see cref="AnimancerEvent.Sequence.Serializable"/>.</summary>
    /// <remarks>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/events/animancer">Animancer Events</see>
    /// </remarks>
    /// https://website/animatorpro/api/AnimatorPro/IHasEvents
    /// 
    public interface IHasEvents
    {
        /************************************************************************************************************************/

        /// <summary>Events which will be triggered as the animation plays.</summary>
        AnimancerEvent.Sequence Events { get; }

        /// <summary>Events which will be triggered as the animation plays.</summary>
        ref AnimancerEvent.Sequence.Serializable SerializedEvents { get; }

        /************************************************************************************************************************/
    }

    /// <summary>A combination of <see cref="ITransition"/> and <see cref="IHasEvents"/>.</summary>
    /// https://website/animatorpro/api/AnimatorPro/ITransitionWithEvents
    /// 
    public interface ITransitionWithEvents : ITransition, IHasEvents { }
}

