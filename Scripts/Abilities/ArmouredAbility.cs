using System.Collections;
using System.Collections.Generic;
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
        
		private const int PowerLevel = 0;
		private const string SigilID = "Armoured";
		private const string SigilName = "Armoured";
		private const string Description = "When a card bearing this sigil takes damage, it will take 1 less damage";
		private const string TextureFile = "Artwork/armoured.png";
		private const string LearnText = "";

		public static void Initialize()
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = PowerLevel;
			info.rulebookName = SigilName;
			info.rulebookDescription = Description;
			info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

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
				abilityBehaviour: typeof(ArmouredAbility), 
				tex: Utils.GetTextureFromPath(TextureFile),
				id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
			);
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