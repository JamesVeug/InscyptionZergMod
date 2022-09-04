using System;
using DiskCardGame;
using StarCraftCore.Scripts.Abilities;
using StarCraftCore.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public class SpawnLarvaAbility : ACustomCreateCardsAdjacent<SpawnLarvaAbility, CreateCardsAbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
		}
	}
}