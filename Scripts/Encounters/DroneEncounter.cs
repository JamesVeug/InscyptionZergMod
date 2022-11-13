using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using StarCraftCore.Scripts.Regions;

namespace ZergMod.Scripts.Encounters
{
    public class DroneEncounter
    {
        public static void Initialize()
        {
            RegionData charRegion = CharRegion.regionData;
            
            EncounterBlueprintData data = EncounterManager.New(Plugin.PluginGuid + "_Zergling");
            data.SetDifficulty(1, 5);
            data.AddDominantTribes(Tribe.Insect);
            data.AddRandomReplacementCards(new string[]
            {
                "Zerg_JSON_Zerglings",
                "Zerg_JSON_Overlord",
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
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                    minDifficulty = 3,
                    maxDifficulty = 5,
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                    minDifficulty = 3,
                    maxDifficulty = 5,
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                    minDifficulty = 4,
                    maxDifficulty = 5,
                },
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