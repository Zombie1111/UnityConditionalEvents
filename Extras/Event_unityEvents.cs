//UnityConditionalEvents by David Westberg https://github.com/Zombie1111/UnityConditionalEvents
using UnityEngine;
using UnityEngine.Events;

namespace zombCondEvents
{
    public class Event_unityEvents : Event
    {
        [SerializeField] private bool alwaysOnlyInvokePositive = false;
        [SerializeField] private UnityEvent positiveEvents = new();
        [SerializeField] private UnityEvent negativeEvents = new();

        public override void TriggerEvent(ConditionalEvent condEvent, GameObject trigger, bool isPositive, bool requirementMet)
        {
            if (isPositive == true || alwaysOnlyInvokePositive == true) positiveEvents.Invoke();
            else negativeEvents.Invoke();
        }

        public override void Init(ConditionalEvent condEvent)
        {
            
        }

        public override void OnWillDestroy(ConditionalEvent condEvent)
        {
            
        }

        public override void OnReset(ConditionalEvent condEvent)
        {

        }
    }
}
