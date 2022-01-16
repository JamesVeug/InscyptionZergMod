using APIPlugin;
using DiskCardGame;
using HarmonyLib;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof(RunState), "Initialize")]
    public class RunState_Initialize
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            CustomSaveManager.SaveFile.DehakaKills = 0;
        }
    }
}