using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class DetectorAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;
        
        
        public override int Priority => this.triggerPriority;
        private int triggerPriority = int.MinValue + 1;

        private bool activated = false;
        
        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Detector";
            info.rulebookDescription = "The card bearing this sigil will reveal submerged cards on the board at the beginning of your turn";
            info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "Grr you manged to foil my plan!";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/detector.png"));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info,typeof(DetectorAbility),tex,AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            DetectorAbility.ability = newAbility.ability;
        }

        public override bool RespondsToResolveOnBoard()
        {
            return Singleton<TurnManager>.Instance.IsPlayerTurn && GetValidCardSlotsToFlip(false).Count > 0;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            // WHY DOES THIS BRRRRRRREEEEEEEEEAAAAAAAAAAAAAK
            Plugin.Log.LogInfo("[DetectorAbility] ========== OnResolveOnBoard: player: " + Singleton<TurnManager>.Instance.IsPlayerTurn + " ==========");
            yield return OnUpkeep(Singleton<TurnManager>.Instance.IsPlayerTurn);
            Plugin.Log.LogInfo("[DetectorAbility] ========== OnResolveOnBoard done");
            yield break;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            Plugin.Log.LogInfo("[DetectorAbility] ========== RespondsToUpkeep: player: " + playerUpkeep + " ==========");
            return playerUpkeep && GetValidCardSlotsToFlip(false).Count > 0;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            Plugin.Log.LogInfo("[DetectorAbility] ========== OnUpkeep: player: " + playerUpkeep + " ==========");
            activated = true;
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.15f);
            yield return base.PreSuccessfulTriggerSequence();

            List<CardSlot> opposingSlots = GetValidCardSlotsToFlip(false);
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                PlayableCard card = opposingSlots[i].Card;
                Plugin.Log.LogInfo("[DetectorAbility] Flipping up " + card.Info.displayedName);
                yield return base.LearnAbility(0f);
                        
                // Our targeted
                card.SetFaceDown(false, false);
                card.UpdateFaceUpOnBoardEffects();
                yield return new WaitForSeconds(0.3f);
            }
            
            this.triggerPriority = int.MinValue + 1;
            yield return new WaitForSeconds(0.3f); // Because its happening too fast maybe?
            Plugin.Log.LogInfo("[DetectorAbility] ========== OnUpkeep done ==========");
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            // Only do something if the card that played was owned by us 
            return playerTurnEnd && GetValidCardSlotsToFlip(true).Count > 0;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Plugin.Log.LogInfo("[DetectorAbility] ========== OnTurnEnd: player: " + playerTurnEnd + " ==========");
            activated = false;
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.15f);
            
            List<CardSlot> opposingSlots = GetValidCardSlotsToFlip(true);
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                PlayableCard card = opposingSlots[i].Card;
                Plugin.Log.LogInfo("[DetectorAbility] Flipping down " + card.Info.displayedName);
                yield return base.LearnAbility(0f);
                        
                // Our targeted
                card.SetCardbackSubmerged();
                card.SetFaceDown(true, false);
                yield return new WaitForSeconds(0.3f);
            }
            
            this.triggerPriority = int.MaxValue;
            yield return new WaitForSeconds(0.3f); // Because its happening too fast maybe?
            Plugin.Log.LogInfo("[DetectorAbility] ========== OnTurnEnd done ==========");
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice && activated;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            // TODO: Card killed while enemy is playing
            // TODO: card is killed while player is playing
            yield return OnTurnEnd(false);
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