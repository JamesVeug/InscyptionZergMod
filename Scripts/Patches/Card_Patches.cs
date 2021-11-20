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
}