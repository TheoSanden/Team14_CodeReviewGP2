using UnityEngine.Events;
using UnityEngine;

namespace Events
{
    public class InteractionEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public InteractionEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<InteractionArgs> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(InteractionArgs obj)
        {
            Response.Invoke(obj);
        }
    }
}
