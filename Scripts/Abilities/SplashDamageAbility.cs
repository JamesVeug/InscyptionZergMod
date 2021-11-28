using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class SplashDamageAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;
        
        private const int PowerLevel = 0;
        private const string SigilID = "Splash Damage";
        private const string SigilName = "Splash Damage";
        private const string Description = "When a card bearing this sigil deals damage it will also hit the adjacent cards";
        private const string TextureFile = "Artwork/splash_damage.png";
        private const string LearnText = "";

        private bool activated = false;
        
        public override int Priority => int.MaxValue;

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
                abilityBehaviour: typeof(SplashDamageAbility), 
                tex: Utils.GetTextureFromPath(TextureFile),
                id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
            );
            SplashDamageAbility.ability = newAbility.ability;
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            CardSlot slot = Utils.GetSlot(target);
            return !Card.Dead && !activated && slot != null;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return this.PreSuccessfulTriggerSequence();

            activated = true;
            
            CardSlot cardSlot = Utils.GetSlot(target);
            List<CardSlot> adjacentSlots = Utils.GetAdjacentSlots(cardSlot);
            foreach (CardSlot slot in adjacentSlots)
            {
                if (slot.Card != null && !slot.Card.Dead && !slot.Card.FaceDown)
                {
                    yield return slot.Card.TakeDamage(amount, Card);
                }
            }

            activated = false;

            yield return base.LearnAbility(0.0f);
        }
    }
}