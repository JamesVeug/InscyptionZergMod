using System;
using APIPlugin;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Patches
{
    // Disabled until i decide what to do with the rest of it
    /*[HarmonyPatch(typeof(CardInfo), "Attack", MethodType.Getter)]
    public class CardInfo_Attack
    {
        public static void Postfix(ref CardInfo __instance, ref int __result)
        {
            Plugin.Log.LogInfo("[CardInfo_Attack] ");
            if (__instance.specialAbilities.Contains(ZerglingSpecialAbility.specialAbility))
            {
                if (__instance.Health >= ZerglingSpecialAbility.MaxZerglingsToSwarm)
                {
                    __result += ZerglingSpecialAbility.SwarmDamageBonus;
                }
            }
        }
    }*/
}