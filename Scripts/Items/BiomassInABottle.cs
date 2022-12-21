using DiskCardGame;
using InscryptionAPI.Items;
using UnityEngine;

namespace ZergMod.Scripts.Items
{
    public static class BiomassInABottle
    {
        public static ConsumableItemData Data;

        public static void Initialize()
        {
            Texture2D texture = Utils.GetTextureFromPath("Artwork/Items/biomassbottle.png");
            
            int currentRandomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
            bool attackBioMass = SeededRandom.Range(0, 100, currentRandomSeed++) <= 25;
            string cardName = attackBioMass ? "Zerg_JSON_biomassAttacker" : "Zerg_JSON_biomass";
            Data = ConsumableItemManager.NewCardInABottle(Plugin.PluginGuid, cardName, texture);
        }
    }
}