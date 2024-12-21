//UnityConditionalEvents by David Westberg https://github.com/Zombie1111/UnityConditionalEvents
using UnityEngine;

namespace zombCondEvents
{
    public abstract class Event : MonoBehaviour
    {
        /// <summary>
        /// Called by a ConditionalEvent when its conditions become true or false
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that trigger this Event</param>
        /// <param name="trigger">The Gameobject that caused the ConditionalEvent to update its conditions</param>
        /// <param name="isPositive">True if the ConditionalEvent was updated as positive</param>
        /// <param name="requirementMet">True if the ConditionalEvent conditions where met</param>
        public virtual void TriggerEvent(ConditionalEvent condEvent, GameObject trigger, bool isPositive, bool requirementMet)
        {

        }

        /// <summary>
        /// Called after ConditionalEvent initinization but before the first TriggerEvent
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that was initilized</param>
        public virtual void Init(ConditionalEvent condEvent)
        {

        }

        /// <summary>
        /// Called before the ConditionalEvent is destroyed but after the last TriggerEvent
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that will be destroyed</param>
        public virtual void OnWillDestroy(ConditionalEvent condEvent)
        {

        }

        /// <summary>
        /// Called to reset the Event (Not called on Init or OnWillDestroy)
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that want the Event to reset</param>
        public virtual void OnReset(ConditionalEvent condEvent)
        {

        }
    }
}
