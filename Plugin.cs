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
            ZerglingSpecialAbility.Initialize(typeof(ZerglingSpecialAbility));
            DehakaSpecialAbility.Initialize(typeof(DehakaSpecialAbility));
            LarvaSpecialAbility.Initialize(typeof(LarvaSpecialAbility));
            
            // Abilities
            RegenerateAbility.Initialize(typeof(RegenerateAbility));
            RegestateAbility.Initialize(typeof(RegestateAbility));
            DoubleAttackAbility.Initialize(typeof(DoubleAttackAbility));
            SpawnLarvaAbility.Initialize(typeof(SpawnLarvaAbility));
            DetectorAbility.Initialize(typeof(DetectorAbility));
            SwarmSeedsAbility.Initialize(typeof(SwarmSeedsAbility));
            SpawnLocustAbility.Initialize(typeof(SpawnLocustAbility));
            ArmoredAbility.Initialize(typeof(ArmoredAbility));
            SplashDamageAbility.Initialize(typeof(SplashDamageAbility));
            AbductAbility.Initialize(typeof(AbductAbility));
            ExplodeAbility.Initialize(typeof(ExplodeAbility));
            BombardAbility.Initialize(typeof(BombardAbility));
            HookAbility.Initialize(typeof(HookAbility));
            RicochetAbility.Initialize(typeof(RicochetAbility));
            BloodBankAbility.Initialize(typeof(BloodBankAbility));
            SummonZergAbility.Initialize(typeof(SummonZergAbility));

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
