using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class DoubleAttackAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;
        
        private const int PowerLevel = 0;
        private const string SigilID = "Double Attack";
        private const string SigilName = "Double Attack";
        private const string Description = "When a card bearing this sigil deals damage to a card and survives, it will perform one additional attack.";
        private const string TextureFile = "Artwork/double_attack.png";
        private const string LearnText = "One attack just isn't enough is it?";

        private List<int> attackedSlots = new List<int>();
        private CardSlot directAttackSlot = null; // Gross yes. But i'm lazy

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
                abilityBehaviour: typeof(DoubleAttackAbility), 
                tex: Utils.GetTextureFromPath(TextureFile),
                id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
            );
            DoubleAttackAbility.ability = newAbility.ability;
        }

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return attacker == this.Card;
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            directAttackSlot = slot;
            yield return null;
        }

        public override bool RespondsToAttackEnded()
        {
            return attackedSlots.Count > 0;
        }

        public override IEnumerator OnAttackEnded()
        {
            attackedSlots.Clear();
            yield return null;
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            if (Card.Dead)
            {
                return false;
            }
            
            if (AlreadyDoubleHitSlot() && !target.Dead && !Card.Dead)
            {
                return false;
            }

            if (target.Dead)
            {
                // Hit a card if it's replaced the slot (Anglers Fish Bucket)
                CardSlot cardSlot = Utils.GetSlot(target);
                if (cardSlot.Card == null || cardSlot.Card.Dead)
                {
                    return false;
                }
            }
            
            return true;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();

            CardSlot targetSlot = directAttackSlot;
            int attackingInstanceID = targetSlot.GetInstanceID();
            attackedSlots.Add(attackingInstanceID);
            
            yield return new WaitForSeconds(0.2f);
            yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(Card.Slot, targetSlot);
            yield return new WaitForSeconds(0.2f);

            yield return base.LearnAbility(0.0f);
        }

        private bool AlreadyDoubleHitSlot()
        {
            CardSlot targetSlot = directAttackSlot;
            int attackingInstanceID = targetSlot.GetInstanceID();
            return attackedSlots.Contains(attackingInstanceID);
        }
    }
}