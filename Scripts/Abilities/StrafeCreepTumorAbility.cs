using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
	public class StrafeCreepTumorAbility : ACustomStrafe<StrafeCreepTumorAbility, StrafeCreepTumorData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
        public static void Initialize(Type declaringType)
        {
            ability = InitializeBase(declaringType);
        }

        public override IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
	        CardInfo squirrel = ScriptableObjectLoader<CardInfo>.AllData.Find((CardInfo info) => info.name == "CreepTumor");
	        yield return Singleton<BoardManager>.Instance.CreateCardInSlot(squirrel, oldSlot, 0.1f, true);
	        yield return new WaitForSeconds(0.25f);
	        oldSlot.Card.SetCardbackSubmerged();
	        oldSlot.Card.SetFaceDown(true, false);
        }
	}
}