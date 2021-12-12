using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class RegestateAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;
        
        private const int PowerLevel = 0;
        private const string SigilID = "Regestate";
        private const string SigilName = "Regestate";
        private const string Description = "When this card is killed it will transform into an Egg for it to regenerate into its original form.\nAn egg has 0 Power and as much Health as the base card.";
        private const string TextureFile = "Artwork/Cards/egg.png";
        private const string LearnText = "That card seems unbalanced";

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
                abilityBehaviour: typeof(RegestateAbility), 
                tex: Utils.GetTextureFromPath(TextureFile),
                id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
            );
            RegestateAbility.ability = newAbility.ability;
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            if (wasSacrifice) 
                return false;
            
            bool isADeathCard = Card.Info.mods.Find((x)=>x.deathCardInfo != null) != null;
            if (isADeathCard)
                return false;
            
            return true;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            
            CardInfo whatToMutateInto = CardLoader.GetCardByName(this.Card.Info.name);
            int totalHealth = this.Card.Health + this.Card.Status.damageTaken;
            int totalEvolves = Mathf.Clamp(Mathf.FloorToInt((float)totalHealth / 4), 1, 3) + 1;
            
            CardInfo egg = (CardInfo)NewCard.cards.Find(info => info.displayedName == "Egg").Clone();
            egg.baseHealth = totalHealth;
            egg.abilities = new List<Ability> { Ability.Evolve };
            egg.evolveParams = new EvolveParams{turnsToEvolve = totalEvolves, evolution = whatToMutateInto};
            
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(egg, Card.slot, 0.15f, true);

            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}