using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using InscryptionAPI.Regions;
using Pixelplacement;
using UnityEngine;
using ZergMod;
using ZergMod.Scripts.Bosses;
using ZergMod.Scripts.Encounters;
using ZergMod.Scripts.Masks;
using Random = UnityEngine.Random;

namespace DiskCardGame.Zerg
{
	public class AbathurOpponent : Part1BossOpponent
	{
		public static Opponent.Type ID = Type.Default;
		
		public override string DefeatedPlayerDialogue => "Biomass extraction commencing";

		public static void Initialize()
		{
			AbathurEncounter.Initialize();
			AbathurEncounter2.Initialize();
			
			OpponentManager.FullOpponent opponent = OpponentManager.Add(Plugin.PluginGuid, "Abathur", 
				AbathurBattleSequencer.ID,
				typeof(AbathurOpponent),
				new List<Texture2D>()
				{
					Utils.GetTextureFromPath("Artwork/Opponents/animated_bossnode_abathar_1.png"),
					Utils.GetTextureFromPath("Artwork/Opponents/animated_bossnode_abathar_2.png"),
					Utils.GetTextureFromPath("Artwork/Opponents/animated_bossnode_abathar_3.png"),
					Utils.GetTextureFromPath("Artwork/Opponents/animated_bossnode_abathar_4.png"),
					Utils.GetTextureFromPath("Artwork/Opponents/animated_bossnode_abathar_5.png"),
				});
			ID = opponent.Id;
		}
		
		public override IEnumerator IntroSequence(EncounterData encounterData)
		{
			AudioController.Instance.FadeOutLoop(0.75f, Array.Empty<int>());
			yield return base.IntroSequence(encounterData);
			
			yield return new WaitForSeconds(0.75f);
			Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
			yield return new WaitForSeconds(0.25f);
			yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("A large slug-like creature slithers towards you!", -2.5f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter);
			yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("It stands up straight and towers over you", -2.5f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter);
			yield return new WaitForSeconds(0.15f);
			LeshyAnimationController.Instance.PutOnMask(AbatharMask.ID, false);
			AudioController.Instance.SetLoopAndPlay("part3_damagerace", 0, true, true);
			AudioController.Instance.SetLoopAndPlay("part3_ambience_undead", 1, true, true);
			AudioController.Instance.SetLoopVolume(0.25f, 4f, 1, true);
			yield return new WaitForSeconds(1.5f);
			Texture eyeTexture = Utils.GetTextureFromPath(Path.Combine(Plugin.Directory, "Artwork/Opponents/LeshyEye_RedSnake.png"));
			LeshyAnimationController.Instance.SetEyesTexture(eyeTexture);
			Singleton<OpponentAnimationController>.Instance.SetHeadTilt(5f, 1.5f, 0.1f);
			yield return base.FaceZoomSequence();
			yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("It's the Evolution Master!", -2.5f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter);
			yield return BossUtils.AbathurMessage("New hostile specimen, must experiment");
			yield return BossUtils.AbathurMessage("Biomass extraction required");
			//yield return new WaitForSeconds(1.5f);
			
			Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield return new WaitForSeconds(0.25f);
			//base.SpawnScenery("HookedFishTableEffects");
			//yield return new WaitForSeconds(0.5f);
		}
		
