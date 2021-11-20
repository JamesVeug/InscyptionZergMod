using System;
using APIPlugin;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Patches
{
    [HarmonyPatch(typeof (Card), "AttachAbilities", new System.Type[] {typeof (CardInfo)})]
    public class Card_AttachAbilities
    {
        public static bool Prefix(CardInfo info, Card __instance)
        {
            foreach (SpecialTriggeredAbility specialTriggeredAbility in info.SpecialAbilities)
            {
                bool customSpecialAbility = false;
                foreach (NewSpecialAbility ability in NewSpecialAbility.SPECIAL_ABILITIES)
                {
                    if (specialTriggeredAbility == ability.SpecialAbility)
                    {
                        Utils.AttachMonoBehaviour<SpecialCardBehaviour>(ability.BehaviourType, __instance.gameObject);

                        customSpecialAbility = true;
                        break;
                    }
                }

                if (!customSpecialAbility)
                {
                    CardTriggerHandler.AddReceiverToGameObject<SpecialCardBehaviour>(specialTriggeredAbility.ToString(), __instance.gameObject);
                }
            }

            return false;
        }
    }
    
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

    public static class Utils
    {
        public static T AttachMonoBehaviour<T>(Type type, GameObject gameObject)
        {
            SpecialCardBehaviour t = gameObject.GetComponent(type) as SpecialCardBehaviour;
            if (t == null)
            {
                return (T)(object)gameObject.AddComponent(type);
            }
            else
            {
                return (T)(object)t;
            }
        }
    }
}