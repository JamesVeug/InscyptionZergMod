using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using NodeAPI;
using UnityEngine;
using ZergMod;

namespace SpritzMod.Scripts
{
    public class EvolveSequencer : CustomSpecialNodeSequencer
    {
        private CardMergeSequencer sequencer = null;
        private CardSingleChoicesSequencer cardChoiceSequencer = null;
        private Texture eyeTexture = null;

        public static void Initialize()
        {
            /*Can be NewNode.MapNodeType.Other or NewNode.MapNodeType.SpecialCardChoice*/
            NewNode.MapNodeType mapNodeType = NewNode.MapNodeType.SpecialCardChoice;

            List<Texture2D> mapAnimationFrames = new List<Texture2D>
            {
                Utils.GetTextureFromPath("Artwork/Sequencers/evolve_1.png"),
                Utils.GetTextureFromPath("Artwork/Sequencers/evolve_2.png"),
                Utils.GetTextureFromPath("Artwork/Sequencers/evolve_3.png"),
                Utils.GetTextureFromPath("Artwork/Sequencers/evolve_4.png")
            };

            /*can be empty*/
            List<NodeData.SelectionCondition> generationPrerequisites = new List<NodeData.SelectionCondition>
            {
	            
            };
            
            /*conditions at which the node always generates instead of other nodes, can be empty*/
            /*you can use something like new CustomPreviousNodesContent("Nodes_Name_For_Code_Purposes", false) (this type is built into nodeapi) for it to always generate once for debug*/
            List<NodeData.SelectionCondition> forceGenerationConditions = new List<NodeData.SelectionCondition>
            {
                new CustomPreviousNodesContent("EvolveSequence", false)
            };
            
            new NewNode("EvolveSequence", mapNodeType , mapAnimationFrames, typeof(EvolveSequencer), 
                generationPrerequisites,
                forceGenerationConditions);
        }

        private CardMergeSequencer CloneCardMergeSequence()
        {
	        CardMergeSequencer clone = Instantiate(GameObject.FindObjectOfType<CardMergeSequencer>(), transform);
	        GameObject backRock = Utils.FindObjectInChildren(clone.gameObject, "Back Rock");
	        GameObject quad = Utils.FindObjectInChildren(backRock, "Quad");

	        Texture t = Utils.GetTextureFromPath("Artwork/Sequencers/card_slot_host_evolve.png");
	        Renderer renderer = quad.GetComponent<Renderer>();
	        renderer.material.mainTexture = t;

	        return clone;
        }
        
        public EvolveSequencer()
        {
	        this.sequencer = CloneCardMergeSequence();

	        DuplicateMergeSequencer duplicateMergeSequencer = GameObject.FindObjectOfType<DuplicateMergeSequencer>();
	        this.cardChoiceSequencer = Instantiate(duplicateMergeSequencer.cardChoiceSequencer, transform);
		        
	        DeckTrialSequencer deckTrialSequencer = GameObject.FindObjectOfType<DeckTrialSequencer>();
	        eyeTexture = deckTrialSequencer.snakeEyeTexture;
        }

        public override IEnumerator DoCustomSequence()
        {
            sequencer.hostSlot.Disable();
            sequencer.sacrificeSlot.Disable();
            Singleton<TableRuleBook>.Instance.SetOnBoard(true);
            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.CardMerging, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
            
            
            Singleton<ExplorableAreaManager>.Instance.TweenHangingLightColors(new Color(0, 0.5f, 0, 1), new Color(0, 0.3f, 0, 1), 0.1f);
            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
	            "You encounter a large strange green slug like looking creature that slithers towards you.", 
	            -2.5f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter);
            
            yield return new WaitForSeconds(0.3f);
            LeshyAnimationController.Instance.SetEyesTexture(eyeTexture);
            
            sequencer.stoneCircleAnim.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            yield return sequencer.pile.SpawnCards(RunState.DeckList.Count, 0.5f);
            List<CardInfo> validHosts = this.GetValidCardsForHost(null);
            List<CardInfo> validSacrifices = this.GetValidCardsForSacrifice(null);
            bool singleValidHostIsSameAsSacrifice = validHosts.Count == 1 && validSacrifices.Count == 1 && validHosts[0] == validSacrifices[0];
            bool hasNoValidOptions = validHosts.Count == 0 || validSacrifices.Count == 0 || singleValidHostIsSameAsSacrifice;
            if (hasNoValidOptions)
            {
                yield return InvalidCardsSequence();
            }
            else
            {
	            yield return AbathurMessage("Primitive Army");
	            yield return AbathurMessage("Potential hidden evolution available");
	            yield return AbathurMessage("Require Biomass extraction from weaker cards to unlock");
	            yield return ValidSequence();
	            yield return AbathurMessage("Army now more efficient. Perfection is inevitable.");
            }
            
