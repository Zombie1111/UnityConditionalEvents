//UnityConditionalEvents by David Westberg https://github.com/Zombie1111/UnityConditionalEvents
using UnityEngine;

namespace zombCondEvents
{
    public abstract class Condition : MonoBehaviour
    {
        /// <summary>
        /// Called by a ConditionalEvent when its updated
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that want to check this Condition</param>
        /// <param name="trigger">The Gameobject that caused the ConditionalEvent to update</param>
        /// <param name="isPositive">True if the ConditionalEvent was updated as positive</param>
        /// <returns>True if the Condition is met</returns>
        public virtual bool CheckCondition(ConditionalEvent condEvent, GameObject trigger, bool isPositive)
        {
            return true;
        }

        /// <summary>
        /// Called after ConditionalEvent initinization but before the first CheckCondition
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that was initilized</param>
        public virtual void Init(ConditionalEvent condEvent)
        {

        }

        /// <summary>
        /// Called before the ConditionalEvent is destroyed but after the last CheckCondition
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that will be destroyed</param>
        public virtual void OnWillDestroy(ConditionalEvent condEvent)
        {
            
        }

        /// <summary>
        /// Called to reset the Condition (Not called on Init or OnWillDestroy)
        /// </summary>
        /// <param name="condEvent">The ConditionalEvent that want the Condition to reset</param>
        public virtual void OnReset(ConditionalEvent condEvent)
        {

        }
    }
}
