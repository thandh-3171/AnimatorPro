using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace AnimatorPro
{
    /// <summary>
    /// An <see cref="AnimancerComponent"/> which uses the <see cref="Object.name"/>s of <see cref="AnimationClip"/>s
    /// so they can be referenced using strings as well as the clips themselves.
    /// </summary>
    /// 
    /// <remarks>
    /// It also has fields to automatically register animations on startup and play the first one automatically without
    /// needing another script to control it, much like Unity's Legacy <see cref="Animation"/> component.
    /// <para></para>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/playing/component-types">Component Types</see>
    /// </remarks>
    /// 
    /// <example><see href="https://website/animatorpro/docs/examples/basics/named-animations">Named Animations</see></example>
    /// 
    /// https://website/animatorpro/api/AnimatorPro/NamedAnimancerComponent
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Named Animator Pro Component")]
    [HelpURL(Strings.DocsURLs.APIDocumentation + "/" + nameof(NamedAnimatorProComponent))]
    public class NamedAnimatorProComponent : AnimancerComponent
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        [SerializeField, Tooltip("If true, the 'Default Animation' will be automatically played by OnEnable")]
        private bool _PlayAutomatically = true;

        /// <summary>[<see cref="SerializeField"/>]
        /// If true, the first clip in the <see cref="Animations"/> array will be automatically played by
        /// <see cref="OnEnable"/>.
        /// </summary>
        public ref bool PlayAutomatically => ref _PlayAutomatically;

        /************************************************************************************************************************/

        [SerializeField, Tooltip("Animations in this array will be automatically registered by Awake" +
            " as states that can be retrieved using their name")]
        private AnimationClip[] _Animations;

        /// <summary>[<see cref="SerializeField"/>]
        /// Animations in this array will be automatically registered by <see cref="Awake"/> as states that can be
        /// retrieved using their name and the first element will be played by <see cref="OnEnable"/> if
        /// <see cref="PlayAutomatically"/> is true.
        /// </summary>
        public AnimationClip[] Animations
        {
            get => _Animations;
            set
            {
                _Animations = value;
                States.CreateIfNew(value);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The first element in the <see cref="Animations"/> array. It will be automatically played by
        /// <see cref="OnEnable"/> if <see cref="PlayAutomatically"/> is true.
        /// </summary>
        public AnimationClip DefaultAnimation
        {
            get => _Animations.IsNullOrEmpty() ? null : _Animations[0];
            set
            {
                if (_Animations.IsNullOrEmpty())
                    _Animations = new AnimationClip[] { value };
                else
                    _Animations[0] = value;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Methods
        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Uses <see cref="ClipState.ValidateClip"/> to ensure that all of the clips in the <see cref="Animations"/>
        /// array are supported by the <see cref="AnimatorPro"/> system and removes any others.
        /// </summary>
        /// <remarks>Called in Edit Mode whenever this script is loaded or a value is changed in the Inspector.</remarks>
        protected virtual void OnValidate()
        {
            if (_Animations == null)
                return;

            for (int i = 0; i < _Animations.Length; i++)
            {
                var clip = _Animations[i];
                if (clip == null)
                    continue;

                try
                {
                    Validate.AssertNotLegacy(clip);
                    continue;
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception, clip);
                }

                Array.Copy(_Animations, i + 1, _Animations, i, _Animations.Length - (i + 1));
                Array.Resize(ref _Animations, _Animations.Length - 1);
                i--;
            }
        }
#endif

        /************************************************************************************************************************/

        /// <summary>Creates a state for each clip in the <see cref="Animations"/> array.</summary>
        /// <remarks>Called by Unity when this component is being loaded.</remarks>
        protected virtual void Awake()
        {
            States.CreateIfNew(_Animations);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Plays the first clip in the <see cref="Animations"/> array if <see cref="PlayAutomatically"/> is true.
        /// <para></para>
        /// Ensures that the <see cref="PlayableGraph"/> is playing.
        /// </summary>
        /// <remarks>Called by Unity when this component becomes enabled and active.</remarks>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (_PlayAutomatically && !_Animations.IsNullOrEmpty())
            {
                var clip = _Animations[0];
                if (clip != null)
                    Play(clip);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the clip's name. This method is used to determine the dictionary key to use for an animation when
        /// none is specified by the user, such as in <see cref="AnimancerComponent.Play(AnimationClip)"/>.
        /// </summary>
        public override object GetKey(AnimationClip clip) => clip.name;

        /************************************************************************************************************************/

        public override void GatherAnimationClips(ICollection<AnimationClip> clips)
        {
            base.GatherAnimationClips(clips);
            clips.Gather(_Animations);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
