using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace CardLoaderMod
{
    public class RegestateAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

            CardInfo cardByName = (CardInfo)NewCard.cards.Find(info => info.displayedName == "Egg").Clone();
            cardByName.evolveParams = new EvolveParams{turnsToEvolve = 2, evolution = (CardInfo)Card.Info.Clone()};
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, Card.slot, 0.15f, true);

            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}