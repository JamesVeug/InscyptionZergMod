using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
	/// <summary>
	/// Credits to Cyantist for ThickShell ability from SigilADay
	/// </summary>
	public class ArmouredAbility : AbilityBehaviour
	{
		public override Ability Ability => ability;
		public static Ability ability;

		public static void Initialize()
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "Armoured";
			info.rulebookDescription = "When a card bearing this sigil takes damage, it will take 1 less damage";
			info.metaCategories = new List<AbilityMetaCategory>
				{ AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

			byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/armoured.png"));
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);

			AbilityIdentifier identifier = AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName);
			NewAbility newAbility = new NewAbility(info, typeof(ArmouredAbility), tex,identifier);
			ArmouredAbility.ability = newAbility.ability;
		}

		private void Start()
		{
			this.mod = new CardModificationInfo();
			this.mod.nonCopyable = true;
			this.mod.singletonId = "ArmouredHP";
			this.mod.healthAdjustment = 0;
			base.Card.AddTemporaryMod(this.mod);
		}

		public override bool RespondsToCardGettingAttacked(PlayableCard source)
		{
			return source == base.Card;
		}

		public override bool RespondsToAttackEnded()
		{
			return this.attacked;
		}

		public override IEnumerator OnCardGettingAttacked(PlayableCard source)
		{
			this.attacked = true;
			yield return base.PreSuccessfulTriggerSequence();
			this.mod.healthAdjustment = 1;
			yield break;
		}

		public override IEnumerator OnAttackEnded()
		{
			this.attacked = false;
			yield return new WaitForSeconds(0.1f);
			this.mod.healthAdjustment = 0;
			base.Card.HealDamage(1);
			base.Card.Anim.LightNegationEffect();
			yield return new WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.25f);
			yield break;
		}

		private bool attacked;
		private CardModificationInfo mod;
	}
}