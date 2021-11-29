using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class ExplodeAbility : SplashDamageAbility
	{
		public override Ability Ability => ability;
		public new static Ability ability;
        
		private const int PowerLevel = 0;
		private const string SigilID = "Explode";
		private const string SigilName = "Explode";
		private const string Description = "When a card bearing this sigil deals damage it will also hit the opponents adjacent cards and perish.";
		private const string TextureFile = "Artwork/explode.png";
		private const string LearnText = "";

		private bool m_startedAttack = false;
        
		public new static void Initialize()
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
				abilityBehaviour: typeof(ExplodeAbility), 
				tex: Utils.GetTextureFromPath(TextureFile),
				id: AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, SigilID)
			);
			ExplodeAbility.ability = newAbility.ability;
		}

		public override IEnumerator OnDealDamage(int amount, PlayableCard target)
		{
			m_startedAttack = true;
			yield return base.OnDealDamage(amount, target);
		}

		public override bool RespondsToDealDamageDirectly(int amount)
		{
			return true;
		}

		public override IEnumerator OnDealDamageDirectly(int amount)
		{
			m_startedAttack = true;
			yield return base.OnDealDamageDirectly(amount);
		}

		public override bool RespondsToAttackEnded()
		{
			return m_startedAttack;
		}

		public override IEnumerator OnAttackEnded()
		{
			yield return base.PreSuccessfulTriggerSequence();
			if (base.Card != null && !base.Card.Dead)
			{
				yield return base.Card.Die(false, null, true);
				yield return base.LearnAbility(0.25f);
			}
			yield break;
		}
	}
}