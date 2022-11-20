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
            EncounterBlueprintData data = EncounterManager.New("Drone Rush");
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
                    minDifficulty = 5,
                    maxDifficulty = 15
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                    minDifficulty = 10,
                    maxDifficulty = 15
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
                    minDifficulty = 5,
                    maxDifficulty = 15,
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Drone"),
                    minDifficulty = 10,
                    maxDifficulty = 15,
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
                    minDifficulty = 5,
                    maxDifficulty = 15,
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_CrawlerForest"),
                    minDifficulty = 10,
                    maxDifficulty = 15,
                },
            });

            RegionData charRegion = CharRegion.regionData;
            if (charRegion != null)
            {
                data.regionSpecific = true;
                charRegion.AddEncounters(data);
                Plugin.Log.LogInfo("Added region " + data.name + " " + charRegion.encounters[charRegion.encounters.Count-1].name);
                foreach (EncounterBlueprintData encounter in charRegion.encounters)
                {
                    Plugin.Log.LogInfo("\t" + encounter.name);
                }
            }
        }
    }
}