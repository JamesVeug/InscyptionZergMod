using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public class DoubleAttackAbility : ACustomAbilityBehaviour<AbilityData>
    {
        public override Ability Ability => ability;
        public static Ability ability = Ability.None;
        
        private List<int> attackedSlots = new List<int>();
        private CardSlot directAttackSlot = null; // Gross yes. But i'm lazy
		
        public static void Initialize(Type declaringType)
        {
            ability = InitializeBase(declaringType);
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