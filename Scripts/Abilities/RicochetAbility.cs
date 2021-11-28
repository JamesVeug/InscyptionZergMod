using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class RicochetAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        private const int Ricochet_Damage = 1;

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Ricochet";
            info.rulebookDescription = "When a card bearing this sigil deals damage to a card it also hits face for 1 damage.";
            info.metaCategories = new List<AbilityMetaCategory>
                { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/ricochet.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info, typeof(RicochetAbility), tex,
                AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            RicochetAbility.ability = newAbility.ability;
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return true;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return new WaitForSeconds(0.125f);
            
            if (Singleton<ViewManager>.Instance.CurrentView != Singleton<BoardManager>.Instance.CombatView)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            
            AddTeeth();

            CombatPhaseManager3D instance = Singleton<CombatPhaseManager3D>.Instance;
            yield return instance.VisualizeCardAttackingDirectly(Utils.GetSlot(Card), Utils.GetSlot(target), amount);
            if (Card.TriggerHandler.RespondsToTrigger(Trigger.DealDamageDirectly, new object[]
            {
                Ricochet_Damage
            }))
            {
                yield return Card.TriggerHandler.OnTrigger(Trigger.DealDamageDirectly, new object[]
                {
                    Ricochet_Damage
                });
            }
            yield return new WaitForSeconds(0.2f);
            yield return base.LearnAbility(0f);
        }

        private void AddTeeth()
        {
            CombatPhaseManager combatManager = Singleton<CombatPhaseManager>.Instance;
            var prop = combatManager.GetType().GetProperty("DamageDealtThisPhase",
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.GetProperty | BindingFlags.SetProperty);
            Plugin.Log.LogInfo("[RicochetAbility] prop " + prop);

            int damage = combatManager.GetPrivatePropertyValue<int>("DamageDealtThisPhase");
            Plugin.Log.LogInfo("[RicochetAbility] damagea " + damage);
            int newDamage = damage + Ricochet_Damage;

            Type t = typeof(CombatPhaseManager);
            if (t.GetProperty("DamageDealtThisPhase", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) ==
                null)
                throw new ArgumentOutOfRangeException("propName",
                    string.Format("Property {0} was not found in Type {1}", "CombatPhaseManager", t.FullName));
            t.InvokeMember("DamageDealtThisPhase",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null,
                combatManager, new object[] { newDamage });
        }
    }
}