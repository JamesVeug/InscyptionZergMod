﻿using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public class SwarmSeedsAbility : ACustomAbilityBehaviour<SwarmSeedsAbility, AbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(declaringType);
		}
		
		public override bool RespondsToResolveOnBoard()
		{
			return !Card.Dead && Card.slot.IsPlayerSlot;
		}

		public override IEnumerator OnResolveOnBoard()
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

			CardInfo cardByName = CardLoader.GetCardByName("Broodling");
			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.25f, null);
			yield return new WaitForSeconds(0.45f);
			
			if (currentView != View.Hand)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(currentView, false, false);
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
}