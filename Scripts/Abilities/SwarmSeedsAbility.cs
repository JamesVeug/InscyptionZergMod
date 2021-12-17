using System.Collections;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public class SwarmSeedsAbility : ACustomAbilityBehaviour<SummonZergAbilityData>
	{
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
			if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
				yield return new WaitForSeconds(0.2f);
			}

			CardInfo cardByName = CardLoader.GetCardByName("Broodling");
			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.25f, null);
			yield return new WaitForSeconds(0.45f);
			yield return base.LearnAbility(0.1f);
			yield break;
		}
	}
}