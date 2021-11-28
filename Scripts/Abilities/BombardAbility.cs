using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class BombardAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;
        
        private const int PowerLevel = 0;
        private const string SigilID = "Bombard";
        private const string SigilName = "Bombard";
        private const string Description = "When a card bearing this sigil deals damage directly it will damage the opposing card for 1 damage.";
        private const string TextureFile = "Artwork/bombard.png";
        private const string LearnText = "Bombarding my side of the board will only weaken that card";

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = PowerLevel;
            info.rulebookName = SigilName;
            info.rulebookDescription = Description;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

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
                abilityBehaviour: typeof(BombardAbility), 
                tex: Utils.GetTextureFromPath(TextureFile),
                id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
            );
            BombardAbility.ability = newAbility.ability;
        }

        public override bool RespondsToDealDamageDirectly(int amount)
        {
            PlayableCard opposingSlotCard = Card.slot.opposingSlot.Card;
            return opposingSlotCard != null && !opposingSlotCard.Dead && !opposingSlotCard.FaceDown;
        }

        public override IEnumerator OnDealDamageDirectly(int amount)
        {
            PlayableCard opposingSlotCard = Card.slot.opposingSlot.Card;
            
            
            yield return new WaitForSeconds(0.1f);
            yield return opposingSlotCard.TakeDamage(1, this.Card);
            yield return new WaitForSeconds(0.1f);
            
            yield return base.LearnAbility(0f);
        }
    }
}