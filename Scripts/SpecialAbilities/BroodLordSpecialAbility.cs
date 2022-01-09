using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class BroodLordSpecialAbility : ACustomSpecialAbilityBehaviour<BroodLordSpecialAbilityData>
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;

        private bool triggered = false;
        
        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(declaringType);
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return !triggered && !PlayableCard.Dead && playerUpkeep && PlayableCard.slot.IsPlayerSlot;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            triggered = true; // Because this triggers twice for some reason. 
            
            View currentView = Singleton<ViewManager>.Instance.CurrentView;
            if (currentView != View.Hand)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                yield return new WaitForSeconds(0.2f);
            }

            CardInfo cardByName = CardLoader.GetCardByName(LoadedData.cardCreatedName);
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);
			
            if (currentView != View.Hand)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(currentView, false, false);
                yield return new WaitForSeconds(0.2f);
            }
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return triggered;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            triggered = false;
            yield return null;
        }
    }
}