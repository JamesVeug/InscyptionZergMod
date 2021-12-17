using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data;
using Random = UnityEngine.Random;

namespace ZergMod.Scripts.Abilities
{
    public class SummonZergAbility : ACustomAbilityBehaviour<SummonZergAbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		// First value = Weight for drop. Higher means larger chance to obtain it
		// Second value = Name of what card will selected
		private static int m_totalWeights = 0; 

		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(declaringType);
			
			// Sort by ascending drop rates
			LoadedData.cardEvolutions.Sort((a,b)=>a.weight - b.weight);
			foreach (WeightData data in LoadedData.cardEvolutions)
			{
				m_totalWeights += data.weight;
			}
		}

		public override bool RespondsToTakeDamage(PlayableCard source)
		{
			return true;
		}

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			CardInfo cardInfo = GetRandomCard();
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

		private CardInfo GetRandomCard()
		{
			List<WeightData> evolutions = LoadedData.cardEvolutions;
			
			int expectedWeight = Random.Range(0, m_totalWeights);
			int currentWeight = 0;
			foreach (WeightData data in evolutions)
			{
				currentWeight += data.weight;
				if (currentWeight >= expectedWeight)
				{
					return CardLoader.GetCardByName(data.cardName);
				}
			}

			return CardLoader.GetCardByName(evolutions[evolutions.Count - 1].cardName);
		}
	}
}