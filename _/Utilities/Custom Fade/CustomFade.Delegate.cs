

using System;

namespace AnimatorPro
{
    /// https://website/animatorpro/api/AnimatorPro/CustomFade
    /// 
    public partial class CustomFade
    {
        /************************************************************************************************************************/

        /// <summary>Modify the current fade to use the specified `calculateWeight` delegate.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        /// <remarks>The `calculateWeight` should follow the <see cref="OptionalWarning.CustomFadeBounds"/> guideline.</remarks>
        public static void Apply(AnimancerComponent animancer, Func<float, float> calculateWeight)
            => Apply(animancer.States.Current, calculateWeight);

        /// <summary>Modify the current fade to use the specified `calculateWeight` delegate.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        /// <remarks>The `calculateWeight` should follow the <see cref="OptionalWarning.CustomFadeBounds"/> guideline.</remarks>
        public static void Apply(AnimancerPlayable animancer, Func<float, float> calculateWeight)
            => Apply(animancer.States.Current, calculateWeight);

        /// <summary>Modify the current fade to use the specified `calculateWeight` delegate.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        /// <remarks>The `calculateWeight` should follow the <see cref="OptionalWarning.CustomFadeBounds"/> guideline.</remarks>
        public static void Apply(AnimatorProState state, Func<float, float> calculateWeight)
            => Delegate.Acquire(calculateWeight).Apply(state);

        /// <summary>Modify the current fade to use the specified `calculateWeight` delegate.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        /// <remarks>The `calculateWeight` should follow the <see cref="OptionalWarning.CustomFadeBounds"/> guideline.</remarks>
        public static void Apply(AnimancerNode node, Func<float, float> calculateWeight)
            => Delegate.Acquire(calculateWeight).Apply(node);

        /************************************************************************************************************************/

        /// <summary>Modify the current fade to use the specified `function` to calculate the weight.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        public static void Apply(AnimancerComponent animancer, Easing.Function function)
            => Apply(animancer.States.Current, function);

        /// <summary>Modify the current fade to use the specified `function` to calculate the weight.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        public static void Apply(AnimancerPlayable animancer, Easing.Function function)
            => Apply(animancer.States.Current, function);

        /// <summary>Modify the current fade to use the specified `function` to calculate the weight.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        public static void Apply(AnimatorProState state, Easing.Function function)
            => Delegate.Acquire(function.GetDelegate()).Apply(state);

        /// <summary>Modify the current fade to use the specified `function` to calculate the weight.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        public static void Apply(AnimancerNode node, Easing.Function function)
            => Delegate.Acquire(function.GetDelegate()).Apply(node);

        /************************************************************************************************************************/

        /// <summary>A <see cref="CustomFade"/> which uses a <see cref="Func{T, TResult}"/> to calculate the weight.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        private sealed class Delegate : CustomFade
        {
            /************************************************************************************************************************/

            private Func<float, float> _CalculateWeight;

            /************************************************************************************************************************/

            public static Delegate Acquire(Func<float, float> calculateWeight)
            {
                if (calculateWeight == null)
                {
                    OptionalWarning.CustomFadeNotNull.Log($"{nameof(calculateWeight)} is null.");
                    return null;
                }

                var fade = ObjectPool<Delegate>.Acquire();
                fade._CalculateWeight = calculateWeight;
                return fade;
            }

            /************************************************************************************************************************/

            protected override float CalculateWeight(float progress) => _CalculateWeight(progress);

            /************************************************************************************************************************/

            protected override void Release() => ObjectPool<Delegate>.Release(this);

            /************************************************************************************************************************/
        }
    }
}
