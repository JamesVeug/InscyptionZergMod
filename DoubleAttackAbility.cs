using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;

namespace CardLoaderMod
{
    public class DoubleAttackAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        private List<int> attackedSlots = new List<int>();

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