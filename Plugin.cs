using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
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
	    public const string PluginGuid = "jamesgames.inscryption.zergmodkaycee";
	    public const string PluginName = "Zerg Mod Kaycee";
	    public const string PluginVersion = "1.1.1.0";
	    public const string DecalPath = "Artwork/watermark_zerg.png";

        public static string Directory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            Directory = this.Info.Location.Replace("ZergMod.dll", "");
            new Harmony(PluginGuid).PatchAll();

            // Backgrounds
            XelNagaBackground.Initialize();
            
            // SpecialAbilities
            ZerglingSpecialAbility.Initialize(typeof(ZerglingSpecialAbility));
            DehakaSpecialAbility.Initialize(typeof(DehakaSpecialAbility));
            LarvaSpecialAbility.Initialize(typeof(LarvaSpecialAbility));
            PrimalSpecialAbility.Initialize(typeof(PrimalSpecialAbility));
            BroodLordSpecialAbility.Initialize(typeof(BroodLordSpecialAbility));
	        
            // Abilities
            RegenerateAbility.Initialize(typeof(RegenerateAbility));
            DoubleAttackAbility.Initialize(typeof(DoubleAttackAbility));
            SpawnLarvaAbility.Initialize(typeof(SpawnLarvaAbility));
            DetectorAbility.Initialize(typeof(DetectorAbility));
            SwarmSeedsAbility.Initialize(typeof(SwarmSeedsAbility));
            SpawnLocustAbility.Initialize(typeof(SpawnLocustAbility));
            ArmoredAbility.Initialize(typeof(ArmoredAbility));
            SplashDamageAbility.Initialize(typeof(SplashDamageAbility));
            AbductAbility.Initialize(typeof(AbductAbility));
            ExplodeAbility.Initialize(typeof(ExplodeAbility));
            HookAbility.Initialize(typeof(HookAbility));
            RicochetAbility.Initialize(typeof(RicochetAbility));
            BloodBankAbility.Initialize(typeof(BloodBankAbility));
            SummonZergAbility.Initialize(typeof(SummonZergAbility));

            // Cards
            XelNagaArtifact.Initialize();

            //ChangeSquirrelToLarva();
            
            Logger.LogInfo($"Loaded {PluginName}!");
        }
        
        public void ChangeSquirrelToLarva()
        {
	        List<Ability> abilities = new List<Ability> { };

	        new CustomCard("Squirrel")
	        {
		        displayedName = "Marine",
		        baseAttack = 1, 
		        baseHealth = 1, 
		        abilities = abilities,
		        decals = Utils.GetDecals(),
		        tex = Utils.GetTextureFromPath("Artwork/Cards/marine.png")
	        };
        }
    }
}
