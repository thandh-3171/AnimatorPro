

using UnityEngine;
using UnityEngine.Serialization;

namespace AnimatorPro
{
    /// <summary>
    /// A component which uses Animation Events with the Function Name "Event" to trigger a callback.
    /// </summary>
    /// <remarks>
    /// This component must always be attached to the same <see cref="GameObject"/> as the <see cref="Animator"/> in
    /// order to receive Animation Events from it.
    /// <para></para>
    /// Documentation: <see href="https://website/animatorpro/docs/manual/events/animation#simple-events">Simple Events</see>
    /// </remarks>
    /// https://website/animatorpro/api/AnimatorPro/SimpleEventReceiver
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Simple Event Receiver")]
    [HelpURL(Strings.DocsURLs.APIDocumentation + "/" + nameof(SimpleEventReceiver))]
    public class SimpleEventReceiver : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField, FormerlySerializedAs("onEvent")]
        private AnimationEventReceiver _OnEvent;

        /// <summary>[<see cref="SerializeField"/>] A callback for Animation Events with the Function Name "Event".</summary>
        public ref AnimationEventReceiver OnEvent => ref _OnEvent;

        /************************************************************************************************************************/

        /// <summary>Called by Animation Events with the Function Name "Event".</summary>
        private void Event(AnimationEvent animationEvent)
        {
            _OnEvent.SetFunctionName(nameof(Event));
            _OnEvent.HandleEvent(animationEvent);
        }

        /************************************************************************************************************************/
    }
}
