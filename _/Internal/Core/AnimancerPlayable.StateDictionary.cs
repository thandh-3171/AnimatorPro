

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatorPro
{
    /// https://website/animatorpro/api/AnimatorPro/AnimatorProPlayable
    /// 
    partial class AnimancerPlayable
    {
        /// <summary>A dictionary of <see cref="AnimatorProState"/>s mapped to their <see cref="AnimatorProState.Key"/>.</summary>
        /// <remarks>
        /// Documentation: <see href="https://website/animatorpro/docs/manual/playing/states">States</see>
        /// </remarks>
        /// https://website/animatorpro/api/AnimatorPro/StateDictionary
        /// 
        public sealed class StateDictionary : IEnumerable<AnimatorProState>, IAnimationClipCollection
        {
            /************************************************************************************************************************/

            /// <summary>The <see cref="AnimancerPlayable"/> at the root of the graph.</summary>
            private readonly AnimancerPlayable Root;

            /************************************************************************************************************************/

            /// <summary>
            /// Determines the <see cref="IEqualityComparer{T}"/> used by every new <see cref="StateDictionary"/> when
            /// it is created. Changing this value will not affect existing instances.
            /// <list type="bullet">
            /// <item>The default is false, which will use a <see cref="FastComparer"/>.</item>
            /// <item>Setting it to true will use a <see cref="FastReferenceComparer"/>, which is faster but does not
            /// work for value types such as enums because it uses <see cref="object.ReferenceEquals"/>.</item>
            /// </list>
            /// </summary>
            public static bool ReferenceKeysOnly { get; set; }

            /// <summary><see cref="AnimatorProState.Key"/> mapped to <see cref="AnimatorProState"/>.</summary>
            private readonly Dictionary<object, AnimatorProState>
                States = new Dictionary<object, AnimatorProState>(
                    ReferenceKeysOnly ? (IEqualityComparer<object>)FastReferenceComparer.Instance : FastComparer.Instance);

            /************************************************************************************************************************/

            /// <summary>[Internal] Creates a new <see cref="StateDictionary"/>.</summary>
            internal StateDictionary(AnimancerPlayable root) => Root = root;

            /************************************************************************************************************************/

            /// <summary>The number of states that have been registered with a <see cref="AnimatorProState.Key"/>.</summary>
            public int Count => States.Count;

            /************************************************************************************************************************/
            #region Create
            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="ClipState"/> to play the `clip`.
            /// <para></para>
            /// To create a state on a different layer, use <c>animancer.Layers[x].CreateState(clip)</c> instead.
            /// </summary>
            /// <remarks>
            /// <see cref="GetKey"/> is used to determine the <see cref="AnimatorProState.Key"/>.
            /// </remarks>
            public ClipState Create(AnimationClip clip) => Root.Layers[0].CreateState(clip);

            /// <summary>
            /// Creates and returns a new <see cref="ClipState"/> to play the `clip` and registers it with the `key`.
            /// <para></para>
            /// To create a state on a different layer, use <c>animancer.Layers[x].CreateState(key, clip)</c> instead.
            /// </summary>
            public ClipState Create(object key, AnimationClip clip) => Root.Layers[0].CreateState(key, clip);

            /************************************************************************************************************************/

            /// <summary>
            /// Calls <see cref="GetOrCreate(AnimationClip, bool)"/> for each of the specified clips.
            /// <para></para>
            /// If you only want to create a single state, use <see cref="AnimancerLayer.CreateState(AnimationClip)"/>.
            /// </summary>
            public void CreateIfNew(AnimationClip clip0, AnimationClip clip1)
            {
                GetOrCreate(clip0);
                GetOrCreate(clip1);
            }

            /// <summary>
            /// Calls <see cref="GetOrCreate(AnimationClip, bool)"/> for each of the specified clips.
            /// <para></para>
            /// If you only want to create a single state, use <see cref="AnimancerLayer.CreateState(AnimationClip)"/>.
            /// </summary>
            public void CreateIfNew(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2)
            {
                GetOrCreate(clip0);
                GetOrCreate(clip1);
                GetOrCreate(clip2);
            }

            /// <summary>
            /// Calls <see cref="GetOrCreate(AnimationClip, bool)"/> for each of the specified clips.
            /// <para></para>
            /// If you only want to create a single state, use <see cref="AnimancerLayer.CreateState(AnimationClip)"/>.
            /// </summary>
            public void CreateIfNew(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, AnimationClip clip3)
            {
                GetOrCreate(clip0);
                GetOrCreate(clip1);
                GetOrCreate(clip2);
                GetOrCreate(clip3);
            }

            /// <summary>Calls <see cref="GetOrCreate(AnimationClip, bool)"/> for each of the specified `clips`.</summary>
            /// <remarks>To create a single state, use <see cref="AnimancerLayer.CreateState(AnimationClip)"/> instead.</remarks>
            public void CreateIfNew(params AnimationClip[] clips)
            {
                if (clips == null)
                    return;

                var count = clips.Length;
                for (int i = 0; i < count; i++)
                {
                    var clip = clips[i];
                    if (clip != null)
                        GetOrCreate(clip);
                }
            }

            /************************************************************************************************************************/
            #endregion
            /************************************************************************************************************************/
            #region Access
            /************************************************************************************************************************/

            /// <summary>
            /// The <see cref="AnimancerLayer.CurrentState"/> on layer 0.
            /// <para></para>
            /// Specifically, this is the state that was most recently started using any of the Play methods on that layer.
            /// States controlled individually via methods in the <see cref="AnimatorProState"/> itself will not register in
            /// this property.
            /// </summary>
            public AnimatorProState Current => Root.Layers[0].CurrentState;

            /************************************************************************************************************************/

            /// <summary>Calls <see cref="GetKey"/> then returns the state registered with that key.</summary>
            /// <exception cref="ArgumentNullException">The key is null.</exception>
            /// <exception cref="KeyNotFoundException">No state is registered with the key.</exception>
            public AnimatorProState this[AnimationClip clip] => States[Root.GetKey(clip)];

            /// <summary>Returns the state registered with the <see cref="IHasKey.Key"/>.</summary>
            /// <exception cref="ArgumentNullException">The `key` is null.</exception>
            /// <exception cref="KeyNotFoundException">No state is registered with the `key`.</exception>
            public AnimatorProState this[IHasKey hasKey] => States[hasKey.Key];

            /// <summary>Returns the state registered with the `key`.</summary>
            /// <exception cref="ArgumentNullException">The `key` is null.</exception>
            /// <exception cref="KeyNotFoundException">No state is registered with the `key`.</exception>
            public AnimatorProState this[object key] => States[key];

            /************************************************************************************************************************/

            /// <summary>
            /// Calls <see cref="GetKey"/> then passes the key to
            /// <see cref="TryGet(object, out AnimatorProState)"/> and returns the result.
            /// </summary>
            public bool TryGet(AnimationClip clip, out AnimatorProState state)
            {
                if (clip == null)
                {
                    state = null;
                    return false;
                }

                return TryGet(Root.GetKey(clip), out state);
            }

            /// <summary>
            /// Passes the <see cref="IHasKey.Key"/> into <see cref="TryGet(object, out AnimatorProState)"/>
            /// and returns the result.
            /// </summary>
            public bool TryGet(IHasKey hasKey, out AnimatorProState state)
            {
                if (hasKey == null)
                {
                    state = null;
                    return false;
                }

                return TryGet(hasKey.Key, out state);
            }

            /// <summary>
            /// If a `state` is registered with the `key`, this method outputs it and returns true. Otherwise the
            /// `state` is set to null and this method returns false.
            /// </summary>
            public bool TryGet(object key, out AnimatorProState state)
            {
                if (key == null)
                {
                    state = null;
                    return false;
                }

                return States.TryGetValue(key, out state);
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Calls <see cref="GetKey"/> and returns the state which registered with that key or creates one if it
            /// doesn't exist.
            /// <para></para>
            /// If the state already exists but has the wrong <see cref="AnimatorProState.Clip"/>, the `allowSetClip`
            /// parameter determines what will happen. False causes it to throw an <see cref="ArgumentException"/> while
            /// true allows it to change the <see cref="AnimatorProState.Clip"/>. Note that the change is somewhat costly to
            /// performance so use with caution.
            /// </summary>
            /// <exception cref="ArgumentException"/>
            public AnimatorProState GetOrCreate(AnimationClip clip, bool allowSetClip = false)
                => GetOrCreate(Root.GetKey(clip), clip, allowSetClip);

            /// <summary>
            /// Returns the state registered with the `transition`s <see cref="IHasKey.Key"/> if there is one. Otherwise
            /// this method uses <see cref="ITransition.CreateState"/> to create a new one and registers it with
            /// that key before returning it.
            /// </summary>
            public AnimatorProState GetOrCreate(ITransition transition)
            {
                var key = transition.Key;
                if (!TryGet(key, out var state))
                {
                    state = transition.CreateState();
                    state.SetRoot(Root);
                    state._Key = key;
                    Register(state);
                }

                return state;
            }

            /// <summary>
            /// Returns the state which registered with the `key` or creates one if it doesn't exist.
            /// <para></para>
            /// If the state already exists but has the wrong <see cref="AnimatorProState.Clip"/>, the `allowSetClip`
            /// parameter determines what will happen. False causes it to throw an <see cref="ArgumentException"/> while
            /// true allows it to change the <see cref="AnimatorProState.Clip"/>. Note that the change is somewhat costly to
            /// performance to use with caution.
            /// </summary>
            /// <exception cref="ArgumentException"/>
            /// <remarks>See also: <see cref="AnimancerLayer.GetOrCreateState(object, AnimationClip, bool)"/></remarks>
            public AnimatorProState GetOrCreate(object key, AnimationClip clip, bool allowSetClip = false)
            {
                if (TryGet(key, out var state))
                {
                    // If a state exists with the 'key' but has the wrong clip, either change it or complain.
                    if (!ReferenceEquals(state.Clip, clip))
                    {
                        if (allowSetClip)
                        {
                            state.Clip = clip;
                        }
                        else
                        {
                            throw new ArgumentException(GetClipMismatchError(key, state.Clip, clip));
                        }
                    }
                }
                else
                {
                    state = Root.Layers[0].CreateState(key, clip);
                }

                return state;
            }

            /************************************************************************************************************************/

            /// <summary>Returns an error message explaining that a state already exists with the specified `key`.</summary>
            public static string GetClipMismatchError(object key, AnimationClip oldClip, AnimationClip newClip)
                => $"A state already exists using the specified '{nameof(key)}', but has a different {nameof(AnimationClip)}:" +
                $"\n - Key: {key}" +
                $"\n - Old Clip: {oldClip}" +
                $"\n - New Clip: {newClip}";

            /************************************************************************************************************************/

            /// <summary>[Internal]
            /// Registers the `state` in this dictionary so the <see cref="AnimatorProState.Key"/> can be used to get it
            /// later on using any of the lookup methods such as <see cref="this[object]"/> or
            /// <see cref="TryGet(object, out AnimatorProState)"/>.
            /// </summary>
            /// <remarks>Does nothing if the <see cref="AnimatorProState.Key"/> is <c>null</c>.</remarks>
            internal void Register(AnimatorProState state)
            {
                var key = state._Key;
                if (key != null)
                {
#if UNITY_ASSERTIONS
                    if (state.Root != Root)
                        throw new ArgumentException(
                            $"{nameof(StateDictionary)} cannot register a state with a different {nameof(Root)}: " + state);
#endif

                    States.Add(key, state);
                }
            }

            /// <summary>[Internal] Removes the `state` from this dictionary (the opposite of <see cref="Register"/>).</summary>
            internal void Unregister(AnimatorProState state)
            {
                var key = state._Key;
                if (key != null)
                    States.Remove(key);
            }

            /************************************************************************************************************************/
            #region Enumeration
            /************************************************************************************************************************/
            // IEnumerable for 'foreach' statements.
            /************************************************************************************************************************/

            /// <summary>Returns an enumerator that will iterate through all registered states.</summary>
            public Dictionary<object, AnimatorProState>.ValueCollection.Enumerator GetEnumerator()
                => States.Values.GetEnumerator();

            /// <inheritdoc/>
            IEnumerator<AnimatorProState> IEnumerable<AnimatorProState>.GetEnumerator()
                => GetEnumerator();

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            /************************************************************************************************************************/

            /// <summary>[<see cref="IAnimationClipCollection"/>]
            /// Adds all the animations of states with a <see cref="AnimatorProState.Key"/> to the `clips`.
            /// </summary>
            public void GatherAnimationClips(ICollection<AnimationClip> clips)
            {
                foreach (var state in States.Values)
                    clips.GatherFromSource(state);
            }

            /************************************************************************************************************************/
            #endregion
            /************************************************************************************************************************/
            #endregion
            /************************************************************************************************************************/
            #region Destroy
            /************************************************************************************************************************/

            /// <summary>
            /// Calls <see cref="AnimatorProState.Destroy"/> on the state associated with the `clip` (if any).
            /// Returns true if the state existed.
            /// </summary>
            public bool Destroy(AnimationClip clip)
            {
                if (clip == null)
                    return false;

                return Destroy(Root.GetKey(clip));
            }

            /// <summary>
            /// Calls <see cref="AnimatorProState.Destroy"/> on the state associated with the <see cref="IHasKey.Key"/>
            /// (if any). Returns true if the state existed.
            /// </summary>
            public bool Destroy(IHasKey hasKey)
            {
                if (hasKey == null)
                    return false;

                return Destroy(hasKey.Key);
            }

            /// <summary>
            /// Calls <see cref="AnimatorProState.Destroy"/> on the state associated with the `key` (if any).
            /// Returns true if the state existed.
            /// </summary>
            public bool Destroy(object key)
            {
                if (!TryGet(key, out var state))
                    return false;

                state.Destroy();
                return true;
            }

            /************************************************************************************************************************/

            /// <summary>Calls <see cref="Destroy(AnimationClip)"/> on each of the `clips`.</summary>
            public void DestroyAll(IList<AnimationClip> clips)
            {
                if (clips == null)
                    return;

                for (int i = clips.Count - 1; i >= 0; i--)
                    Destroy(clips[i]);
            }

            /// <summary>Calls <see cref="Destroy(AnimationClip)"/> on each of the `clips`.</summary>
            public void DestroyAll(IEnumerable<AnimationClip> clips)
            {
                if (clips == null)
                    return;

                foreach (var clip in clips)
                    Destroy(clip);
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Calls <see cref="Destroy(AnimationClip)"/> on all states gathered by
            /// <see cref="IAnimationClipSource.GetAnimationClips"/>.
            /// </summary>
            public void DestroyAll(IAnimationClipSource source)
            {
                if (source == null)
                    return;

                var clips = ObjectPool.AcquireList<AnimationClip>();
                source.GetAnimationClips(clips);
                DestroyAll(clips);
                ObjectPool.Release(clips);
            }

            /// <summary>
            /// Calls <see cref="Destroy(AnimationClip)"/> on all states gathered by
            /// <see cref="IAnimationClipCollection.GatherAnimationClips"/>.
            /// </summary>
            public void DestroyAll(IAnimationClipCollection source)
            {
                if (source == null)
                    return;

                var clips = ObjectPool.AcquireSet<AnimationClip>();
                source.GatherAnimationClips(clips);
                DestroyAll(clips);
                ObjectPool.Release(clips);
            }

            /************************************************************************************************************************/
            #endregion
            /************************************************************************************************************************/
            #region Key Error Methods
#if UNITY_EDITOR
            /************************************************************************************************************************/
            // These are overloads of other methods that take a System.Object key to ensure the user doesn't try to use an
            // AnimancerState as a key, since the whole point of a key is to identify a state in the first place.
            /************************************************************************************************************************/

            /// <summary>[Warning]
            /// You should not use an <see cref="AnimancerState"/> as a key.
            /// The whole point of a key is to identify a state in the first place.
            /// </summary>
            [Obsolete("You should not use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
            public AnimatorProState this[AnimatorProState key] => key;

            /// <summary>[Warning]
            /// You should not use an <see cref="AnimatorProState"/> as a key.
            /// The whole point of a key is to identify a state in the first place.
            /// </summary>
            [Obsolete("You should not use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
            public bool TryGet(AnimatorProState key, out AnimatorProState state)
            {
                state = key;
                return true;
            }

            /// <summary>[Warning]
            /// You should not use an <see cref="AnimatorProState"/> as a key.
            /// The whole point of a key is to identify a state in the first place.
            /// </summary>
            [Obsolete("You should not use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
            public AnimatorProState GetOrCreate(AnimatorProState key, AnimationClip clip) => key;

            /// <summary>[Warning]
            /// You should not use an <see cref="AnimatorProState"/> as a key.
            /// Just call <see cref="AnimatorProState.Destroy"/>.
            /// </summary>
            [Obsolete("You should not use an AnimancerState as a key. Just call AnimancerState.Destroy.", true)]
            public bool Destroy(AnimatorProState key)
            {
                key.Destroy();
                return true;
            }

            /************************************************************************************************************************/
#endif
            #endregion
            /************************************************************************************************************************/
        }
    }
}

