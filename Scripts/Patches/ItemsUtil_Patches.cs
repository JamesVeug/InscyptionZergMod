using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Helpers;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof(ItemsUtil), "AllConsumables", MethodType.Getter)]
    public class ItemsUtil_AllConsumables
    {
        public static ConsumableItemData BiomassInABottle = null;
        
        public static void Postfix(CardInfo __instance, ref List<ConsumableItemData> __result)
        {
            for (var i = 0; i < __result.Count; i++)
            {
                ConsumableItemData data = __result[i];
                if (data.name == "SquirrelBottle")
                {
                    ConsumableItemData BiomassInABottle = GameObject.Instantiate(data, null);
                    BiomassInABottle.name = "BiomassInABottle";
                    BiomassInABottle.description = "OH look. A Biomass in a Bottle";
                    BiomassInABottle.rulebookName = "Biomass in a Bottle";
                    BiomassInABottle.rulebookDescription = "A Biomass is created in your hand. Biomass is defined as 0 Power 1 Health with Morsel.";

                    Sprite texture = ZergMod.Utils.GetTextureFromPath("Artwork/Items/biomassbottle.png").ConvertTexture();
                    BiomassInABottle.rulebookSprite = texture;
                    __result.Add(BiomassInABottle);
                    break;
                }
            }
        }
    }
}