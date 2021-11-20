using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class Draw2LocustsAbility : AbilityBehaviour
	{
		public override Ability Ability => ability;
		public static Ability ability;

		public static void Initialize()
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "Draw Locusts";
			info.rulebookDescription = "Draw 2 Locusts at the end of the round\nA Locust is defined as: 1 Power 1 Health";
			info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

			List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
			DialogueEvent.Line line = new DialogueEvent.Line();
			line.text = "Although brittle they are dangerous in high numbers";
			lines.Add(line);
			info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

			byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/draw_locusts.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);

			NewAbility newAbility = new NewAbility(info,typeof(Draw2LocustsAbility),tex,AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
			Draw2LocustsAbility.ability = newAbility.ability;
		}

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return !Card.Dead && playerTurnEnd && Card.slot.IsPlayerSlot;
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return PreSuccessfulTriggerSequence();
			yield return CreateCards();
			yield return LearnAbility(0f);
		}

		private IEnumerator CreateCards()
		{
			if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
				yield return new WaitForSeconds(0.2f);
			}

			CardInfo cardByName = CardLoader.GetCardByName("Locust");
			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.25f, null);
			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.125f, null);
			yield return new WaitForSeconds(0.45f);
			yield return base.LearnAbility(0.1f);
			yield break;
		}
	}
}