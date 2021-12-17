using System.Collections;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public class BombardAbility : ACustomAbilityBehaviour<BombardAbilityData>
    {
        public override bool RespondsToDealDamageDirectly(int amount)
        {
            PlayableCard opposingSlotCard = Card.slot.opposingSlot.Card;
            return opposingSlotCard != null && !opposingSlotCard.Dead && !opposingSlotCard.FaceDown;
        }

        public override IEnumerator OnDealDamageDirectly(int amount)
        {
            yield return new WaitForSeconds(0.1f);
            
            PlayableCard opposingSlotCard = Card.slot.opposingSlot.Card;
            yield return opposingSlotCard.TakeDamage(LoadedData.bombardDamage, this.Card);
            
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0f);
        }
    }
}