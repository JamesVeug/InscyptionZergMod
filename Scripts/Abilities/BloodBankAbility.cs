using System;
using DiskCardGame;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public class BloodBankAbility : ACustomAbilityBehaviour<AbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(declaringType);
		}
	}
}