using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Abilities
{
    public class SpawnLarvaAbility : CreateCardsAdjacent
	{
		public override Ability Ability => ability;
		public static Ability ability;
        
		private const int PowerLevel = 0;
		private const string SigilID = "Spawn Larva";
		private const string SigilName = "Spawn Larva";
		private const string Description = "When placed on the board will create larva on both sides";
		private const string TextureFile = "Artwork/Sigils/spawn_larva.png";
		private const string LearnText = "Extra Larva? Trying to rush me i see";

		public override string SpawnedCardId => "Squirrel";
		public override string CannotSpawnDialogue => "No room for Larva!";
        
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
				abilityBehaviour: typeof(SpawnLarvaAbility), 
				tex: Utils.GetTextureFromPath(TextureFile),
				id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
			);
			SpawnLarvaAbility.ability = newAbility.ability;
		}
	}
}