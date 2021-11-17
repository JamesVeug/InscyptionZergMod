using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod
{
    public class DoubleAttackAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        private List<int> attackedSlots = new List<int>();
        private CardSlot directAttackSlot = null; // Gross yes. But i'm lazy

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Final Attack";
            info.rulebookDescription = "When this card deals damage it will follow up with one more attack.";
            info.metaCategories = new List<AbilityMetaCategory>
                { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Oof that one will be painful";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/double_attack.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info, typeof(DoubleAttackAbility), tex,
                AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
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

        public override IEnumerator OnAttackEnded()
        {
            attackedSlots.Clear();
            yield return null;
        }

        public override bool RespondsToAttackEnded()
        {
            return attackedSlots.Count > 0;
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return !AlreadyDoubleHitSlot();
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();

            CardSlot targetSlot = directAttackSlot;
            int attackingInstanceID = targetSlot.GetInstanceID();
            attackedSlots.Add(attackingInstanceID);
            
            yield return new WaitForSeconds(0.4f);
            if (target.Dead)
            {
                // Deal damage directly
                yield return Singleton<CombatPhaseManager>.Instance.VisualizeCardAttackingDirectly(Card.Slot, targetSlot, amount);
            }
            else
            {
                // Hit the same card again
                yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(Card.Slot, targetSlot);
            }
            yield return new WaitForSeconds(0.4f);

            yield return base.LearnAbility(0.0f);
        }

        private bool AlreadyDoubleHitSlot()
        {
            CardSlot targetSlot = directAttackSlot;
            int attackingInstanceID = targetSlot.GetInstanceID();
            return attackedSlots.Contains(attackingInstanceID);
        }

        public override bool RespondsToDealDamageDirectly(int amount)
        {
            return !AlreadyDoubleHitSlot();
        }

        public override IEnumerator OnDealDamageDirectly(int amount)
        {
            yield return base.PreSuccessfulTriggerSequence();

            yield return new WaitForSeconds(0.5f);
            yield return Singleton<CombatPhaseManager>.Instance.VisualizeCardAttackingDirectly(Card.Slot, directAttackSlot, amount);
            yield return new WaitForSeconds(0.5f);
            
            yield return base.LearnAbility(0.0f);
        }
    }
}