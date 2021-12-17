using System;
using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Cards;
using Random = UnityEngine.Random;

namespace ZergMod.Scripts.Abilities
{
    public class SummonZergAbility : AbilityBehaviour
	{
		public override Ability Ability => ability;
		public static Ability ability;
        
		private const int PowerLevel = 0;
		private const string SigilID = "Summon Zerg";
		private const string SigilName = "Summon Zerg";
		private const string Description = "When a card bearing this sigil takes damage it will create a random zerg card in your hand";
		private const string TextureFile = "Artwork/Cards/drone.png";
		private const string LearnText = "The swarm grows";

		// First value = Weight for drop. Higher means larger chance to obtain it
		// Second value = Name of what card will selected
		private static int m_totalWeights = 0; 
		private static List<Tuple<int, string>> m_dropDates = new List<Tuple<int, string>>()
		{
			new Tuple<int, string>(55, Zerglings.ID),
			new Tuple<int, string>(35, Roach.ID),
			new Tuple<int, string>(35, Hydralisk.ID),
			new Tuple<int, string>(35, Corruptor.ID),
			new Tuple<int, string>(30, Scourge.ID),
			new Tuple<int, string>(30, Banelings.ID),
			new Tuple<int, string>(30, Mutalisk.ID),
			new Tuple<int, string>(30, Lurker.ID),
			new Tuple<int, string>(20, SwarmHost.ID),
			new Tuple<int, string>(20, Broodlord.ID),
			new Tuple<int, string>(20, Ultralisk.ID),
			new Tuple<int, string>(15, Viper.ID),
			new Tuple<int, string>(10, Guardian.ID),
			new Tuple<int, string>(5, Infestor.ID),
		};

		public static void Initialize()
		{
			// Sort by ascending drop rates
			m_dropDates.Sort((a,b)=>a.Item1 - b.Item1);
			foreach (Tuple<int,string> data in m_dropDates)
			{
				m_totalWeights += data.Item1;
			}
			
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = PowerLevel;
			info.rulebookName = SigilName;
			info.rulebookDescription = Description;
			info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

			if (!string.IsNullOrEmpty(LearnText))
			{
				List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
				DialogueEvent.Line line = new DialogueEvent.Line();
				line.text = LearnText;
				lines.Add(line);
				info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);
			}

			NewAbility newAbility = new NewAbility(
				info: info, 
				abilityBehaviour: typeof(SummonZergAbility), 
				tex: Utils.GetTextureFromPath(TextureFile),
				id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
			);
			SummonZergAbility.ability = newAbility.ability;
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
			int expectedWeight = Random.Range(0, m_totalWeights);
			int currentWeight = 0;
			foreach (Tuple<int,string> data in m_dropDates)
			{
				currentWeight += data.Item1;
				if (currentWeight >= expectedWeight)
				{
					return CardLoader.GetCardByName(data.Item2);
				}
			}

			return CardLoader.GetCardByName(m_dropDates[m_dropDates.Count - 1].Item2);
		}
	}
}