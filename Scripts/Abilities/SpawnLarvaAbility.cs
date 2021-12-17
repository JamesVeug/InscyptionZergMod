using System;
using DiskCardGame;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public class SpawnLarvaAbility : ACustomCreateCardsAdjacent<CreateCardsAbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(declaringType);
		}
	}
}