            Singleton<ExplorableAreaManager>.Instance.ResetHangingLightsToZoneColors(0.25f);
			yield return sequencer.pile.DestroyCards(0.5f);
			sequencer.stoneCircleAnim.SetTrigger("exit");
			yield return new WaitForSeconds(0.25f);
			LeshyAnimationController.Instance.ResetEyesTexture();
			sequencer.confirmStone.Exit();
			yield return new WaitForSeconds(0.75f);
			sequencer.stoneCircleAnim.gameObject.SetActive(false);
			sequencer.confirmStone.SetStoneInactive();
			sequencer.sacrificeSlot.DestroyCard();
			sequencer.hostSlot.DestroyCard();
        }

        private IEnumerator ValidSequence()
        {
	        Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, false);
	        sequencer.sacrificeSlot.RevealAndEnable();
	        sequencer.sacrificeSlot.ClearDelegates();
	        SelectCardFromDeckSlot selectCardFromDeckSlot = sequencer.sacrificeSlot;
	        selectCardFromDeckSlot.CursorSelectStarted = (Action<MainInputInteractable>)Delegate.Combine(
		        selectCardFromDeckSlot.CursorSelectStarted, new Action<MainInputInteractable>(this.OnSlotSelected));

	        sequencer.hostSlot.RevealAndEnable();
	        sequencer.hostSlot.ClearDelegates();
	        SelectCardFromDeckSlot selectCardFromDeckSlot2 = sequencer.hostSlot;
	        selectCardFromDeckSlot2.CursorSelectStarted = (Action<MainInputInteractable>)Delegate.Combine(
		        selectCardFromDeckSlot2.CursorSelectStarted, new Action<MainInputInteractable>(this.OnSlotSelected));

