using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public class SpawnLocustAbility : ACustomAbilityBehaviour<AbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(declaringType);
		}
		
		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return !Card.Dead && !playerTurnEnd && Card.slot.IsPlayerSlot;
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return PreSuccessfulTriggerSequence();
			yield return CreateCards();
			yield return LearnAbility(0f);
		}

		private IEnumerator CreateCards()
		{
			View currentView = Singleton<ViewManager>.Instance.CurrentView;
			if (currentView != View.Hand)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
				yield return new WaitForSeconds(0.2f);
			}

			CardInfo cardByName = CardLoader.GetCardByName("Locust");
			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.25f, null);
			yield return new WaitForSeconds(0.45f);
			yield return base.LearnAbility(0.1f);


			if (currentView != View.Hand)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(currentView, false, false);
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
}