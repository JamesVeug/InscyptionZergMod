using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using APIPlugin;
using ZergMod.Scripts.Abilities;
using ZergMod.Scripts.Backgrounds;
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
	    public const string PluginVersion = "0.8.0.0";
	    public const string DecalPath = "Artwork/watermark.png";

        public static string Directory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            Directory = this.Info.Location.Replace("ZergMod.dll", "");
            new Harmony("jamesgames.inscryption.zergmod").PatchAll();

            // Backgrounds
            XelNagaBackground.Initialize();
            
            Egg.Initialize();
            
            // SpecialAbilities
            ZerglingSpecialAbility.Initialize();
            DehakaSpecialAbility.Initialize();
            LarvaSpecialAbility.Initialize();
            
            // Abilities
            RegenerateAbility.Initialize();
            RegestateAbility.Initialize();
            DoubleAttackAbility.Initialize();
            SpawnLarvaAbility.Initialize();
            DetectorAbility.Initialize();
            SwarmSeedsAbility.Initialize();
            SpawnLocustAbility.Initialize();
            ArmoredAbility.Initialize();
            SplashDamageAbility.Initialize();
            AbductAbility.Initialize();
            ExplodeAbility.Initialize();
            BombardAbility.Initialize();
            HookAbility.Initialize();
            RicochetAbility.Initialize();
            BloodBankAbility.Initialize();
            SummonZergAbility.Initialize();
            
            // Evolutions
            Banelings.Initialize();
            Overseer.Initialize();
            Lurker.Initialize();
            Ravager.Initialize();
            Devourer.Initialize();
            Guardian.Initialize();
            CrawlerForest.Initialize();
            Broodlord.Initialize();
            
            // Units
            Locust.Initialize();
            Zerglings.Initialize();
            Roach.Initialize();
            Mutalisk.Initialize();
            Overlord.Initialize();
            Kerrigan.Initialize();
            Queen.Initialize();
            Hydralisk.Initialize();
            Broodling.Initialize();
            Drone.Initialize();
            Ultralisk.Initialize();
            Dehaka.Initialize();
            InfestedTerran.Initialize();
            Infestor.Initialize();
            SwarmHost.Initialize();
            Leviathan.Initialize();
            Viper.Initialize();
            Scourge.Initialize();
            Corruptor.Initialize();
            XelNagaArtifact.Initialize();
            Larva.Initialize();

            Logger.LogInfo($"Loaded {PluginName}!");
        }
    }
}
