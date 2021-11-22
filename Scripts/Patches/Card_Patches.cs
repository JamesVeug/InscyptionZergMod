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
}