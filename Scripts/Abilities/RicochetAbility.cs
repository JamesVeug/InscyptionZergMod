using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private const int PowerLevel = 0;
        private const string SigilID = "Ricochet";
        private const string SigilName = "Ricochet";
        private const string Description = "When a card bearing this sigil deals damage to a card it also hits face for 1 damage.";
        private const string TextureFile = "Artwork/ricochet.png";
        private const string LearnText = "";

        private const int Ricochet_Damage = 1;

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = PowerLevel;
            info.rulebookName = SigilName;
            info.rulebookDescription = Description;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            if (!string.IsNullOrEmpty(LearnText))
            {
                List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
                DialogueEvent.Line line = new DialogueEvent.Line();
                line.text = LearnText;
                lines.Add(line);
                info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);
            }

            NewAbility newAbility = new NewAbility(
                info: info, 
                abilityBehaviour: typeof(RicochetAbility), 
                tex: Utils.GetTextureFromPath(TextureFile),
                id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
            );
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