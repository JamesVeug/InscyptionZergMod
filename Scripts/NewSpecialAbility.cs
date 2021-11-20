using System;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class NewSpecialAbility
    {
        public static List<NewSpecialAbility> SPECIAL_ABILITIES = new List<NewSpecialAbility>();
        
        public SpecialTriggeredAbility SpecialAbility;
        public Type BehaviourType;
        public string BehaviourTypeString;

        public NewSpecialAbility(System.Type abilityBehaviour)
        {
            SpecialAbility = (SpecialTriggeredAbility) (100 + SPECIAL_ABILITIES.Count);
            BehaviourType = abilityBehaviour;
            BehaviourTypeString = abilityBehaviour.ToString();
            
            SPECIAL_ABILITIES.Add(this);
            Debug.Log("Loaded custom special ability " + BehaviourTypeString + "!");
        }

        public static bool GetSpecialAbility(SpecialTriggeredAbility ability, out NewSpecialAbility newSpecialAbility)
        {
            for (int i = 0; i < SPECIAL_ABILITIES.Count; i++)
            {
                if (SPECIAL_ABILITIES[i].SpecialAbility == ability)
                {
                    newSpecialAbility = SPECIAL_ABILITIES[i];
                    return true;
                }
            }

            newSpecialAbility = null;
            return false;
        }
    }
}
