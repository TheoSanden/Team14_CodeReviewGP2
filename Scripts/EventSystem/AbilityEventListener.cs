using UnityEngine.Events;
using UnityEngine;

namespace Events
{
    public class AbilityEventListener : MonoBehaviour
    {
        [SerializeField] private AbilityEvent Event;
        [SerializeField] private UnityEvent<Ability> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(Ability obj)
        {
            Response?.Invoke(obj);
        }
    }
}
