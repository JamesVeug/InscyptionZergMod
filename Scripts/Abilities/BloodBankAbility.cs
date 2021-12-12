using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class BloodBankAbility : AbilityBehaviour
	{
		public override Ability Ability => ability;
		public static Ability ability;
        
		private const int PowerLevel = 0;
		private const string SigilID = "Blood Bank";
		private const string SigilName = "Blood Bank";
		private const string Description = "When a card bearing this sigil is sacrified, its health is reduced relative to blood required.";
		private const string TextureFile = "Artwork/Cards/drone.png";
		private const string LearnText = "That's one gigantic card capable of multiple sacrifices in replacement of health";

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
				abilityBehaviour: typeof(BloodBankAbility), 
				tex: Utils.GetTextureFromPath(TextureFile),
				id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
			);
			BloodBankAbility.ability = newAbility.ability;
		}
	}
}