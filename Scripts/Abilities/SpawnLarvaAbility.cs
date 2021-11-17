using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace CardLoaderMod
{
    public class SpawnLarvaAbility : CreateCardsAdjacent
	{
		public override Ability Ability => ability;
		public static Ability ability;

		public override string SpawnedCardId => "Squirrel";
		public override string CannotSpawnDialogue => "No room for Larva!";
        
		public static void Initialize()
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "Spawn Larva";
			info.rulebookDescription = "When placed on the board will create larva on both sides";
			info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

			List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
			DialogueEvent.Line line = new DialogueEvent.Line();
			line.text = "Extra Larva? Trying to rush me i see";
			lines.Add(line);
			info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

			byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/spawn_larva.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);

			NewAbility newAbility = new NewAbility(info,typeof(SpawnLarvaAbility),tex,AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
			SpawnLarvaAbility.ability = newAbility.ability;
		}
	}
}