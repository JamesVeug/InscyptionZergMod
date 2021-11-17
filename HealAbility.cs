using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace CardLoaderMod
{
    public class HealAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return base.PreSuccessfulTriggerSequence();
		        
            if (Singleton<ViewManager>.Instance.CurrentView != View.Board)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.2f);
            }

            PlayableCard playableCard = this.Card;
            if (playableCard.Status.damageTaken <= 0)
            {
                yield break;
            }
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);
            base.Card.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
            playableCard.Status.damageTaken--;
            yield return new WaitForSeconds(0.5f);
            
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}