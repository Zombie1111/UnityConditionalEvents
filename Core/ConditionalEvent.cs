//UnityConditionalEvents by David Westberg https://github.com/Zombie1111/UnityConditionalEvents
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace zombCondEvents
{
    [System.Serializable]
    public class ConditionalEvent
    {
        #region Editor
#if UNITY_EDITOR
        [Header("Editor Only")]
        [SerializeField] private bool autoAddConditionsFromChildren = true;
        [SerializeField] private bool autoAddEventsFromChildren = true;
        private HashSet<int> condsAddedFromChildren = new();
        private HashSet<int> eventsAddedFromChildren = new();

        /// <summary>
        /// Should be called in editor for autoAddConditionsFromChildren and autoAddEventsFromChildren to work (EDITOR ONLY)
        /// </summary>
        /// <param name="thisObj">The gameobject the ConditionalEvent is attatched to</param>
        public void EditorTick(GameObject thisObj)
        {
            if (Application.isPlaying == true) return;//For consistency with build

            bool changedAnything = false;
            //The auto add stuff feels a bit overcomplicated but since its editor only its not worth rewriting it
            if (autoAddConditionsFromChildren == true)
            {
                bool mayRemovedAuto = condsAddedFromChildren.Contains(conditions.Count);
                if (condsAddedFromChildren.Count == 0 || mayRemovedAuto == true) for (int i = 0; i < conditions.Count; i++)
                    {
                         if (conditions[i] == null) continue;
                         condsAddedFromChildren.Add(i);
                    }

                if (RemoveAllNullConditions() == true) changedAnything = true;
                if (AddAllConditionsInChildren(thisObj) == true)
                {
                    changedAnything = true;
                    if (mayRemovedAuto == true) Debug.Log("Tried to remove an auto added Condition from " + thisObj.transform.name);
                }
                else if (mayRemovedAuto == true) condsAddedFromChildren.Clear();
            }
            else condsAddedFromChildren.Clear();

            if (autoAddEventsFromChildren == true)
            {
                bool mayRemovedAuto = eventsAddedFromChildren.Contains(events.Count);
                if (eventsAddedFromChildren.Count == 0 || mayRemovedAuto == true) for (int i = 0; i < events.Count; i++)
                    {
                        if (events[i] == null) continue;
                        eventsAddedFromChildren.Add(i);
                    }

                if (RemoveAllNullEvents() == true) changedAnything = true;
                if (AddAllEventsFromChildren(thisObj) == true)
                {
                    changedAnything = true;
                    if (mayRemovedAuto == true) Debug.Log("Tried to remove an auto added Event from " + thisObj.transform.name);
                }
                else if (mayRemovedAuto == true) eventsAddedFromChildren.Clear();
            }
            else eventsAddedFromChildren.Clear();

            if (changedAnything == false) return;
            EditorUtility.SetDirty(thisObj);
        }
#endif
        #endregion Editor

        #region Api

        public enum ResetType
        {
            everything,
            conditionalEventOnly,
            conditionsOnly,
            eventsOnly
        }

        /// <summary>
        /// Call to reset stuff
        /// </summary>
        /// <param name="resetType">What stuff to reset. If conditionalEventOnly, only resets the memory for AllOnce/AnyOnce RequiredConditions</param>
        public void Reset(ResetType resetType)
        {
            switch (resetType)
            {
                case ResetType.everything:
                    ResetConditionalEvent();
                    ResetConditions();
                    ResetEvents();
                    break;
                case ResetType.conditionalEventOnly:
                    ResetConditionalEvent();
                    break;
                case ResetType.conditionsOnly:
                    ResetConditions();
                    break;
                default:
                    ResetEvents();
                    break;
            }

            void ResetConditionalEvent()
            {
                conditionStates = new bool[conditions.Count];
            }

            void ResetConditions()
            {
                foreach (Condition cond in conditions)
                {
                    cond.OnReset(this);
                }
            }

            void ResetEvents()
            {
                foreach (Event eevent in events)
                {
                    eevent.OnReset(this);
                }
            }
        }

        /// <summary>
        /// Returns true if any null Condition in the conditions list existed and was removed
        /// </summary>
        public bool RemoveAllNullConditions()
        {
            bool removedAnything = false;

            for (int i = conditions.Count - 1; i >= 0; i--)
            {
                if (conditions[i] != null) continue;
#if UNITY_EDITOR
                if (Application.isPlaying == false && condsAddedFromChildren.Contains(i) == false) continue;
#endif

                conditions.RemoveAt(i);
                removedAnything = true;

#if UNITY_EDITOR
                if (Application.isPlaying == true) continue;

                HashSet<int> newHash = new(condsAddedFromChildren.Count);
                foreach (int ii in condsAddedFromChildren)//Hashset will be recreated once for every condition removed, not worth optimizing since editor only
                {
                    if (ii == i) continue;
                    if (ii > i) newHash.Add(ii - 1);
                    else newHash.Add(ii);
                }

                condsAddedFromChildren = newHash;
#endif
            }

            return removedAnything;
        }

        /// <summary>
        /// Returns true if any null Event in the events list existed and was removed
        /// </summary>
        public bool RemoveAllNullEvents()
        {
            bool removedAnything = false;

            for (int i = events.Count - 1; i >= 0; i--)
            {
                if (events[i] != null) continue;
#if UNITY_EDITOR
                if (Application.isPlaying == false && eventsAddedFromChildren.Contains(i) == false) continue;
#endif

                events.RemoveAt(i);
                removedAnything = true;

#if UNITY_EDITOR
                if (Application.isPlaying == true) continue;

                HashSet<int> newHash = new(eventsAddedFromChildren.Count);
                foreach (int ii in eventsAddedFromChildren)//Hashset will be recreated once for every event removed, not worth optimizing since editor only
                {
                    if (ii == i) continue;
                    if (ii > i) newHash.Add(ii - 1);
                    else newHash.Add(ii);
                }

                eventsAddedFromChildren = newHash;
#endif
            }

            return removedAnything;
        }

        /// <summary>
        /// Finds all Condition components in parentObj (Or any of its children) and tries to add them to the conditions list (Returns true if any Condition was added)
        /// </summary>
        public bool AddAllConditionsInChildren(GameObject parentObj)
        {
            bool addedAnything = false;

            foreach (Condition cond in parentObj.GetComponentsInChildren<Condition>(true))
            {
                if (TryAddCondition(cond) == false) continue;
                addedAnything = true;

#if UNITY_EDITOR
                if (Application.isPlaying == true) continue;
                condsAddedFromChildren.Add(conditions.Count - 1);
#endif
            }

            return addedAnything;
        }

        /// <summary>
        /// Finds all Event components in parentObj (Or any of its children) and tries to add them to the events list (Returns true if any Event was added)
        /// </summary>
        public bool AddAllEventsFromChildren(GameObject parentObj)
        {
            bool addAnything = false;

            foreach (Event eevent in parentObj.GetComponentsInChildren<Event>(true))
            {
                if (TryAddEvent(eevent) == false) continue;
                addAnything = true;

#if UNITY_EDITOR
                if (Application.isPlaying == true) continue;
                eventsAddedFromChildren.Add(events.Count - 1);
#endif
            }

            return addAnything;
        }

        /// <summary>
        /// Adds the given Condition to the conditions list if its not already added (Returns true if the Condition was added)
        /// </summary>
        public bool TryAddCondition(Condition cond)
        {
            if (conditions.Contains(cond) == true) return false;

            conditions.Add(cond);
            return true;
        }

        /// <summary>
        /// Adds the given Event to the events list if its not already added (Returns true if the Event was added)
        /// </summary>
        public bool TryAddEvent(Event eevent)
        {
            if (events.Contains(eevent) == true) return false;

            events.Add(eevent);
            return true;
        }

        /// <summary>
        /// Returns true if the given Condition could be removed from the conditions list
        /// </summary>
        public bool TryRemoveCondition(Condition cond)
        {
            return conditions.Remove(cond);
        }

        /// <summary>
        /// Returns true if the given Event could be removed from the events list
        /// </summary>
        public bool TryRemoveEvent(Event eevent)
        {
            return events.Remove(eevent);
        }

        /// <summary>
        /// Returns the conditions list (Returned list can be modified to add/remove conditions)
        /// </summary>
        public List<Condition> GetConditions()
        {
            return conditions;
        }

        /// <summary>
        /// Returns the events list (Returned list can be modified to add/remove events)
        /// </summary>
        public List<Event> GetEvents()
        {
            return events;
        }

        #endregion Api

        #region Main

        /// <summary>
        /// The gameobject the ConditionalEvent is attatched to
        /// </summary>
        [System.NonSerialized] public GameObject gameobject = null;
        /// <summary>
        /// The transform the ConditionalEvent is attatched to
        /// </summary>
        [System.NonSerialized] public Transform transform = null;

        private bool isInitilized = false;

        /// <summary>
        /// At runtime, this should be called before any other function
        /// </summary>
        /// <param name="thisObj">The gameobject the ConditionalEvent is attatched to</param>
        public void Init(GameObject thisObj)
        {
            if (isInitilized == true) return;
            isInitilized = true;

            //Events and conditions from children are added in EditorTick. Since the lists are serialized they will be saved so no need to get them in Init()
            gameobject = thisObj;//Not used internally but useful for Conditions/Events so they can access the gameobject/transform though condEvent.gameobject
            transform = thisObj.transform;

            foreach (Condition cond in conditions)
            {
                if (cond == null) continue;
                cond.Init(this);//I think its more likely that a Event will depend on a Condition than the opposite?
            }

            foreach (Event eevent in events)
            {
                if (eevent == null) continue;
                eevent.Init(this);
            }
        }

        /// <summary>
        /// Should be called before the gameobject the ConditionalEvent is attatched to is destroyed
        /// </summary>
        public void WillDestroy()
        {
            if (isInitilized == false) return;
            isInitilized = false;

            foreach (Event eevent in events)
            {
                if (eevent == null) continue;
                eevent.OnWillDestroy(this);
            }

            foreach (Condition cond in conditions)
            {
                if (cond == null) continue;
                cond.OnWillDestroy(this);
            }

            gameobject = null;//Just for clarity that it is "destroyed"
            transform = null;
        }

        /// <summary>
        /// Call to check the conditions and trigger the events if the requirement is met.
        /// </summary>
        /// <param name="trigger">The gameobject that caused you to call this function or the gameobject the script calling this function is attatched to</param>
        /// <param name="isPositive">Should the trigger be seen as positive? Example: if called from a collision event, enter could be positive and exit negative</param>
        /// <returns>True if any event was triggered</returns>
        public bool UpdateConditionalEvents(GameObject trigger, bool isPositive)
        {
            bool requirementMet = CheckConditions(trigger, isPositive);

            if (requirementNeeded != RequirementNeeded.always)
            {
                //ChangePositivityIfShould() will only be called once because the only the first condition will be checked if it fails
                if (requirementNeeded == RequirementNeeded.never) goto SkipRequirementCheck;

                if (requirementNeeded == RequirementNeeded.ifPositive && ChangePositivityIfShould(isPositive) == false) goto SkipRequirementCheck;
                else if (ChangePositivityIfShould(isPositive) == true) goto SkipRequirementCheck;
            }

            if (requirementMet == false) return false;

            SkipRequirementCheck:;

            return TriggerEvents(trigger, isPositive, requirementMet);
        }

        private bool ChangePositivityIfShould(bool positivity)
        {
            switch (positivityMode)
            {
                case PositivityMode.defualt:
                    return positivity;
                case PositivityMode.reversed:
                    return !positivity;
                case PositivityMode.alwaysPositive:
                    return true;
                default:
                    break;
            }

            return false;
        }

        #endregion Main

        #region Conditions

        private enum RequirementNeeded
        {
            always = 0,
            never = 1,
            ifPositive = 2,
            ifNegative = 3
        }

        private enum RequiredConditions
        {
            all = 0,
            allOnce = 1,
            any = 2,
            anyOnce = 3
        }

        private enum RequirementReversion
        {
            never = 0,
            always = 1,
            ifNegative = 2,
            ifPositive = 3,
        }

        [Space()]
        [Header("Conditions And Requirements")]
        [Tooltip("If the requirement must be met for any Event to trigger. If ifPositive, the requirement must only be met if positive input" +
            "(Checked after requirementReversion)")]
        [SerializeField] private RequirementNeeded requirementNeeded = RequirementNeeded.always;
        [Tooltip("What condition(s) needs to be true for the requirement to be met")]
        [SerializeField] private RequiredConditions requiredConditions = RequiredConditions.all;
        [Tooltip("If false should mean true. If ifNegative, requirement is met will be reversed if negative input (bool = !bool, applied after requiredConditions)")]
        [SerializeField] private RequirementReversion requirementReversion = RequirementReversion.never;
        [Tooltip("The conditions that will be checked, checked after positivityMode but before requirementNeeded" +
            " (The minimum possible amount of Conditions will be checked to determent if requirements is met or not)")]
        [SerializeField] private List<Condition> conditions = new();
        private bool[] conditionStates = new bool[0];

        /// <summary>
        /// Returns true if the requirement is met. You normally wanna use UpdateConditionalEvents()
        /// </summary>
        public bool CheckConditions(GameObject trigger, bool isPositive)
        {
            isPositive = ChangePositivityIfShould(isPositive);

            //Check the conditions (Remember)
            if (requiredConditions == RequiredConditions.allOnce || requiredConditions == RequiredConditions.anyOnce)
            {
                if (conditionStates.Length != conditions.Count) conditionStates = new bool[conditions.Count];
                int trueCount = 0;

                for (int i = 0; i < conditions.Count; i++)
                {
                    if (conditionStates[i] == true)
                    {
                        trueCount++;
                        continue;
                    }

                    if (conditions[i].CheckCondition(this, trigger, isPositive) == false) continue;

                    conditionStates[i] = true;
                    trueCount++;
                }

                if (requiredConditions == RequiredConditions.anyOnce) return ReverseIfShould(trueCount > 0);
                return ReverseIfShould(trueCount == conditionStates.Length);
            }

            //Check the conditions (Simple)
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i] == null) continue;//Currently null will not cause Requirement.All to fail, I think this is good?
                if (conditions[i].CheckCondition(this, trigger, isPositive) == false)
                {
                    if (requiredConditions == RequiredConditions.all) return ReverseIfShould(false);
                    continue;
                }

                if (requiredConditions == RequiredConditions.all) continue;
                return ReverseIfShould(true);
            }

            return ReverseIfShould(requiredConditions == RequiredConditions.all);

            bool ReverseIfShould(bool rMet)
            {
                switch (requirementReversion)
                {
                    case RequirementReversion.never:
                        return rMet;
                    case RequirementReversion.always:
                        return !rMet;
                    case RequirementReversion.ifNegative:
                        if (isPositive == false) return !rMet;
                        return rMet;
                    default:
                    break;
                }

                if (isPositive == true) return !rMet;
                return rMet;
            }
        }

        #endregion Conditions

        #region Events

        private enum PositivityMode
        {
            defualt = 0,
            reversed = 1,
            alwaysPositive = 2,
            alwaysNegative = 3
        }

        private enum PositivityFromRequirement
        {
            none = 0,
            overwritePositivity = 1,
            andCheck = 2,
            elseCheck = 3,
        }

        private enum TriggerBehaviour
        {
            always,
            ifPositivityChanged,
            ifRequirementMetChanged,
            ifPositivityOrRequirementChanged,
        }
        [Space()]
        [Header("Events And Other")]
        [Tooltip("Should input positivity be kept, reversed or overwriten? Applied very first before any other checks")]
        [SerializeField] private PositivityMode positivityMode = PositivityMode.defualt;
        [Tooltip("If none, positivity will be unchanged. andCheck == positivity = positivity && requirementMet, overwritePositivity == positivity = requirementMet"
            + " Applied after positivityMode and requirement checks but before triggerBehaviour and events")]
        [SerializeField] private PositivityFromRequirement positivityFromRequirment = PositivityFromRequirement.none;
        [Tooltip("If Events should only be triggered if something has changed or always trigger, checked after positivityFromRequirment")]
        [SerializeField] private TriggerBehaviour triggerBehaviour = TriggerBehaviour.always;
        [Tooltip("The events to trigger, triggered very last after all other checks")]
        [SerializeField] private List<Event> events = new();
        private bool hasTriggeredOnce = false;
        private bool lastPositivity = false;
        private bool lastRequirementMet = false;

        /// <summary>
        /// Tries to trigger all events. You normally wanna use UpdateConditionalEvents()
        /// </summary>
        /// <returns>True if any event was triggered</returns>
        public bool TriggerEvents(GameObject trigger, bool isPositive, bool requirementMet)
        {
            //Change positivity
            isPositive = ChangePositivityIfShould(isPositive);

            switch (positivityFromRequirment)
            {
                case PositivityFromRequirement.none:
                    break;
                case PositivityFromRequirement.overwritePositivity:
                    isPositive = requirementMet;
                    break;
                case PositivityFromRequirement.andCheck:
                    isPositive = isPositive && requirementMet;
                    break;
                default:
                    isPositive = isPositive || requirementMet;
                    break;
            }

            //Trigger events
            if (hasTriggeredOnce == false || triggerBehaviour == TriggerBehaviour.always) goto SkipCheckTriggerBehaviour;
            switch (triggerBehaviour)
            {
                case TriggerBehaviour.ifPositivityChanged:
                    if (lastPositivity == isPositive) return false;
                    break;
                case TriggerBehaviour.ifRequirementMetChanged:
                    if (lastRequirementMet == requirementMet) return false;
                    break;
                default:
                    if (lastPositivity == isPositive && lastRequirementMet == requirementMet) return false;
                    break;
            }

            SkipCheckTriggerBehaviour:;

            hasTriggeredOnce = true;
            lastPositivity = isPositive;
            lastRequirementMet = requirementMet;

            foreach (Event eevent in events)
            {
                if (eevent == null) continue;//If no events are assigned it would still return true, not a problem?
                eevent.TriggerEvent(this, trigger, isPositive, requirementMet);
            }

            return true;
        }

        #endregion Events
    }
}