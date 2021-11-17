using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace CardLoaderMod
{
    public class ZerglingEvolveAbility : AbilityBehaviour
    {
        public override Ability Ability
        {
            get { return ability; }
        }

        public static Ability ability;

        protected string SpawnedCardId => "Zergling";
        protected string CannotSpawnDialogue => "No spaces left to spawn!";

        private int numTurnsInPlay;

        private bool CanCreateExtraCards()
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;

            return toLeftValid && toRightValid;
        }

        // Token: 0x0600133A RID: 4922 RVA: 0x00043747 File Offset: 0x00041947
        public IEnumerator SpawnExtraCards()
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            if (toLeftValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toLeft);
            }

            if (toRightValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toRight);
            }

            if (toLeftValid || toRightValid)
            {
                yield return base.LearnAbility(0f);
            }

            yield break;
        }
	    
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }
	    
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            int num = (base.Card.Info.evolveParams != null) ? base.Card.Info.evolveParams.turnsToEvolve : 1;
            this.numTurnsInPlay++;
            int num2 = Mathf.Max(1, num - this.numTurnsInPlay);
            base.Card.RenderInfo.OverrideAbilityIcon(Ability.Evolve, ResourceBank.Get<Texture>("Art/Cards/AbilityIcons/ability_evolve_" + num2));
            base.Card.RenderCard();
            if (this.numTurnsInPlay >= num)
            {
                if (!CanCreateExtraCards())
                {
                    if (!base.HasLearned && (Localization.CurrentLanguage == Language.English ||
                                             Localization.Translate(this.CannotSpawnDialogue) !=
                                             this.CannotSpawnDialogue))
                    {
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(this.CannotSpawnDialogue, -0.65f,
                            0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null,
                            true);
                    }
                    yield break;
                }
			    
                CardInfo evolution = this.GetTransformCardInfo();
                if (true)
                {
                    foreach (CardModificationInfo cardModificationInfo in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                    {
                        CardModificationInfo cardModificationInfo2 = (CardModificationInfo)cardModificationInfo.Clone();
                        if (cardModificationInfo2.HasAbility(Ability.Evolve))
                        {
                            cardModificationInfo2.abilities.Remove(Ability.Evolve);
                        }
                        evolution.Mods.Add(cardModificationInfo2);
                    }
                }
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.Card.TransformIntoCard(evolution, new Action(this.RemoveTemporaryModsWithEvolve));
                yield return new WaitForSeconds(0.15f);

                yield return SpawnExtraCards();
                yield return new WaitForSeconds(0.25f);
                evolution = null;
            }
            yield break;
        }

        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(
                CardLoader.GetCardByName(this.SpawnedCardId), slot, 0.15f, true);
            yield break;
        }
	    
        private void RemoveTemporaryModsWithEvolve()
        {
            for (CardModificationInfo temporaryEvolveMod = this.GetTemporaryEvolveMod(); temporaryEvolveMod != null; temporaryEvolveMod = this.GetTemporaryEvolveMod())
            {
                base.Card.RemoveTemporaryMod(temporaryEvolveMod, true);
            }
        }
	    
        private CardModificationInfo GetTemporaryEvolveMod()
        {
            return base.Card.TemporaryMods.Find((CardModificationInfo x) => x.abilities.Contains(Ability.Evolve));
        }
	    
        protected virtual CardInfo GetTransformCardInfo()
        {
            if (base.Card.Info.evolveParams == null)
            {
                return EvolveParams.GetDefaultEvolution(base.Card.Info);
            }
            return base.Card.Info.evolveParams.evolution.Clone() as CardInfo;
        }
    }
}