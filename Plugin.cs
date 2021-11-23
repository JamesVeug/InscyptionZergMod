using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using APIPlugin;
using ZergMod.Scripts.Cards;

namespace ZergMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.inscryption.zergmod";
	    public const string PluginName = "Zerg Mod";
	    public const string PluginVersion = "0.3.0.0";
	    public const string DecalPath = "Artwork/watermark.png";

        public static string Directory;
        public static ManualLogSource Log;

        private void Awake()
        {
	        Log = Logger;
            Logger.LogInfo($"Loading {PluginName}...");
            Directory = this.Info.Location.Replace("ZergMod.dll", "");
            new Harmony("jamesgames.inscryption.zergmod").PatchAll();

            
            Egg.Initialize();
            
            // SpecialAbilities
            ZerglingSpecialAbility.Initialize();
            
            // Abilities
            RegenerateAbility.Initialize();
            RegestateAbility.Initialize();
            DoubleAttackAbility.Initialize();
            SpawnLarvaAbility.Initialize();
            DetectorAbility.Initialize();
            Draw2BroodlingsAbility.Initialize();
            Draw2LocustsAbility.Initialize();
            ArmouredAbility.Initialize();
            SplashDamageAbility.Initialize();
            AbductAbility.Initialize();
            
            // Evolutions
            Banelings.Initialize();
            Overseer.Initialize();
            Lurker.Initialize();
            Ravenger.Initialize();
            
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
            Broodlord.Initialize();
            Drone.Initialize();
            Ultralisk.Initialize();
            Dehaka.Initialize();
            InfestedTerran.Initialize();
            Infestor.Initialize();
            SwarmHost.Initialize();
            Leviathan.Initialize();
            Viper.Initialize();

            // Squirrel / Lava
            ChangeSquirrelToLarva();
            Logger.LogInfo($"Loaded {PluginName}!");
        }
        
        public void ChangeSquirrelToLarva()
        {
	        byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,"Artwork/larva.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability>{ SplashDamageAbility.ability, ArmouredAbility.ability };

	        new CustomCard("Squirrel")
	        {
		        displayedName="Larva", 
		        tex=tex, 
		        altTex=tex, 
		        /*baseAttack = 1, 
		        baseHealth = 10, 
		        abilities = abilities,*/
		        decals = Utils.GetDecals()
	        };
        }
    }

}
