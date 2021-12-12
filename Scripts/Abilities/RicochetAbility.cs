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
        private const string Description = "When a card bearing this sigil deals damage to a card it will also try hitting over the card for 1 damage.";
        private const string TextureFile = "Artwork/Sigils/ricochet.png";
        private const string LearnText = "";

        private const int Ricochet_Damage = 1;
        private bool activated = false;

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
            return !activated && !Card.Dead;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            activated = true;
            
            // Wait for them to finish the attack
            yield return new WaitForSeconds(0.125f);
            
            // Switch to Combat view
            if (Singleton<ViewManager>.Instance.CurrentView != Singleton<BoardManager>.Instance.CombatView)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
                yield return new WaitForSeconds(0.2f);
            }

            // Adjust settings
            CardModificationInfo flyingModifier = new CardModificationInfo
            {
                attackAdjustment = -Card.Attack + Ricochet_Damage, // Set the damage to 1
                abilities = new List<Ability> { Ability.Flying } // Allow flying over targets
            };
            Card.AddTemporaryMod(flyingModifier);
            
            // Attack
            yield return new WaitForSeconds(0.2f);
            yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(Card.Slot, Utils.GetSlot(target));
            yield return new WaitForSeconds(0.2f);
            
            // Revert settings
            Card.RemoveTemporaryMod(flyingModifier);
            
            // Learn
            yield return base.LearnAbility(0f);
            activated = false;
        }
    }
}