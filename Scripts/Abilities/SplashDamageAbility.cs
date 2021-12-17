using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public class SplashDamageAbility : ACustomAbilityBehaviour<AbilityData>
    {
        private bool activated = false;

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            CardSlot slot = Utils.GetSlot(target);
            return !Card.Dead && !activated && slot != null;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return this.PreSuccessfulTriggerSequence();

            activated = true;
            
            CardSlot cardSlot = Utils.GetSlot(target);
            List<CardSlot> adjacentSlots = Utils.GetAdjacentSlots(cardSlot);
            foreach (CardSlot slot in adjacentSlots)
            {
                if (slot.Card != null && !slot.Card.Dead && !slot.Card.FaceDown)
                {
                    yield return slot.Card.TakeDamage(amount, Card);
                }
            }

            activated = false;

            yield return base.LearnAbility(0.0f);
        }
    }
}