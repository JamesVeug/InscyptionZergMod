using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class RegestateAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Regestate";
            info.rulebookDescription = "When this card is killed it will transform into an Egg for it to regenerate into its original form";
            info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

            List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
            DialogueEvent.Line line = new DialogueEvent.Line();
            line.text = "That card seems unbalanced";
            lines.Add(line);
            info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,"Artwork/egg.png"));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info,typeof(RegestateAbility),tex,AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            RegestateAbility.ability = newAbility.ability;
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

            CardInfo cardByName = (CardInfo)NewCard.cards.Find(info => info.displayedName == "Egg").Clone();
            cardByName.evolveParams = new EvolveParams{turnsToEvolve = 2, evolution = (CardInfo)Card.Info.Clone()};
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, Card.slot, 0.15f, true);

            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}