using System;
using System.Collections.Generic;
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
            //Plugin.Log.LogInfo("[Card.AttachAbilities] ");
            foreach (SpecialTriggeredAbility specialTriggeredAbility in info.SpecialAbilities)
            {
                //Plugin.Log.LogInfo("[Card.AttachAbilities] " + specialTriggeredAbility);
                bool customSpecialAbility = false;
                foreach (NewSpecialAbility ability in NewSpecialAbility.specialAbilities)
                {
                    if (specialTriggeredAbility == ability.specialTriggeredAbility)
                    {
                        //Plugin.Log.LogInfo("[Card.AttachAbilities] Found custom special ability: " + specialTriggeredAbility);
                        Utils.AttachMonoBehaviour<SpecialCardBehaviour>(ability.abilityBehaviour, __instance.gameObject);

                        customSpecialAbility = true;
                        break;
                    }
                }

                if (!customSpecialAbility)
                {
                    //Plugin.Log.LogInfo("[Card.AttachAbilities] Fallback for special ability: " + specialTriggeredAbility);
                    CardTriggerHandler.AddReceiverToGameObject<SpecialCardBehaviour>(specialTriggeredAbility.ToString(), __instance.gameObject);
                }
            }

            return false;
        }
    }
    
    [HarmonyPatch(typeof (Card), "SetInfo", new System.Type[] {typeof (CardInfo)})]
    public class Card_SetInfo
    {
        public static void Postfix(CardInfo info, Card __instance)
        {
            // Fixes Zerglings portrait not changing from 2 to 4 when buffing the health at the campfire
            foreach (IPortraitChanges portraitChanges in __instance.gameObject.GetComponents<IPortraitChanges>())
            {
                if (portraitChanges.ShouldRefreshPortrait())
                {
                    portraitChanges.RefreshPortrait();
                }
            }
        }
    }
    
    [HarmonyPatch(typeof (Card), "ApplyAppearanceBehaviours", new System.Type[] {typeof (List<CardAppearanceBehaviour.Appearance>)})]
    public class Card_ApplyAppearanceBehaviours
    {
        public static bool Prefix(List<CardAppearanceBehaviour.Appearance> appearances, Card __instance)
        {
            foreach (CardAppearanceBehaviour.Appearance appearance in appearances)
            {
                if (NewBackgroundBehaviour.Behaviours.TryGetValue(appearance, out NewBackgroundBehaviour behaviour))
                {
                    Type type = behaviour.Behaviour;
                    if (!__instance.gameObject.GetComponent(type))
                    {
                        (__instance.gameObject.AddComponent(type) as CardAppearanceBehaviour).ApplyAppearance();
                    }
                }
                else
                {
                    Type type = CustomType.GetType("DiskCardGame", appearance.ToString());
                    if (!__instance.gameObject.GetComponent(type))
                    {
                        (__instance.gameObject.AddComponent(type) as CardAppearanceBehaviour).ApplyAppearance();
                    }
                }
            }

            return false;
        }
    }
}