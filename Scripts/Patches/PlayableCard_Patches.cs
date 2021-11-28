using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace ZergMod.Patches
{
    /*[HarmonyPatch(typeof (PlayableCard), "TakeDamage", new System.Type[] {typeof (int), typeof (PlayableCard)})]
    public class PlayableCard_TakeDamage
    {
        public static IEnumerator Postfix(IEnumerator __result, int damage, PlayableCard attacker)
        {
            Plugin.Log.LogInfo("[PlayableCard_TakeDamage] Deal damage: " + attacker.Info.displayedName + " " + damage);
            if (attacker.HasAbility(RicochetAbility.ability))
            {
                CombatPhaseManager3D combatManager = Singleton<CombatPhaseManager3D>.Instance;
                var prop = combatManager.GetType().GetProperty("DamageDealtThisPhase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                int DamageDealtThisPhase = (int)prop.GetValue(combatManager);
                int newDamageDealt = DamageDealtThisPhase + 1;
                
                Plugin.Log.LogInfo("[PlayableCard_TakeDamage] new damage dealt: " + DamageDealtThisPhase + " -> " + newDamageDealt);
                prop.SetValue(combatManager, newDamageDealt);
                
                Plugin.Log.LogInfo("[PlayableCard_TakeDamage] visualizing direct damage");
                yield return combatManager.VisualizeCardAttackingDirectly(ZergMod.Utils.GetSlot(attacker), ZergMod.Utils.GetSlot(attacker), damage);
                Plugin.Log.LogInfo("[PlayableCard_TakeDamage] visualizing direct damage done");
                if (attacker.TriggerHandler.RespondsToTrigger(Trigger.DealDamageDirectly, new object[]
                {
                    attacker.Attack
                }))
                {
                    yield return attacker.TriggerHandler.OnTrigger(Trigger.DealDamageDirectly, new object[]
                    {
                        attacker.Attack
                    });
                }
                Plugin.Log.LogInfo("[PlayableCard_TakeDamage] done");
            }
        }
    }*/
}