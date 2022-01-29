using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;
using Random = UnityEngine.Random;

namespace ZergMod.Scripts.Abilities
{
    public class SummonZergAbility : ACustomAbilityBehaviour<SummonZergAbility, SummonZergAbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;

		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(declaringType);
		}

		public override bool RespondsToTakeDamage(PlayableCard source)
		{
			return true;
		}

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			CardInfo cardInfo = Utils.GetRandomWeightedCard(LoadedData.cardEvolutions, LoadedData.TotalWeights);
			if (cardInfo == null)
			{
				if (!Card.Dead)
				{
					Card.Anim.StrongNegationEffect();
					yield return new WaitForSeconds(0.3f);
				}
				yield break;
			}
			
			if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
				yield return new WaitForSeconds(0.2f);
			}

			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardInfo, null, 0.25f, null);
			yield return new WaitForSeconds(0.45f);
			yield return base.LearnAbility(0.1f);
		}
	}
}