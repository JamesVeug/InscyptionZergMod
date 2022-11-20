using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using StarCraftCore.Scripts.Regions;

namespace ZergMod.Scripts.Encounters
{
    public class SwarmHostEncounter
    {
        public static void Initialize()
        {
            EncounterBlueprintData data = EncounterManager.New("Swarm Host snipe");
            data.SetDifficulty(1, 15);
            data.AddDominantTribes(Tribe.Insect);
            data.AddRandomReplacementCards(new string[]
            {
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
                    card = CardLoader.GetCardByName("Zerg_JSON_Locust"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Locust"),
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Roach"),
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Locust"),
                },
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Locust"),
                },
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Roach"),
                }
            });
            data.AddTurn(new EncounterBlueprintData.CardBlueprint[]
            {
                new EncounterBlueprintData.CardBlueprint()
                {
                    card = CardLoader.GetCardByName("Zerg_JSON_Swarm Host"),
                }
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