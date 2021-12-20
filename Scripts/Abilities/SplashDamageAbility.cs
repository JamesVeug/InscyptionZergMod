using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public class SplashDamageAbility : ACustomAbilityBehaviour<AbilityData>
    {
        public override Ability Ability => ability;
        public static Ability ability = Ability.None;
		
        private bool activated = false;
        
        public static void Initialize(Type declaringType)
        {
            ability = InitializeBase(declaringType);
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            CardSlot slot = Utils.GetSlot(target);
            return !activated && slot != null;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return this.PreSuccessfulTriggerSequence();

            if (Singleton<ViewManager>.Instance.CurrentView != View.Board)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            
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