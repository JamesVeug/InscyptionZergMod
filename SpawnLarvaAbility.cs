using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace CardLoaderMod
{
    public class SpawnLarvaAbility : CreateCardsAdjacent
	{
		public override Ability Ability => ability;
		public static Ability ability;

		public override string SpawnedCardId => "Squirrel";
		public override string CannotSpawnDialogue => "No room for Larva!";
	}
}