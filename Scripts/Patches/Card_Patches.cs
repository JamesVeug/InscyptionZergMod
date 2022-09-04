using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Helpers;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof(Card), "AttachAbilities",new System.Type[] {typeof(CardInfo)})]
    public class Card_AttachAbilities
    {
        public static bool Prefix(Card __instance, CardInfo info)
        {
            Plugin.Log.LogInfo("[CardTriggerHandler_AddReceiverToGameObject] " + __instance.name + " " + info.displayedName);
            foreach (SpecialTriggeredAbility specialTriggeredAbility in info.SpecialAbilities)
            {
                Plugin.Log.LogInfo("\t " + specialTriggeredAbility);
            }

            return true;
        }
    }
}