using System;
using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using StarCraftCore.Scripts.Abilities;
using StarCraftCore.Scripts.Data.Sigils;
using UnityEngine;

namespace ZergMod.Scripts.Abilities
{
    public class DevourAbility : ACustomAbilityBehaviour<DevourAbility, AbilityData>
	{
		public override Ability Ability => ability;
		public static Ability ability = Ability.None;
		
		public static void Initialize(Type declaringType)
		{
			ability = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
		}
		
		public override bool RespondsToResolveOnBoard()
		{
			PlayableCard target = Card.slot.opposingSlot.Card;
			return !Card.Dead && Card.slot.IsPlayerSlot && target != null && !target.Dead && !target.HasAbility(Ability.MadeOfStone);
		}

		public override IEnumerator OnResolveOnBoard()
		{
			yield return PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.25f);

			int currentRandomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
			int attackBonus = SeededRandom.Range(0, 100, currentRandomSeed++) <= 50 ? 1 : 0;
			int healthBonus = SeededRandom.Range(0, 100, currentRandomSeed++) <= 50 ? 2 : 1;
			
			CardModificationInfo mod = new CardModificationInfo();
			mod.attackAdjustment = Mathf.Clamp(Card.slot.opposingSlot.Card.Attack, 0, attackBonus); 
			mod.healthAdjustment = Mathf.Clamp(Card.slot.opposingSlot.Card.Health, 0, healthBonus); 

			bool impactFrameReached = false;
			base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), Card.slot.opposingSlot, delegate()
			{
				impactFrameReached = true;
			});
			yield return new WaitUntil(() => impactFrameReached);
			this.Card.AddTemporaryMod(mod);
			yield return Card.slot.opposingSlot.Card.Die(false, Card, true);
			
			
			yield return new WaitForSeconds(0.25f);
			yield return LearnAbility(0f);
		}
	}
}