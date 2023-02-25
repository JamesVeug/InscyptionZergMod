using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using ZergMod.Scripts;
using ZergMod.Scripts.Abilities;
using ZergMod.Scripts.Cards;
using ZergMod.Scripts.Encounters;
using ZergMod.Scripts.Items;
using ZergMod.Scripts.Masks;
using ZergMod.Scripts.SpecialAbilities;
using ZergMod.Scripts.Traits;
using ZergMod.Scripts.VariableStatBehaviours;

namespace ZergMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("jamesgames.inscryption.starcraftcore", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.inscryption.zergmod";
	    public const string PluginName = "Zerg Mod";
	    public const string PluginVersion = "2.4.1.0";
	    public const string DecalPath = "Artwork/watermark.png";

        public static string Directory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            Directory = this.Info.Location.Replace("ZergMod.dll", "");
            new Harmony(PluginGuid).PatchAll();
            
            ZergMod.Scripts.Bosses.BossUtils.Initialize();
            
            // Traits
            ZerglingTrait.Initialize();
            
            // VariableStatBehaviours
            ZerglingVariableStat.Initialize();
            
            // Abilities
            SpawnLarvaAbility.Initialize(typeof(SpawnLarvaAbility));
            SpawnLocustAbility.Initialize(typeof(SpawnLocustAbility));
            StrafeCreepTumorAbility.Initialize(typeof(StrafeCreepTumorAbility));
            SummonZergAbility.Initialize(typeof(SummonZergAbility));
            SwarmSeedsAbility.Initialize(typeof(SwarmSeedsAbility));
            DevourAbility.Initialize(typeof(DevourAbility));
            
            // Special Abilities
            BrendaSpecialAbility.Initialize(typeof(BrendaSpecialAbility));
            BroodLordSpecialAbility.Initialize(typeof(BroodLordSpecialAbility));
            DehakaSpecialAbility.Initialize(typeof(DehakaSpecialAbility));
            LarvaSpecialAbility.Initialize(typeof(LarvaSpecialAbility));
            PrimalSpecialAbility.Initialize(typeof(PrimalSpecialAbility));
            ZerglingSpecialAbility.Initialize(typeof(ZerglingSpecialAbility));
            
            // Cards
            XelNagaArtifact.Initialize();
	        
	        // Masks
	        AbatharMask.Initialize();
	        
	        Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void Start()
        {
	        // Wait for JSONLoader to load Zerg related cards
            
	        // Items
	        BiomassInABottle.Initialize();
	        BiomassAttackerInABottle.Initialize();
            
	        // Encounters
	        AirEncounter.Initialize();
	        DroneEncounter.Initialize();
	        SwarmHostEncounter.Initialize();
	        UltraliskEncounter.Initialize();
	        ZerglingEncounter.Initialize();
	        
	        // Opponents
	        AbathurOpponent.Initialize();
            
	        // Sequences
	        EvolveSequencer.Initialize();
        }
    }
}
