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
            info.rulebookName = "Double Attack";
            info.rulebookDescription = "When a card bearing this sigil deals damage to a card and survives, it will perform one additional attack.";
            info.metaCategories = new List<AbilityMetaCategory>
                { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "One attack just isn't enough is it?";
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
            return !AlreadyDoubleHitSlot() && !target.Dead && !Card.Dead;
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