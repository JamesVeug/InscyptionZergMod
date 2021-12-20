using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using APIPlugin;
using TinyJSON;
using UnityEngine;
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
	    public const string PluginVersion = "0.9.0.0";
	    public const string DecalPath = "Artwork/watermark.png";

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

            // Cards
            XelNagaArtifact.Initialize();

            // JsonDumpNewCards();

            // Squirrel / Lava
            //ChangeSquirrelToLarva();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void JsonDumpNewCards()
        {
	        Dictionary<Ability, NewAbility> allCustomAbilities = new Dictionary<Ability, NewAbility>();
	        Dictionary<SpecialTriggeredAbility, NewSpecialAbility> allCustomSpecialAbilities = new Dictionary<SpecialTriggeredAbility, NewSpecialAbility>();
            foreach (NewAbility newAbility in NewAbility.abilities)
            {
	            allCustomAbilities[newAbility.ability] = newAbility;
            }
            foreach (NewSpecialAbility newSpecialAbility in NewSpecialAbility.specialAbilities)
            {
	            allCustomSpecialAbilities[newSpecialAbility.specialTriggeredAbility] = newSpecialAbility;
            }

            for (var index = 0; index < NewCard.cards.Count; index++)
            {
	            CardInfo cardInfo = NewCard.cards[index];
	            string path = Path.Combine(Directory, "Data", "Cards", cardInfo.name + ".jldr");

	            Dictionary<string, object> data = new Dictionary<string, object>();
	            data["name"] = cardInfo.name;
	            data["displayedName"] = cardInfo.displayedName;
	            data["description"] = cardInfo.description;
	            data["metaCategories"] = cardInfo.metaCategories;
	            data["cardComplexity"] = cardInfo.cardComplexity;
	            data["temple"] = cardInfo.temple;
	            data["baseAttack"] = cardInfo.baseAttack;
	            data["baseHealth"] = cardInfo.baseHealth;
	            data["bloodCost"] = cardInfo.BloodCost;
	            data["bonesCost"] = cardInfo.bonesCost;
	            data["appearanceBehaviour"] = cardInfo.appearanceBehaviour;

	            string textureName = Utils.TextureToPath[cardInfo.portraitTex.texture];
	            if (textureName != null)
	            {
		            data["texture"] = "JamesGames-ZergMod/" + textureName;
	            }

	            if (NewCard.emissions.TryGetValue(cardInfo.portraitTex.name, out Sprite emission))
	            {
		            string emitTextureName = Utils.TextureToPath[emission.texture];
		            data["emissionTexture"] = "JamesGames-ZergMod/" + emitTextureName;
	            }

	            if (cardInfo.alternatePortrait != null)
	            {
		            string altTextureName = Utils.TextureToPath[cardInfo.alternatePortrait.texture];
		            data["altTexture"] = "JamesGames-ZergMod/" + altTextureName;
	            }

	            if (cardInfo.abilities.Count > 0)
	            {
		            List<Ability> abilities = new List<Ability>();
		            List<Dictionary<string, object>> customAbilities = new List<Dictionary<string, object>>();

		            foreach (Ability ability in cardInfo.abilities)
		            {
			            if (allCustomAbilities.TryGetValue(ability, out NewAbility newAbility))
			            {
				            Dictionary<string, object> customAbility = new Dictionary<string, object>();
				            customAbility["GUID"] = PluginGuid;

				            var s = newAbility.id.ToString();
				            int startIndex = s.IndexOf('(') + 1;
				            customAbility["name"] = s.Substring(startIndex, s.IndexOf(')') - startIndex);
				            customAbilities.Add(customAbility);
			            }
			            else
			            {
				            abilities.Add(ability);
			            }
		            }

		            if (abilities.Count > 0)
		            {
			            data["abilities"] = abilities;
		            }

		            if (customAbilities.Count > 0)
		            {
			            data["customAbilities"] = customAbilities;
		            }
	            }

	            if (cardInfo.specialAbilities.Count > 0)
	            {
		            List<SpecialTriggeredAbility> specialAbilities = new List<SpecialTriggeredAbility>();
		            List<Dictionary<string, object>> customSpecialAbilities = new List<Dictionary<string, object>>();

		            foreach (SpecialTriggeredAbility specialAbility in cardInfo.specialAbilities)
		            {
			            if (allCustomSpecialAbilities.TryGetValue(specialAbility, out NewSpecialAbility newAbility))
			            {
				            Dictionary<string, object> customAbility = new Dictionary<string, object>();
				            customAbility["GUID"] = PluginGuid;

				            var s = newAbility.id.ToString();
				            int startIndex = s.IndexOf('(') + 1;
				            customAbility["name"] = s.Substring(startIndex, s.IndexOf(')') - startIndex);
				            customSpecialAbilities.Add(customAbility);
			            }
			            else
			            {
				            specialAbilities.Add(specialAbility);
			            }
		            }

		            if (specialAbilities.Count > 0)
		            {
			            data["specialAbilities"] = specialAbilities;
		            }

		            if (customSpecialAbilities.Count > 0)
		            {
			            data["customSpecialAbilities"] = customSpecialAbilities;
		            }
	            }


	            if (NewCard.evolveIds.TryGetValue(index, out EvolveIdentifier evolveIdentifier))
	            {
		            Dictionary<string, object> evolve = new Dictionary<string, object>();
		            evolve["name"] = evolveIdentifier.Evolution.evolution.name;
		            evolve["turnsToEvolve"] = evolveIdentifier.Evolution.turnsToEvolve;
		            data["evolution"] = evolve;
	            }

	            string dump = JSON.Dump(data, EncodeOptions.PrettyPrint);
	            if (File.Exists(path))
	            {
		            File.Delete(path);
	            }

	            Logger.LogInfo($"Dumping  {cardInfo.name} to {path}!");
	            File.WriteAllText(path, dump);
            }
        }

        public void ChangeSquirrelToLarva()
        {
	        List<Ability> abilities = new List<Ability> { Ability.TriStrike };
	        List<SpecialTriggeredAbility> specialAbilities = new List<SpecialTriggeredAbility> { LarvaSpecialAbility.specialAbility };
	        List<CardAppearanceBehaviour.Appearance> appearances = new List<CardAppearanceBehaviour.Appearance>
		        {
			        CardAppearanceBehaviour.Appearance.RareCardBackground
		        };

	        new CustomCard("Squirrel")
	        {
		        displayedName = "Zerg",
		        baseAttack = 7, 
		        baseHealth = 7, 
		        appearanceBehaviour = appearances,
		        tex = Utils.GetTextureFromPath("Artwork/zerg.png"),
		        abilities = abilities,
		        specialAbilities = specialAbilities,
		        decals = Utils.GetDecals()
	        };
        }
    }
}
