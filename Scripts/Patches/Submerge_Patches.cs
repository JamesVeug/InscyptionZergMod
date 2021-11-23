using System;
using System.Collections.Generic;
using BepInEx.Logging;
using DiskCardGame;
using EasyFeedback.APIs;
using HarmonyLib;
using UnityEngine;
using ZergMod;
using Plugin = APIPlugin.Plugin;

namespace ZergMod.Patches
{
    [HarmonyPatch(typeof(Submerge), "RespondsToUpkeep", new System.Type[] { typeof(bool) })]
    public class Submerge_RespondsToUpkeep
    {
        public static void Postfix(bool playerUpkeep, Submerge __instance, ref bool __result)
        {
            if (!__result)
            {
                return;
            }

            PlayableCard card = __instance.Card;
            if (Utils.OpponentHasADetector(card))
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(Submerge), "RespondsToTurnEnd", new System.Type[] { typeof(bool) })]
    public class Submerge_RespondsToTurnEnd
    {
        public static void Postfix(bool playerTurnEnd, Submerge __instance, ref bool __result)
        {
            if (!__result)
            {
                return;
            }

            PlayableCard card = __instance.Card;
            if (Utils.OpponentHasADetector(card))
            {
                __result = false;
            }
        }
    }
}