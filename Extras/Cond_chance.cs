//UnityConditionalEvents by David Westberg https://github.com/Zombie1111/UnityConditionalEvents
using UnityEngine;

namespace zombCondEvents
{
    public class Cond_chance : Condition
    {
        [Tooltip("Chance if invoked as positive")]
        [SerializeField] private float chancePositive = 0.5f;
        [Tooltip("Chance if invoked as negative")]
        [SerializeField] private float chanceNegative = 0.5f;

        public override bool CheckCondition(ConditionalEvent condEvent, GameObject trigger, bool isPositive)
        {
            return UnityEngine.Random.Range(0.0f, 1.0f) < (isPositive == true ? chancePositive : chanceNegative);
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
