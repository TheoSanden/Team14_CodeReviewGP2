using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "new IntEvent", menuName = "ScriptableObjects/Events/IntEvent", order = 0)]
    public class IntEvent : GameEvent
    {
        private readonly List<IntEventListener> eventListeners =
        new List<IntEventListener>();

        public void Raise(int obj)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(obj);
        }

        public void RegisterListener(IntEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(IntEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}
