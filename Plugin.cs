using BepInEx;
using BepInEx.Logging;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
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
	    public const string PluginVersion = "0.1.0.0";

        public static string Directory;

        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            Directory = this.Info.Location.Replace("CardLoaderMod.dll", "");
            
            Egg.Initialize();
            
            // Abilities
            HealAbility.Initialize();
            RegestateAbility.Initialize();
            DoubleAttackAbility.Initialize();
            SpawnLarvaAbility.Initialize();
            
            // Units
            Zerglings.Initialize();
            Roach.Initialize();
            Mutalisk.Initialize();
            Overlord.Initialize();
            Kerrigan.Initialize();
            Queen.Initialize();
            Hydralisk.Initialize();

            // Squirrel / Lava
            ChangeSquirrelToLarva();
        }
        
        public void ChangeSquirrelToLarva()
        {
	        byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,"Artwork/larva.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability>() { DoubleAttackAbility.ability, Ability.TriStrike };
	        
	        new CustomCard("Squirrel") {displayedName="Larva", tex=tex, abilities = abilities, baseHealth = 10};
        }

        /// <summary>
        /// Deprecated. No longer used
        /// </summary>
        public void CreateEggForCard(string cardName, int turnsToEvolve, Ability evolveAbility=Ability.Evolve, bool rare=false, bool choiceNode=true)
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        if (rare)
	        {
		        metaCategories.Add(CardMetaCategory.Rare);
	        }

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,"Artwork/egg.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability> {evolveAbility, Ability.DeathShield};
	        CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == cardName);
	        EvolveIdentifier identifier = new EvolveIdentifier(cardName, turnsToEvolve, new CardModificationInfo(cardInfo));
	        
	        NewCard.Add(cardName + " Egg", metaCategories, CardComplexity.Simple, CardTemple.Nature,cardName + " Egg",0,2,
		        description:"Egg that is gestates into a " + cardName,
		        cost:1,
		        abilities:abilities,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex,
		        evolveId:identifier);
        }
    }

}