	        yield return sequencer.confirmStone.WaitUntilConfirmation();
	        sequencer.hostSlot.Disable();
	        sequencer.sacrificeSlot.Disable();
	        yield return new WaitForSeconds(0.25f);
	        Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, true);
	        Singleton<RuleBookController>.Instance.SetShown(false, true);
	        yield return new WaitForSeconds(1f);
	        foreach (SpecialCardBehaviour cardBehaviour in sequencer.hostSlot.Card.GetComponents<SpecialCardBehaviour>())
	        {
		        yield return cardBehaviour.OnSelectedForCardMergeHost();
	        }

	        // Remove sacrifice
	        CardInfo sacrificedInfo = sequencer.sacrificeSlot.Card.Info;
	        RunState.Run.playerDeck.RemoveCard(sacrificedInfo);
	        sequencer.sacrificeSlot.Card.Anim.SetSacrificeHoverMarkerShown(false);
	        sequencer.sacrificeSlot.Card.Anim.PlayDeathAnimation(false);
	        AudioController.Instance.PlaySound3D("sacrifice_default", MixerGroup.TableObjectsSFX, base.transform.position, 1f,
		        0f, null, null, null, null, false);
	        yield return new WaitForSeconds(0.5f);
	        AudioController.Instance.PlaySound3D("card_blessing", MixerGroup.TableObjectsSFX,
		        sequencer.hostSlot.transform.position, 1f, 0f, new AudioParams.Pitch(0.6f), null, null, null, false);
	        sequencer.hostSlot.Card.Anim.PlayTransformAnimation();
	        yield return new WaitForSeconds(0.15f);
	        
	        // Evolve current card
	        CardInfo evolvedInfo = (CardInfo)sequencer.hostSlot.Card.Info.evolveParams.evolution.Clone();
	        evolvedInfo.Mods.AddRange(sequencer.hostSlot.Card.Info.Mods);
	        RunState.Run.playerDeck.RemoveCard(sequencer.hostSlot.Card.Info);
	        sequencer.hostSlot.Card.SetInfo(evolvedInfo);
	        sequencer.hostSlot.Card.SetInteractionEnabled(false);
	        RunState.Run.playerDeck.AddCard(evolvedInfo);
	        yield return null;
	        sequencer.hostSlot.Card.RenderCard();
	        
	        yield return new WaitForSeconds(1.5f);

	        Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
	        yield return new WaitForSeconds(0.25f);
	        sequencer.hostSlot.FlyOffCard();
        }

        private List<CardInfo> GetValidCardsForHost(CardInfo sacrifice = null)
        {
            List<CardInfo> list = new List<CardInfo>(RunState.DeckList);
            list.RemoveAll((a) => a.evolveParams == null);
            return list;
        }

        private List<CardInfo> GetValidCardsForSacrifice(CardInfo host = null)
        {
            List<CardInfo> list = new List<CardInfo>(RunState.DeckList);
            return list;
        }

        private IEnumerator AbathurMessage(string message)
        {
	        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
		        "[c:light_green]" + message + "[c:]", 
		        -2.5f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter);
        }
        
        private IEnumerator InvalidCardsSequence()
        {
	        CardChoicesNodeData nodeData = new CardChoicesNodeData();
	        nodeData.overrideChoices = this.GetEvolvingCardChoices();
	        if (nodeData.overrideChoices.Count > 0)
	        {
		        yield return AbathurMessage("Army strands not capable of improving. Choose strand to join Army.");
		        
		        Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, false);
		        yield return new WaitForSeconds(0.1f);
		        yield return cardChoiceSequencer.CardSelectionSequence(nodeData);
	        }
	        else
	        {
		        yield return AbathurMessage("Army strands not capable of improving. Need more samples!");
	        }
	        yield return new WaitForSeconds(0.1f);
        }
        
        private List<CardChoice> GetEvolvingCardChoices()
        {
	        List<CardInfo> allCards = new List<CardInfo>(CardLoader.AllData);
	        allCards.RemoveAll((CardInfo x) => x.evolveParams == null);
	        
	        List<CardChoice> finalList = new List<CardChoice>();
	        int currentRandomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
	        while (allCards.Count > 0 && finalList.Count < 3)
	        {
		        CardInfo cardInfo = allCards[SeededRandom.Range(0, allCards.Count, currentRandomSeed++)];
		        finalList.Add(new CardChoice
		        {
			        CardInfo = CardLoader.GetCardByName(cardInfo.name)
		        });
		        allCards.Remove(cardInfo);
	        }
	        return finalList;
        }
        
        private void OnSlotSelected(MainInputInteractable slot)
        {
	        Singleton<RuleBookController>.Instance.SetShown(false, true);
	        sequencer.sacrificeSlot.SetEnabled(false);
	        sequencer.hostSlot.SetEnabled(false);
	        sequencer.hostSlot.ShowState(HighlightedInteractable.State.NonInteractable, false, 0.15f);
	        sequencer.sacrificeSlot.ShowState(HighlightedInteractable.State.NonInteractable, false, 0.15f);
	        sequencer.confirmStone.Exit();
	        bool flag = slot == sequencer.hostSlot;
	        List<CardInfo> list;
	        if (flag)
	        {
		        list = this.GetValidCardsForHost((sequencer.sacrificeSlot.Card == null) ? null : sequencer.sacrificeSlot.Card.Info);
	        }
	        else
	        {
		        bool flag2 = sequencer.sacrificeSlot.Card != null;
		        if (flag2)
		        {
			        sequencer.sacrificeSlot.Card.Anim.SetSacrificeHoverMarkerShown(false);
		        }
		        list = this.GetValidCardsForSacrifice((sequencer.hostSlot.Card == null) ? null : sequencer.hostSlot.Card.Info);
	        }
	        bool flag3 = sequencer.hostSlot.Card != null && slot == sequencer.sacrificeSlot;
	        if (flag3)
	        {
		        list.Remove(sequencer.hostSlot.Card.Info);
	        }
	        bool flag4 = sequencer.sacrificeSlot.Card != null && slot == sequencer.hostSlot;
	        if (flag4)
	        {
		        list.Remove(sequencer.sacrificeSlot.Card.Info);
	        }
	        bool flag5 = list.Count == 0;
	        if (flag5)
	        {
		        bool flag6 = sequencer.noValidChoicesDialogueEvent != null;
		        if (flag6)
		        {
			        base.StopCoroutine(sequencer.noValidChoicesDialogueEvent);
		        }
		        sequencer.noValidChoicesDialogueEvent = base.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowThenClear("There are no compatible sacrifices... It won't work.", 3.5f, 0f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null));
	        }
	        SelectCardFromDeckSlot selectFromDeckSlot = slot as SelectCardFromDeckSlot;
	        selectFromDeckSlot.SelectFromCards(list, delegate
	        {
		        this.OnSelectionEnded(selectFromDeckSlot);
	        }, true);
        }
        
        private void OnSelectionEnded(SelectCardFromDeckSlot slot)
        {
	        Singleton<RuleBookController>.Instance.SetShown(false, true);
	        sequencer.sacrificeSlot.SetShown(true, false);
	        sequencer.sacrificeSlot.ShowState(HighlightedInteractable.State.Interactable, false, 0.15f);
	        sequencer.hostSlot.SetShown(true, false);
	        sequencer.hostSlot.ShowState(HighlightedInteractable.State.Interactable, false, 0.15f);
	        Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, true);
	        bool flag = slot == sequencer.sacrificeSlot && sequencer.sacrificeSlot.Card != null;
	        if (flag)
	        {
		        sequencer.sacrificeSlot.Card.Anim.SetSacrificeHoverMarkerShown(true);
	        }
	        bool flag2 = sequencer.hostSlot.Card != null && sequencer.sacrificeSlot.Card != null;
	        if (flag2)
	        {
		        sequencer.confirmStone.Enter();
	        }
        }
    }
}