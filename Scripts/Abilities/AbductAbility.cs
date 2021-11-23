using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace ZergMod
{
    public class AbductAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        private CardSlot m_targetedCardSlot = null;

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Abduct";
            info.rulebookDescription = "When a card bearing this sigil is played, a targeted enemy card is moved to the space in front of it, if that space is empty";
            info.metaCategories = new List<AbilityMetaCategory>
                { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/abduct.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info, typeof(AbductAbility), tex,
                AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            AbductAbility.ability = newAbility.ability;
        }

        public override bool RespondsToResolveOnBoard()
        {
	        if (!Card.Slot.IsPlayerSlot)
		        return false;

	        CardSlot oppositeCardSlot = GetOppositeCardSlot();
	        if (oppositeCardSlot.Card != null)
	        {
		        return false;
	        }

	        return true;
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
        
        public override IEnumerator OnResolveOnBoard()
        {
	        Plugin.Log.LogInfo("[AbductAbility][OnResolveOnBoard] Starting");
	        
	        CardSlot oppositeCardSlot = GetOppositeCardSlot();
	        if (oppositeCardSlot.Card != null)
	        {
		        Plugin.Log.LogWarning("[AbductAbility][OnResolveOnBoard] Card in front of Viper full");
		        Card.Anim.StrongNegationEffect();
		        yield return new WaitForSeconds(0.3f);
		        yield break;
	        }
	        
	        BoardManager boardManager = Singleton<BoardManager>.Instance;
	        Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(boardManager.ChoosingSlotViewMode, false);
	        //Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
	        
	        // Find who we want to abduct
	        IEnumerator onResolveOnBoard = ChooseTarget();
	        yield return onResolveOnBoard;
	        
	        // Make sure its a valid target
	        CardSlot cardSlot = m_targetedCardSlot;
	        if (cardSlot != null && cardSlot.Card != null)
	        {
		        // Pull them to the correct slot
		        PlayableCard playableCard = cardSlot.Card;

		        yield return PullTargetToSlot(playableCard, oppositeCardSlot);
	        }

	        Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(boardManager.DefaultViewMode, false);
	        //Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
	        Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
	        Singleton<CombatPhaseManager>.Instance.VisualizeClearSniperAbility();
	        
	        Plugin.Log.LogInfo("[AbductAbility][OnResolveOnBoard] Done");
	        yield break;
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
	        List<CardSlot> validTargetSlots = allTargetSlots.FindAll(x => x.Card != null && !x.Card.Dead);

	        foreach (CardSlot slot in validTargetSlots)
	        {
		        string thing = slot != null && slot.Card != null ? slot.Card.Info.displayedName : "empty";
		        Plugin.Log.LogInfo("[AbductAbility][ChooseTarget] Slot: " + thing);
	        }
	        
	        if (validTargetSlots.Count == 0)
	        {
		        Plugin.Log.LogWarning("[AbductAbility][ChooseTarget] No cards to pull");
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
			        Plugin.Log.LogWarning("[AbductAbility][ChooseTarget] Clicked on: " + s.Card.Info.displayedName);
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
    }
}