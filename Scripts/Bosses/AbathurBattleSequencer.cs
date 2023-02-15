using System.Collections;
using UnityEngine;
using ZergMod;
using ZergMod.Scripts.Bosses;
using ZergMod.Scripts.Encounters;

namespace DiskCardGame.Zerg
{
	public class AbathurBattleSequencer : Part1BossBattleSequencer
	{
		public static string ID = "Zerg.AbathurBattleSequencer";

		public override Opponent.Type BossType => AbathurOpponent.ID;
		public override StoryEvent DefeatedStoryEvent => StoryEvent.LeshyDefeated;
		
		private bool eggDialoguePlayed;
		
		public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
		{
			EncounterData encounterData = base.BuildCustomEncounter(nodeData);
			encounterData.Blueprint = AbathurEncounter.Instance;
			encounterData.opponentTurnPlan = EncounterBuilder.BuildOpponentTurnPlan(encounterData.Blueprint, nodeData.difficulty + RunState.Run.DifficultyModifier, true);
			if (nodeData.difficulty >= 15)
			{
				EncounterData.StartCondition startCondition = new EncounterData.StartCondition();
				startCondition.cardsInOpponentSlots[0] = CardLoader.GetCardByName("Zerg_JSON_AbathurEgg");
				//startCondition.cardsInOpponentSlots[1] = CardLoader.GetCardByName("Zerg_JSON_AbathurEgg");
				startCondition.cardsInOpponentSlots[3] = CardLoader.GetCardByName("Zerg_JSON_AbathurEgg");
				encounterData.startConditions.Add(startCondition);
			}
			return encounterData;
		}
		
		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
		{
			return card.Info.name == "Zerg_JSON_AbathurEgg" && (deathSlot.Card == null || deathSlot.Card.Dead);
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
		{
			yield return new WaitForSeconds(0.2f);
			CardInfo cardByName = CardLoader.GetCardByName("Zerg_JSON_Zerglings");
			yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, deathSlot, 0.1f, true);
			yield return new WaitForSeconds(0.25f);
			if (!this.eggDialoguePlayed)
			{
				this.eggDialoguePlayed = true;
				yield return new WaitForSeconds(0.5f);
				yield return BossUtils.AbathurMessage("Poor decision", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);

			}
			
			yield return DrawBiomassCard();
			yield return new WaitForSeconds(0.1f);
		}

		private IEnumerator DrawBiomassCard()
		{
			if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
				yield return new WaitForSeconds(0.2f);
			}
			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("Zerg_JSON_biomass"), null, 0.25f, null);
			yield return new WaitForSeconds(0.45f);
		}
	}
}