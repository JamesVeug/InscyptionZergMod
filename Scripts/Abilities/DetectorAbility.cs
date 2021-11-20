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

            NewAbility newAbility = new NewAbility(info,typeof(HealAbility),tex,AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            HealAbility.ability = newAbility.ability;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            bool cardOwnedByPlayer = base.Card.OpponentCard == playerUpkeep; 
            if(!cardOwnedByPlayer)
            {
                // Only do something if its not their turn
                // Submerge flips all cards owned by whose turn it is
                return false;
            }

            List<CardSlot> opposingSlots = base.Card.GetOpposingSlots();
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                CardSlot slot = opposingSlots[i];
                if (slot.Card != null)
                {
                    if (slot.Card.HasAbility(Ability.Submerge) || slot.Card.HasAbility(Ability.SubmergeSquid))
                    {
                        // Our targeted
                        return true;
                    }
                }
            }

            return false;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            activated = true;
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.15f);
            yield return base.PreSuccessfulTriggerSequence();
            
            List<CardSlot> opposingSlots = base.Card.GetOpposingSlots();
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                CardSlot slot = opposingSlots[i];
                if (slot.Card != null && !slot.Card.Dead)
                {
                    if (slot.Card.HasAbility(Ability.Submerge) || slot.Card.HasAbility(Ability.SubmergeSquid))
                    {
                        // Our targeted
                        base.Card.SetFaceDown(false, false);
                        base.Card.UpdateFaceUpOnBoardEffects();
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
            
            yield return base.LearnAbility(0f);
            this.triggerPriority = int.MinValue + 1;
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            // Only do something if the card that played was owned by us
            bool cardOwnedByPlayer = base.Card.OpponentCard == playerTurnEnd; 
            return cardOwnedByPlayer && !base.Card.Dead;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            activated = false;
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.15f);
            
            List<CardSlot> opposingSlots = base.Card.GetOpposingSlots();
            for (int i = 0; i < opposingSlots.Count; i++)
            {
                CardSlot slot = opposingSlots[i];
                if (slot.Card != null && !slot.Card.Dead)
                {
                    if (slot.Card.HasAbility(Ability.Submerge) || slot.Card.HasAbility(Ability.SubmergeSquid))
                    {
                        // Our targeted
                        base.Card.SetCardbackSubmerged();
                        base.Card.SetFaceDown(true, false);
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
            
            yield return new WaitForSeconds(0.3f);
            this.triggerPriority = int.MaxValue;
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
    }
}