		public override void SetSceneEffectsShown(bool showEffects)
		{
			if (showEffects)
			{
				LeshyAnimationController.Instance.SetHairColor(Color.black);
				Tween.Rotate(Singleton<ExplorableAreaManager>.Instance.HangingLight.transform, new Vector3(150f, 0f, 0f), Space.World, 25f, 0f, Tween.EaseInOut, Tween.LoopType.PingPong, null, null, true);
				Singleton<ExplorableAreaManager>.Instance.SetHangingLightIntensity(10f);
				Singleton<ExplorableAreaManager>.Instance.SetHangingLightCookie(ResourceBank.Get<Texture>("Art/Effects/WavesTextureCube"));
				Color color = GameColors.Instance.darkLimeGreen;
				color.a = 0.5f;
				Singleton<TableVisualEffectsManager>.Instance.ChangeTableColors(color, GameColors.Instance.marigold, this.InteractablesGlowColor, color, color, this.InteractablesGlowColor, GameColors.Instance.gray, GameColors.Instance.gray, GameColors.Instance.lightGray);
				return;
			}
			LeshyAnimationController.Instance.ResetHairColor();
			Tween.Cancel(Singleton<ExplorableAreaManager>.Instance.HangingLight.transform.GetInstanceID());
			Singleton<ExplorableAreaManager>.Instance.ResetHangingLightIntensity();
			Singleton<ExplorableAreaManager>.Instance.ClearHangingLightCookie();
			Singleton<TableVisualEffectsManager>.Instance.ResetTableColors();
			Singleton<OpponentAnimationController>.Instance.ResetHeadTilt(0.2f);
			LeshyAnimationController.Instance.ResetEyesTexture();
		}

		public override IEnumerator StartNewPhaseSequence()
		{
			yield return base.StartNewPhaseSequence();
			
			base.TurnPlan.Clear();
			yield return this.EvolveSequence();
			
			yield return Phase2Blueprint();
		}

		private IEnumerator Phase2Blueprint()
		{
			this.Blueprint = AbathurEncounter2.Instance;
			int difficulty = 0;
			if (Singleton<TurnManager>.Instance.BattleNodeData != null)
			{
				difficulty = Singleton<TurnManager>.Instance.BattleNodeData.difficulty + RunState.Run.DifficultyModifier;
			}
			List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, difficulty, true);
			this.ReplaceAndAppendTurnPlan(plan);
			yield return this.QueueNewCards(true, true);
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x00065B53 File Offset: 0x00063D53
		private IEnumerator EvolveSequence()
		{
			Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
			yield return new WaitForSeconds(0.4f);
			
			yield return BossUtils.AbathurMessage("Experiment successful");
			yield return BossUtils.AbathurMessage("Must acquire essence for the Dark One");
			yield return new WaitForSeconds(0.5f);
			yield return base.ClearQueue();
			yield return new WaitForSeconds(0.5f);

			// Evolve everything on Abathur's side
			List<CardSlot> slotsToBeAttacked = new List<CardSlot>();
			foreach (CardSlot slot in Singleton<BoardManager>.Instance.PlayerSlotsCopy)
			{
				if (slot.Card != null)
				{
					slotsToBeAttacked.AddRange(slot.Card.GetOpposingSlots());
				}
			}
			
			CardInfo eggInfo = CardLoader.GetCardByName("Zerg_JSON_AbathurEgg");
			foreach (CardSlot slot in Singleton<BoardManager>.Instance.OpponentSlotsCopy)
			{
				if (slot.Card == null)
				{
					// Put an egg in an empty slot
					if (slotsToBeAttacked.Contains(slot))
					{
						yield return Singleton<BoardManager>.Instance.CreateCardInSlot(eggInfo, slot, 0.1f, true);
					}
					else
					{
						continue;
					}
				}
				else if (slot.Card.Info.name == "Zerg_JSON_AbathurEgg")
				{
					// turn egg into zergling
					/*slot.Card.ExitBoard(0.4f, Vector3.zero);
					yield return new WaitForSeconds(0.1f);
					yield return Singleton<BoardManager>.Instance.CreateCardInSlot(zerglings, slot, 0.1f, true);*/
				}
				else
				{
					// evolve card
					CardInfo evolution = this.GetTransformCardInfo(slot.Card.Info);
					yield return slot.Card.TransformIntoCard(evolution, null, null);
				}
				yield return new WaitForSeconds(0.5f);
			}
		}
		
		protected virtual CardInfo GetTransformCardInfo(CardInfo cardInfo)
		{
			if (cardInfo.evolveParams == null)
			{
				return EvolveParams.GetDefaultEvolution(cardInfo);
			}
			return cardInfo.evolveParams.evolution.Clone() as CardInfo;
		}
	}
}