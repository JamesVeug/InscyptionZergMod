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
        
        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Final Attack";
            info.rulebookDescription = "When this card deals damage it will follow up with one more attack.";
            info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Oof that one will be painful";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,"Artwork/double_attack.png"));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info,typeof(DoubleAttackAbility),tex,AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            DoubleAttackAbility.ability = newAbility.ability;
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
            return target != null && !target.Dead;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();

            CardSlot targetSlot = target.slot;
            int attackingInstanceID = targetSlot.GetInstanceID();
            if (attackedSlots.Contains(attackingInstanceID))
            {
                yield break;
            }

            attackedSlots.Add(attackingInstanceID);
            yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(Card.Slot, targetSlot, 0.1f);
            
            yield return base.LearnAbility(0.25f);
            yield break;
        }

        public override bool RespondsToDealDamageDirectly(int amount)
        {
            return true;
        }

        public override IEnumerator OnDealDamageDirectly(int amount)
        {
            yield return base.PreSuccessfulTriggerSequence();
            
            yield return new WaitForSeconds(0.5f);
            yield return Singleton<CombatPhaseManager>.Instance.VisualizeCardAttackingDirectly(Card.Slot, Card.slot.opposingSlot, amount);
            yield return new WaitForSeconds(0.5f);
        }
    }
}