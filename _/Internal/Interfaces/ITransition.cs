

using Object = UnityEngine.Object;

namespace AnimatorPro
{
    /// <summary>An object which can create an <see cref="AnimatorProState"/> and set its details.</summary>
    /// <remarks>
    /// Transitions are generally used as arguments for <see cref="AnimancerPlayable.Play(ITransition)"/>.
    /// <para></para>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/transitions">Transitions</see>
    /// </remarks>
    /// https://website/animatorpro/api/AnimatorPro/ITransition
    /// 
    public interface ITransition : IHasKey, IPolymorphic
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Creates and returns a new <see cref="AnimatorProState"/>.
        /// <para></para>
        /// Note that using methods like <see cref="AnimancerPlayable.Play(ITransition)"/> will also call
        /// <see cref="Apply"/>, so if you call this method manually you may want to call that method as well. Or you
        /// can just use <see cref="AnimancerUtilities.CreateStateAndApply"/>.
        /// </summary>
        /// <remarks>
        /// The first time a transition is used on an object, this method is called to create the state and register it
        /// in the internal dictionary using the <see cref="IHasKey.Key"/> so that it can be reused later on.
        /// </remarks>
        AnimatorProState CreateState();

        /// <summary>The amount of time this transition should take (in seconds).</summary>
        float FadeDuration { get; }

        /// <summary>
        /// The <see cref="AnimatorPro.FadeMode"/> which should be used when this transition is passed into
        /// <see cref="AnimancerPlayable.Play(ITransition)"/>.
        /// </summary>
        FadeMode FadeMode { get; }

        /// <summary>
        /// Called by <see cref="AnimancerPlayable.Play(ITransition)"/> to apply any modifications to the `state`.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="CreateState"/>, this method is called every time the transition is used so it can do
        /// things like set the <see cref="AnimatorProState.Events"/> or starting <see cref="AnimatorProState.Time"/>.
        /// </remarks>
        void Apply(AnimatorProState state);

        /************************************************************************************************************************/
    }

    /// <summary>An <see cref="ITransition"/> which creates a specific type of <see cref="AnimatorProState"/>.</summary>
    /// <remarks>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/transitions">Transitions</see>
    /// </remarks>
    /// https://website/animatorpro/api/AnimatorPro/ITransition_1
    /// 
    public interface ITransition<TState> : ITransition where TState : AnimatorProState
    {
        /************************************************************************************************************************/

        /// <summary>
        /// The state that was created by this object. Specifically, this is the state that was most recently
        /// passed into <see cref="ITransition.Apply"/> (usually by <see cref="AnimancerPlayable.Play(ITransition)"/>).
        /// </summary>
        TState State { get; }

        /************************************************************************************************************************/

        /// <summary>Creates and returns a new <typeparamref name="TState"/>.</summary>
        new TState CreateState();

        /************************************************************************************************************************/
    }
}

