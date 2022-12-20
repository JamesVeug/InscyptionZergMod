using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using ZergMod.Scripts.Traits;

namespace ZergMod.Scripts.VariableStatBehaviours
{
	public class ZerglingVariableStat : VariableStatBehaviour
	{
		private static StatIconManager.FullStatIcon FullStatIcon;

		public override SpecialStatIcon IconType => FullStatIcon.Id;
		
		public static void Initialize()
		{
			StatIconInfo val = ScriptableObject.CreateInstance<StatIconInfo>();
			val.metaCategories = new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook };
			val.appliesToAttack = true;
			val.appliesToHealth = false;
			val.rulebookName = "Zergling Rush";
			val.rulebookDescription = "The value represented with this sigil will be equal to the number of Zerglings that the owner has on their side of the table.";
			val.iconGraphic = Utils.GetTextureFromPath("Artwork/StatIcons/zergling_icon.png");
			FullStatIcon = StatIconManager.Add(Plugin.PluginGuid, val, typeof(ZerglingVariableStat));
		}

		public override int[] GetStatValues()
		{
			List<CardSlot> list = base.PlayableCard.Slot.IsPlayerSlot ? Singleton<BoardManager>.Instance.PlayerSlotsCopy : Singleton<BoardManager>.Instance.OpponentSlotsCopy;
			int num = 0;
			foreach (CardSlot cardSlot in list)
			{
				if (cardSlot.Card != null && cardSlot.Card.Info.HasTrait(ZerglingTrait.ID))
				{
					num++;
				}
			}
			int[] array = new int[2];
			array[0] = num;
			return array;
		}
	}
}