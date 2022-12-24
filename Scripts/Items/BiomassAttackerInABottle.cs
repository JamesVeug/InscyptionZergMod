using DiskCardGame;
using InscryptionAPI.Items;
using UnityEngine;

namespace ZergMod.Scripts.Items
{
    public static class BiomassAttackerInABottle
    {
        public static ConsumableItemData Data;

        public static void Initialize()
        {
            Texture2D texture = Utils.GetTextureFromPath("Artwork/Items/biomassbottle.png");
            Data = ConsumableItemManager.NewCardInABottle(Plugin.PluginGuid, "Zerg_JSON_biomassAttacker", texture);
        }
    }
}