using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class ExplodeAbility : SplashDamageAbility
	{
		public override Ability Ability => ability;
		public new static Ability ability;

		private bool m_startedAttack = false;
        
		public new static void Initialize()
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "Explode";
			info.rulebookDescription = "When a card bearing this sigil deals damage it will also hit the adjacent cards and perish.";
			info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

			byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/explode.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);

			NewAbility newAbility = new NewAbility(info,typeof(ExplodeAbility),tex,AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
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