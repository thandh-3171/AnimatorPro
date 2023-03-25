

using UnityEngine.Animations;

namespace AnimatorPro
{
    /// <summary>[Pro-Only]
    /// A base class that allows Animation Jobs to be easily inserted into an Animancer graph.
    /// </summary>
    /// <remarks>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/ik#animated-properties">Animated Properties</see>
    /// </remarks>
    /// <example><see href="https://website/animatorpro/docs/examples/jobs">Animation Jobs</see></example>
    /// https://website/animatorpro/api/AnimatorPro/AnimatorProJob_1
    /// 
    public abstract class AnimancerJob<T> where T : struct, IAnimationJob
    {
        /************************************************************************************************************************/

        /// <summary>The <see cref="IAnimationJob"/>.</summary>
        protected T _Job;

        /// <summary>The <see cref="AnimationScriptPlayable"/> running the job.</summary>
        protected AnimationScriptPlayable _Playable;

        /************************************************************************************************************************/

        /// <summary>Creates the <see cref="_Playable"/> and inserts it between the root and the graph output.</summary>
        protected void CreatePlayable(AnimancerPlayable animancer)
        {
            _Playable = animancer.InsertOutputJob(_Job);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Destroys the <see cref="_Playable"/> and restores the graph connection it was intercepting.
        /// </summary>
        /// <remarks>
        /// This method is NOT called automatically, so if you need to guarantee that things will get cleaned up you
        /// should use <see cref="AnimancerPlayable.Disposables"/>.
        /// </remarks>
        public virtual void Destroy()
        {
            AnimancerUtilities.RemovePlayable(_Playable);
        }

        /************************************************************************************************************************/
    }
}
