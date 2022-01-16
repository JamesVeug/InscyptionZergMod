using APIPlugin;
using DiskCardGame;
using HarmonyLib;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (AbilitiesUtil), "GetInfo", new System.Type[] {typeof (Ability)})]
    public class AbilitiesUtil_GetInfo
    {
        public static bool Prefix(ref Ability ability, ref AbilityInfo __result)
        {
            if (ability == Ability.None)
            {
                Plugin.Log.LogWarning("[AbilitiesUtil_GetInfo] Ability is None. Overriding with TailOnHit because i'm lazy");
                ability = Ability.TailOnHit; // Because why not
                return true;
            }

            if (ability < Ability.NUM_ABILITIES)
                return true;
            
            foreach (NewAbility newAbility in NewAbility.abilities)
            {
                if (newAbility.ability == ability)
                {
                    __result = newAbility.info;
                    return false;
                }
            }
            
            return true;
        }
    }
}