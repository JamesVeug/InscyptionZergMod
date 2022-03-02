using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public class SwarmAbility : ACustomAbilityBehaviour<SwarmAbility, SwarmAbilityData>
    {
        public override Ability Ability => SwarmAbility.ability;
        public static Ability ability;
        public static Trigger OnOtherCardAttackEndedTrigger = (Trigger)10000; 
        
        private List<int> attackedSlots = new List<int>();
        
        public static void Initialize(Type declaringType)
        {
            ability = InitializeBase(declaringType);
        }

        public override bool RespondsToAttackEnded()
        {
            return attackedSlots.Count > 0;
        }

        public override IEnumerator OnAttackEnded()
        {
            //Plugin.Log.LogInfo($"[OnAttackEnded] {this.Card.slot.Index}");
            attackedSlots.Clear();
            yield return null;
        }

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            //Plugin.Log.LogInfo($"[RespondsToOtherCardDealtDamage] {this.Card.Info.displayedName}. {attacker.Info.displayedName}->{target.Info.displayedName}");
            //Plugin.Log.LogInfo($"[RespondsToOtherCardDealtDamage] {this.Card.slot.Index}. {attacker.slot.Index}->{target.slot.Index}");

            if (target.Dead)
            {
                //Plugin.Log.LogInfo("[RespondsToOtherCardDealtDamage] target is dead");
                return false;
            }
            
            if (attacker.OpponentCard != this.Card.OpponentCard)
            {
                //Plugin.Log.LogInfo("[RespondsToOtherCardDealtDamage] attacker card not owned by this card");
                return false;
            }

            int slotDifference = Mathf.Abs(attacker.slot.Index - this.Card.slot.Index);
            if (slotDifference != 1)
            {
                //Plugin.Log.LogInfo($"[RespondsToOtherCardDealtDamage] Not a neighbor: {slotDifference}");
                return false;
            }

            if (attackedSlots.Contains(target.slot.Index))
            {
                //Plugin.Log.LogInfo($"[RespondsToOtherCardDealtDamage] Slot already attacked");
                return false;
            }
            
            return true;
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return true;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            attackedSlots.Add(target.slot.Index);
            yield return null;
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return attackedSlots.Contains(deathSlot.Index);
        }

        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            attackedSlots.Remove(deathSlot.Index);
            yield return null;
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            attackedSlots.Add(target.slot.Index);
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(Card.slot, target.slot, 0f);
            yield return base.LearnAbility(0.1f);
            Plugin.Log.LogInfo($"[OnOtherCardDealtDamage] Attacked {target.slot.Index}");
        }
    }
}