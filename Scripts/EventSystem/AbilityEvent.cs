using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "new AbilityEvent", menuName = "ScriptableObjects/Events/AbilityEvent", order = 0)]
    public class AbilityEvent : GameEvent
    {
        private readonly List<AbilityEventListener> eventListeners =
            new List<AbilityEventListener>();

        public void Raise(Ability obj)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(obj);
        }

        public void RegisterListener(AbilityEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(AbilityEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}
