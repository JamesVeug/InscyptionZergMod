using DiskCardGame;
using HarmonyLib;
using ZergMod.Scripts.SpecialAbilities;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof(RunState), "Initialize")]
    public class RunState_Initialize
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            DehakaSpecialAbility.SavedDehakaKills = 0;
        }
    }
}