using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using StarCraftCore.Scripts.Regions;

namespace ZergMod.Scripts.Encounters
{
    public class ZerglingEncounter
    {
        public static void Initialize()
        {
            RegionData charRegion = CharRegion.regionData;
            
            EncounterBlueprintData data = EncounterManager.New(Plugin.PluginGuid + "_Zergling");
            data.SetDifficulty(1, 15);
            data.AddDominantTribes(Tribe.Insect);
            data.AddRandomReplacementCards(new string[]
            {
                "Zerg_JSON_Baneling",
                "Zerg_JSON_Roach",
                "Zerg_JSON_Hydralisk",
                "Zerg_JSON_Roach",
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
                    card = CardLoader.GetCardByName("Zerg_JSON_Zerglings"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Overlord"),
                    minDifficulty = 0,
                    maxDifficulty = 4,
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Zerglings"),
                    minDifficulty = 5,
                    maxDifficulty = 15,
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Zerglings"),
                    minDifficulty = 2,
                    maxDifficulty = 15,
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Baneling"),
                    minDifficulty = 5,
                    maxDifficulty = 15,
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Zerglings"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Zerglings"),
                    minDifficulty = 5,
                    maxDifficulty = 15,
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Overlord"),
                    minDifficulty = 0,
                    maxDifficulty = 4,
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Hydralisk"),
                    minDifficulty = 5,
                    maxDifficulty = 15,
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Zerglings"),
                    minDifficulty = 2,
                    maxDifficulty = 15,
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