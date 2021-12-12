using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class DetectorAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;
        
        private const int PowerLevel = 0;
        private const string SigilID = "Detector";
        private const string SigilName = "Detector";
        private const string Description = "When a card bearing this sigil is on the board all opponent's submerged cards will be revealed.";
        private const string TextureFile = "Artwork/Sigils/detector.png";
        private const string LearnText = "Revealing hidden cards will not help you defeat me";
        
        public override int Priority => this.triggerPriority;
        private int triggerPriority = int.MinValue + 1;

        private bool activated = false;
        
        public static void Initialize()
        {
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
                abilityBehaviour: typeof(DetectorAbility), 
                tex: Utils.GetTextureFromPath(TextureFile),
                id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
            );
            DetectorAbility.ability = newAbility.ability;
        }

        public override bool RespondsToResolveOnBoard()
        {
            return Singleton<TurnManager>.Instance.IsPlayerTurn && GetValidCardSlotsToFlip(false).Count > 0;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            activated = true;
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false);
            yield return new WaitForSeconds(0.15f);
            yield return base.PreSuccessfulTriggerSequence();

            List<CardSlot> opposingSlots = GetValidCardSlotsToFlip(false);
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                PlayableCard card = opposingSlots[i].Card;
                yield return base.LearnAbility(0f);
                        
                // Our targeted
                card.SetFaceDown(false, false);
                card.UpdateFaceUpOnBoardEffects();
                yield return new WaitForSeconds(0.3f);
            }
            
            this.triggerPriority = int.MinValue + 1;
            yield return new WaitForSeconds(0.3f); // Because its happening too fast maybe?
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return activated;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            activated = false;
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false);
            yield return new WaitForSeconds(0.15f);
            
            List<CardSlot> opposingSlots = GetValidCardSlotsToFlip(true);
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                PlayableCard card = opposingSlots[i].Card;
                yield return base.LearnAbility(0f);
                        
                // Our targeted
                card.SetCardbackSubmerged();
                card.SetFaceDown(true, false);
                yield return new WaitForSeconds(0.3f);
            }
            
            this.triggerPriority = int.MaxValue;
            yield return new WaitForSeconds(0.3f); // Because its happening too fast maybe?
        }

        private List<CardSlot> GetOpponentSlots(bool isPlayer)
        {
            return isPlayer ? BoardManager.Instance.opponentSlots : BoardManager.Instance.playerSlots;
        }

        private List<CardSlot> GetValidCardSlotsToFlip(bool facingDown)
        {
            List<CardSlot> slots = new List<CardSlot>();
            
            List<CardSlot> opposingSlots = GetOpponentSlots(Singleton<TurnManager>.Instance.IsPlayerTurn);
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                CardSlot slot = opposingSlots[i];
                if (slot.Card != null && !slot.Card.Dead)
                {
                    if (slot.Card.HasAbility(Ability.Submerge) || slot.Card.HasAbility(Ability.SubmergeSquid) && (slot.Card.FaceDown == facingDown))
                    {
                        slots.Add(slot);
                    }
                }
            }

            return slots;
        }
    }
}