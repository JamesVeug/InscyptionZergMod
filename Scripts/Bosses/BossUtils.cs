using System.Collections;
using DiskCardGame;
using InscryptionAPI.Dialogue;
using UnityEngine;

namespace ZergMod.Scripts.Bosses
{
	public static class BossUtils
	{
		public static string AmonColorCode;

		public static void Initialize()
		{
			AmonColorCode = DialogueManager.AddColor(Plugin.PluginGuid, "amon", Color.red).ColorCode;
		}
		
		public static IEnumerator AbathurMessage(string message, 
			float effectFOVOffset = -2.5f,
			float effectEyelidIntensity = 0.5f,
			Emotion emotion = Emotion.Neutral,
			TextDisplayer.LetterAnimation letterAnimation = TextDisplayer.LetterAnimation.Jitter,
			DialogueEvent.Speaker speaker = DialogueEvent.Speaker.Single,
			string[] variableStrings = null,
			bool obeyTimescale = true)
		{
			yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(
				$"[c:{AmonColorCode}]{message}[c:]",
				effectFOVOffset, effectEyelidIntensity, emotion, letterAnimation, speaker, variableStrings, obeyTimescale);
		}
	}
}