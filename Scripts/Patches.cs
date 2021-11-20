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
            Plugin.Log.LogInfo("Prefix Card.AttachAbilities: " + NewSpecialAbility.SPECIAL_ABILITIES.Count);
            foreach (SpecialTriggeredAbility specialTriggeredAbility in info.SpecialAbilities)
            {
                bool customSpecialAbility = false;
                foreach (NewSpecialAbility ability in NewSpecialAbility.SPECIAL_ABILITIES)
                {
                    if (specialTriggeredAbility == ability.SpecialAbility)
                    {
                        Plugin.Log.LogInfo("\tPrefix Attach Custom Special Ability: " + ability.BehaviourTypeString);
                        Utils.AttachMonoBehaviour<SpecialCardBehaviour>(ability.BehaviourType, __instance.gameObject);

                        customSpecialAbility = true;
                        break;
                    }
                }

                if (!customSpecialAbility)
                {
                    Plugin.Log.LogInfo("\tPrefix Attach Vanilla Special Ability: " + specialTriggeredAbility.ToString());
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
            Plugin.Log.LogInfo("Prefix CardTriggerHandler.AddAbility: " + ability.ToString());
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
            Plugin.Log.LogInfo("[AttachMonoBehaviour] Attaching " + type + " to " + gameObject.name);
            SpecialCardBehaviour t = gameObject.GetComponent(type) as SpecialCardBehaviour;
            if (t == null)
            {
                Plugin.Log.LogInfo("[AttachMonoBehaviour] Added " + type + " to " + gameObject.name);
                return (T)(object)gameObject.AddComponent(type);
            }
            else
            {
                Plugin.Log.LogInfo("[AttachMonoBehaviour] " + gameObject.name + " already has " + type + " attached: " + t);
                return (T)(object)t;
            }
        }
    }
}