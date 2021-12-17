using System;
using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using UnityEngine;

namespace ZergMod.Scripts.Abilities
{
    public class AbductAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;
        
        private const int PowerLevel = 0;
        private const string SigilID = "Abduct";
        private const string SigilName = "Abduct";
        private const string Description = "When a card bearing this sigil is played, a targeted enemy card is moved to the space in front of it, if that space is empty";
        private const string TextureFile = "Artwork/Sigils/abduct.png";
        private const string LearnText = "";

        private CardSlot m_targetedCardSlot = null;

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = PowerLevel;
            info.rulebookName = SigilName;
            info.rulebookDescription = Description;
            info.metaCategories = new List<AbilityMetaCategory>
                { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

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
	            abilityBehaviour: typeof(AbductAbility), 
	            tex: Utils.GetTextureFromPath(TextureFile),
                id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
	            );
            AbductAbility.ability = newAbility.ability;
        }

        public override bool RespondsToResolveOnBoard()
        {
	        return Card.Slot.IsPlayerSlot;
        }
        
        public override IEnumerator OnResolveOnBoard()
        {
	        if (!CanAbduct())
	        {
		        Card.Anim.StrongNegationEffect();
		        yield return new WaitForSeconds(0.3f);
		        yield break;
	        }
	        
	        BoardManager boardManager = Singleton<BoardManager>.Instance;
	        Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(boardManager.ChoosingSlotViewMode, false);
	        
	        // Find who we want to abduct
	        IEnumerator onResolveOnBoard = ChooseTarget();
	        yield return onResolveOnBoard;
	        
	        // Make sure its a valid target
	        CardSlot cardSlot = m_targetedCardSlot;
	        if (cardSlot != null && cardSlot.Card != null)
	        {
		        // Pull them to the correct slot
		        PlayableCard playableCard = cardSlot.Card;
		        CardSlot oppositeCardSlot = GetOppositeCardSlot();
		        yield return PullTargetToSlot(playableCard, oppositeCardSlot);
	        }
	        else
	        {
		        Card.Anim.StrongNegationEffect();
		        yield return new WaitForSeconds(0.3f);
	        }

	        Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(boardManager.DefaultViewMode, false);
	        //Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
	        Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
	        Singleton<CombatPhaseManager>.Instance.VisualizeClearSniperAbility();
        }

        private IEnumerator ChooseTarget()
        {
	        CombatPhaseManager combatPhaseManager = Singleton<CombatPhaseManager>.Instance;
	        BoardManager boardManager = Singleton<BoardManager>.Instance;
	        List<CardSlot> opposingSlots = new List<CardSlot>(boardManager.GetSlots(Card.OpponentCard));

	        Action<CardSlot> callback1 = null;
	        Action<CardSlot> callback2 = null;
	        
	        combatPhaseManager.VisualizeStartSniperAbility(Card.slot);
	        
	        CardSlot cardSlot = Singleton<InteractionCursor>.Instance.CurrentInteractable as CardSlot;
	        if (cardSlot != null && opposingSlots.Contains(cardSlot))
	        {
		        combatPhaseManager.VisualizeAimSniperAbility(Card.slot, cardSlot);
	        }

	        List<CardSlot> allTargetSlots = opposingSlots;
	        List<CardSlot> validTargetSlots = opposingSlots;
	        List<CardSlot> slotsWithcards = allTargetSlots.FindAll(x => x.Card != null && !x.Card.Dead);

	        if (slotsWithcards.Count == 0)
	        {
		        Card.Anim.StrongNegationEffect();
		        yield return new WaitForSeconds(0.3f);
		        yield break;
	        }

	        m_targetedCardSlot = null;
	        Action<CardSlot> targetSelectedCallback;
	        if ((targetSelectedCallback = callback1) == null)
	        {
		        targetSelectedCallback = (callback1 = delegate(CardSlot s)
		        {
			        m_targetedCardSlot = s;
			        combatPhaseManager.VisualizeConfirmSniperAbility(s);
		        });
	        }
	        Action<CardSlot> invalidTargetCallback = null;
	        Action<CardSlot> slotCursorEnterCallback;
	        if ((slotCursorEnterCallback = callback2) == null)
	        {
		        slotCursorEnterCallback = (callback2 = delegate(CardSlot s)
		        {
			        combatPhaseManager.VisualizeAimSniperAbility(Card.slot, s);
		        });
	        }
	        yield return boardManager.ChooseTarget(allTargetSlots, validTargetSlots, targetSelectedCallback, invalidTargetCallback, slotCursorEnterCallback, () => false, CursorType.Target);
        }
        
        private IEnumerator PullTargetToSlot(PlayableCard otherCard, CardSlot slot)
        {
	        yield return base.PreSuccessfulTriggerSequence();
	        Vector3 a = (otherCard.Slot.transform.position + slot.transform.position) / 2f;
	        Tween.Position(otherCard.transform, a + Vector3.up * 0.5f, 0.05f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
	        yield return Singleton<BoardManager>.Instance.AssignCardToSlot(otherCard, slot, 0.05f, null, true);
	        yield return new WaitForSeconds(0.2f);
        }

        private CardSlot GetOppositeCardSlot()
        {
	        BoardManager boardManager = Singleton<BoardManager>.Instance;
	        List<CardSlot> list = new List<CardSlot>(boardManager.GetSlots(Card.OpponentCard));
	        for (int i = 0; i < list.Count; i++)
	        {
		        if (list[i].Index == Card.slot.Index)
		        {
			        return list[i];
		        }
	        }

	        return null;
        }

        private bool CanAbduct()
        {
	        CardSlot oppositeCardSlot = GetOppositeCardSlot();
	        if (oppositeCardSlot.Card != null)
	        {
		        return false;
	        }

	        BoardManager boardManager = Singleton<BoardManager>.Instance;
	        List<CardSlot> list = new List<CardSlot>(boardManager.GetSlots(Card.OpponentCard));
	        for (int i = 0; i < list.Count; i++)
	        {
		        if (list[i].Card != null)
		        {
			        // There is something to abduct
			        return true;
		        }
	        }

	        return false;
        }
    }
}