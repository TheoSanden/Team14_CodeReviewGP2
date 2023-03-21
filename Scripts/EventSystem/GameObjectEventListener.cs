using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Events
{
    public class GameObjectEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameObjectEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<GameObject> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(GameObject obj)
        {
            Response.Invoke(obj);
        }
    }
}

