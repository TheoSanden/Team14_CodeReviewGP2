using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "new InteractionEvent", menuName = "ScriptableObjects/Events/InteractionEvent", order = 0)]
    public class InteractionEvent : GameEvent
    {
        private readonly List<InteractionEventListener> eventListeners =
          new List<InteractionEventListener>();

        public void Raise(InteractionArgs obj)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(obj);
        }

        public void RegisterListener(InteractionEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(InteractionEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}
