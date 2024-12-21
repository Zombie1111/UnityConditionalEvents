//UnityConditionalEvents by David Westberg https://github.com/Zombie1111/UnityConditionalEvents
using UnityEngine;

namespace zombCondEvents
{
    public class Event_debugLog : Event
    {
        [SerializeField] private bool logInitDestroyReset = false;
        [SerializeField] private bool logTrigger = true;
        [SerializeField] private string msg = string.Empty;

        public override void TriggerEvent(ConditionalEvent condEvent, GameObject trigger, bool isPositive, bool requirementMet)
        {
            if (logTrigger == false) return;
            Debug.Log("Triggered: " + condEvent.transform.name + " Trigger: " + (trigger != null ? trigger.transform.name : "NULL")
                + " isP: " + isPositive + " req: " + requirementMet + " msg: " + msg);
        }

        public override void Init(ConditionalEvent condEvent)
        {
            if (logInitDestroyReset == false) return;
            Debug.Log("Initlized: " + condEvent.transform.name + " msg: " + msg);
        }

        public override void OnWillDestroy(ConditionalEvent condEvent)
        {
            if (logInitDestroyReset == false) return;
            Debug.Log("Destroyed: " + condEvent.transform.name + " msg: " + msg);
        }

        public override void OnReset(ConditionalEvent condEvent)
        {
            if (logInitDestroyReset == false) return;
            Debug.Log("Reseted: " + condEvent.transform.name + " msg: " + msg);
        }
    }
}
