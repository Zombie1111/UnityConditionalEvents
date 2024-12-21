//UnityConditionalEvents by David Westberg https://github.com/Zombie1111/UnityConditionalEvents
using UnityEngine;
using zombCondEvents;

public class Cond_maxTriggerCount : Condition
{
    [Tooltip("Max trigger count, combined negative and positive")]
    [SerializeField] private int maxTrigCount_both = 10;
    [SerializeField] private int maxTrigCount_positive = -1;
    [SerializeField] private int maxTrigCount_negative = -1;
    int trigCount_both = 0;
    int trigCount_positive = 0;
    int trigCount_negative = 0;

    public override bool CheckCondition(ConditionalEvent condEvent, GameObject trigger, bool isPositive)
    {
        trigCount_both++;
        if (isPositive == true) trigCount_positive++;
        else trigCount_negative++;

        return CheckCount(trigCount_both, maxTrigCount_both)
            && (isPositive == true ? CheckCount(trigCount_positive, maxTrigCount_positive ): CheckCount(trigCount_negative, maxTrigCount_negative));

        static bool CheckCount(int count, int max)
        {
            if (max < 0) return true;
            return count <= max;
        }
    }

    public override void Init(ConditionalEvent condEvent)
    {

    }

    public override void OnWillDestroy(ConditionalEvent condEvent)
    {
        
    }

    public override void OnReset(ConditionalEvent condEvent)
    {
        trigCount_both = 0;
        trigCount_positive = 0;
        trigCount_negative = 0;
    }
}
