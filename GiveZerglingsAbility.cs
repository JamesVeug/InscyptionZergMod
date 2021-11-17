using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace CardLoaderMod
{
    public class GiveZerglingsAbility : AbilityBehaviour
    {
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public static Ability ability;

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
		    
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }
		    
            CardInfo larva = CardLoader.GetCardByName("Squirrel");
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(larva, null, 0.25f, null);
            yield return new WaitForSeconds(0.25f);
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(larva, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);
            yield return base.LearnAbility(0.1f);
            yield return base.LearnAbility(0f);
            yield break;
        }
	    
        // BoardManager.Instance.LastSelectedSlot.Card
        // BoardManager.Instance.CardsOnBoard
	    
        // public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        // public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        // public override bool RespondsToTakeDamage(PlayableCard source) { return true; }
        // public override IEnumerator OnTakeDamage(PlayableCard source)
    }
}