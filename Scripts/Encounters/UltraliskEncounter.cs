using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using StarCraftCore.Scripts.Regions;

namespace ZergMod.Scripts.Encounters
{
    public class UltraliskEncounter
    {
        public static void Initialize()
        {
            RegionData charRegion = CharRegion.regionData;
            
            EncounterBlueprintData data = EncounterManager.New(Plugin.PluginGuid + "_Zergling");
            data.SetDifficulty(5, 15);
            data.AddDominantTribes(Tribe.Insect);
            data.AddRandomReplacementCards(new string[]
            {
                "Zerg_JSON_Hydralisk",
            });
            data.AddTurnMods(new []
            {
                new EncounterBlueprintData.TurnModBlueprint()
                {
                    turn = 1,
                    applyAtDifficulty = 1,
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Hydralisk"),
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Ultralisk"),
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Hydralisk"),
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Hydralisk"),
                }
            });

            if (charRegion != null)
            {
                if (!CharRegion.HasClearedEncounters)
                {
                    charRegion.encounters.Clear();
                    CharRegion.HasClearedEncounters = true;
                }

                data.regionSpecific = true;
                charRegion.AddEncounters(data);
            }
        }
    }
}