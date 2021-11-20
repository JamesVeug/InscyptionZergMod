using System;
using APIPlugin;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Patches
{
    [HarmonyPatch(typeof (CardTriggerHandler), "AddAbility", new System.Type[] {typeof (SpecialTriggeredAbility)})]
    public class CardTriggerHandler_AddAbility
    {
        public static bool Prefix(ref SpecialTriggeredAbility ability, CardTriggerHandler __instance)
        {
            if (ability < SpecialTriggeredAbility.NUM_ABILITIES)
                return true;
            
            if (NewSpecialAbility.GetSpecialAbility(ability, out NewSpecialAbility newSpecialAbility))
            {
                SpecialCardBehaviour behaviour = Utils.AttachMonoBehaviour<SpecialCardBehaviour>(newSpecialAbility.BehaviourType, __instance.gameObject);
                __instance.specialAbilities.Add(new Tuple<SpecialTriggeredAbility, SpecialCardBehaviour>(ability, behaviour));
            }
            else
            {
                Plugin.Log.Log(LogLevel.Warning, "Special ability not found: " + ability + " for " + __instance.gameObject.name);
            }

            return false;
        }
    }
}