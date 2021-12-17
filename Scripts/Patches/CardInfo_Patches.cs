using DiskCardGame;
using HarmonyLib;
using ZergMod.Scripts.SpecialAbilities;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof(CardInfo), "Attack", MethodType.Getter)]
    public class CardInfo_Attack
    {
        public static void Postfix(ref CardInfo __instance, ref int __result)
        {
            if (__instance.specialAbilities.Contains(DehakaSpecialAbility.specialAbility))
            {
                __result += (CustomSaveManager.SaveFile.DehakaKills + 1) / 2;
            }
        }
    }
    
    [HarmonyPatch(typeof(CardInfo), "Health", MethodType.Getter)]
    public class CardInfo_Health
    {
        public static void Postfix(ref CardInfo __instance, ref int __result)
        {
            if (__instance.specialAbilities.Contains(DehakaSpecialAbility.specialAbility))
            {
                __result += (CustomSaveManager.SaveFile.DehakaKills / 2);
            }
        }
    }
}