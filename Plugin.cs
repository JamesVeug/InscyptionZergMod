using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using ZergMod.Scripts;
using ZergMod.Scripts.Abilities;
using ZergMod.Scripts.Cards;
using ZergMod.Scripts.SpecialAbilities;

namespace ZergMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.inscryption.zergmod";
	    public const string PluginName = "Zerg Mod";
	    public const string PluginVersion = "2.0.0.0";
	    public const string DecalPath = "Artwork/watermark.png";

        public static string Directory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            Directory = this.Info.Location.Replace("ZergMod.dll", "");
            new Harmony(PluginGuid).PatchAll();

            // Abilities
            SpawnLarvaAbility.Initialize(typeof(SpawnLarvaAbility));
            SpawnLocustAbility.Initialize(typeof(SpawnLocustAbility));
            StrafeCreepTumorAbility.Initialize(typeof(StrafeCreepTumorAbility));
            SummonZergAbility.Initialize(typeof(SummonZergAbility));
            SwarmSeedsAbility.Initialize(typeof(SwarmSeedsAbility));
            
            // Special Abilities
            BrendaSpecialAbility.Initialize(typeof(BrendaSpecialAbility));
            BroodLordSpecialAbility.Initialize(typeof(BroodLordSpecialAbility));
            DehakaSpecialAbility.Initialize(typeof(DehakaSpecialAbility));
            LarvaSpecialAbility.Initialize(typeof(LarvaSpecialAbility));
            PrimalSpecialAbility.Initialize(typeof(PrimalSpecialAbility));
            ZerglingSpecialAbility.Initialize(typeof(ZerglingSpecialAbility));
            
            // Cards
            XelNagaArtifact.Initialize();
            
            // Encounters
            EvolveSequencer.Initialize();
            
            CardInfo card = CardManager.BaseGameCards.CardByName("Wolf");
            card.AddTraits(Trait.KillsSurvivors);

            
            CardInfo card2 = CardManager.BaseGameCards.CardByName("PackRat");
            card2.baseAttack = 1;
            ChangeSquirrelToLarva();

            Logger.LogInfo($"Loaded {PluginName}!");
        }
        
        public void ChangeSquirrelToLarva()
        {
	        List<Ability> abilities = new List<Ability> { StrafeCreepTumorAbility.ability };

	        CardInfo card = CardManager.BaseGameCards.CardByName("Squirrel");
	        card.baseAttack = 1;
	        card.baseHealth = 10;
	        card.abilities = abilities;
	        card.decals = Utils.GetDecals();
        }
    }
}
