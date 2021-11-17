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

namespace CardLoaderMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "jamesgames.inscryption.zergmod";
        private const string PluginName = "Zerg Mod";
        private const string PluginVersion = "1.0.0.0";

        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            CreateGenericEggCard();
            //AddZerglingEvolveAbility();
            AddHealAbility();
            AddRegestateAbility();
            AddDoubleAttackAbility();
            AddSpawnLarvaAbility();

            ChangeSquirrelToLarva();
            AddZerglings();
            AddRoach();
            AddMutalisk();
            AddOverlord();
            AddKerrigan();
            AddQueen();
            AddHydralisk();
        }

        public void AddRegestateAbility()
        {
	        AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
	        info.powerLevel = 0;
	        info.rulebookName = "Regestate";
	        info.rulebookDescription = "When this card is killed it will transform into an Egg for it to regenerate into its original form";
	        info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

	        List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
	        DialogueEvent.Line line = new DialogueEvent.Line();
	        line.text = "That card seems unbalanced";
	        lines.Add(line);
	        info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/egg.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        NewAbility ability = new NewAbility(info,typeof(RegestateAbility),tex,AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
	        RegestateAbility.ability = ability.ability;
        }
        
        public void AddDoubleAttackAbility()
        {
	        AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
	        info.powerLevel = 0;
	        info.rulebookName = "Final Attack";
	        info.rulebookDescription = "When this card deals damage it will follow up with one more attack.";
	        info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

	        List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
	        DialogueEvent.Line line = new DialogueEvent.Line();
	        line.text = "Oof that one will be painful";
	        lines.Add(line);
	        info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/double_attack.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        NewAbility ability = new NewAbility(info,typeof(DoubleAttackAbility),tex,AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
	        DoubleAttackAbility.ability = ability.ability;
        }
        
        public void AddSpawnLarvaAbility()
        {
	        AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
	        info.powerLevel = 0;
	        info.rulebookName = "Spawn Larva";
	        info.rulebookDescription = "When placed on the board will create larva on both sides";
	        info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

	        List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
	        DialogueEvent.Line line = new DialogueEvent.Line();
	        line.text = "Extra Larva? Trying to rush me i see";
	        lines.Add(line);
	        info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/spawn_larva.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        NewAbility ability = new NewAbility(info,typeof(SpawnLarvaAbility),tex,AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
	        SpawnLarvaAbility.ability = ability.ability;
        }

        public void AddHealAbility()
        {
	        AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
	        info.powerLevel = 0;
	        info.rulebookName = "Regenerate";
	        info.rulebookDescription = "Heals 1 health at the end of a turn";
	        info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

	        List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
	        DialogueEvent.Line line = new DialogueEvent.Line();
	        line.text = "Delaying the inevitable i see";
	        lines.Add(line);
	        info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/heal.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        NewAbility ability = new NewAbility(info,typeof(HealAbility),tex,AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
	        HealAbility.ability = ability.ability;
        }
        
        public void AddZerglingEvolveAbility()
        {
	        AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
	        info.powerLevel = 0;
	        info.rulebookName = "Hatch Zergling's";
	        info.rulebookDescription = "Hatches 3 Zergling's on the board at the end of your opponents turn";
	        info.metaCategories = new List<AbilityMetaCategory> {AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular};

	        List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
	        DialogueEvent.Line line = new DialogueEvent.Line();
	        line.text = "Whoa what happened there?";
	        lines.Add(line);
	        info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/spawn_3_zerglings.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        NewAbility ability = new NewAbility(info,typeof(ZerglingEvolveAbility),tex,AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
	        ZerglingEvolveAbility.ability = ability.ability;
        }

        public void ChangeSquirrelToLarva()
        {
	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/larva.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability>() { DoubleAttackAbility.ability, Ability.TriStrike };
	        
	        new CustomCard("Squirrel") {displayedName="Larva", tex=tex, abilities = abilities, baseHealth = 10};
        }

        public void CreateGenericEggCard()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/egg.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability> {Ability.Evolve};
	        
	        NewCard.Add("Egg", metaCategories, CardComplexity.Simple, CardTemple.Nature,"Egg",0,3,
		        description:"Egg that is gestating into something else",
		        cost:1,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex,
		        abilities:abilities);
        }
        
        public void CreateEggForCard(string cardName, int turnsToEvolve, Ability evolveAbility=Ability.Evolve, bool rare=false, bool choiceNode=true)
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        if (rare)
	        {
		        metaCategories.Add(CardMetaCategory.Rare);
	        }

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/egg.png"));
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

        public void AddZerglings()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.ChoiceNode);
	        metaCategories.Add(CardMetaCategory.TraderOffer);
	        metaCategories.Add(CardMetaCategory.Part3Random);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/two_zerglings.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);
	        
	        List<Ability> abilities = new List<Ability> {DoubleAttackAbility.ability};
	        
	        int health = 1;
	        int baseAttack = 2;
	        NewCard.Add("Zerglings", metaCategories, CardComplexity.Simple, CardTemple.Nature,"Zerglings",
		        baseAttack,
		        health,
		        description:
		        "Friendly alien dog",
		        cost:1,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex,
		        abilities:abilities);
        }

        public void AddMutalisk()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.ChoiceNode);
	        metaCategories.Add(CardMetaCategory.TraderOffer);
	        metaCategories.Add(CardMetaCategory.Part3Random);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/mutalisk.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability> { Ability.Flying, Ability.SplitStrike };

	        int baseAttack = 3;
	        var baseHealth = 1;
	        NewCard.Add("Mutalisk", metaCategories, 
		        CardComplexity.Simple, 
		        CardTemple.Nature,
		        "Mutalisk",
		        baseAttack,
		        baseHealth,
		        description:"Flying devil",
		        cost:2,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex, abilities:abilities);
        }
        
        public void AddOverlord()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.ChoiceNode);
	        metaCategories.Add(CardMetaCategory.TraderOffer);
	        metaCategories.Add(CardMetaCategory.Part3Random);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/overlord.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability> { Ability.Reach, Ability.Flying };

	        int baseAttack = 0;
	        var baseHealth = 4;
	        NewCard.Add("Overlord", metaCategories, 
		        CardComplexity.Simple, 
		        CardTemple.Nature,
		        "Overlord",
		        baseAttack,
		        baseHealth,
		        description:"Lazy alien balloon",
		        cost:1,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex, abilities:abilities);
        }

        public void AddRoach()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.ChoiceNode);
	        metaCategories.Add(CardMetaCategory.TraderOffer);
	        metaCategories.Add(CardMetaCategory.Part3Random);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/roach.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability> { HealAbility.ability };

	        int baseAttack = 2;
	        int baseHealth = 2;
	        NewCard.Add("Roach", metaCategories, 
		        CardComplexity.Simple, 
		        CardTemple.Nature,
		        "Roach",
		        baseAttack,
		        baseHealth,
		        description:"Quick attacker",
		        cost:2,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex,
		        abilities:abilities);
        }

        public void AddQueen()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.ChoiceNode);
	        metaCategories.Add(CardMetaCategory.TraderOffer);
	        metaCategories.Add(CardMetaCategory.Part3Random);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/queen.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability> { SpawnLarvaAbility.ability };

	        int baseAttack = 1;
	        int baseHealth = 3;
	        NewCard.Add("Queen", metaCategories, 
		        CardComplexity.Simple, 
		        CardTemple.Nature,
		        "Queen",
		        baseAttack,
		        baseHealth,
		        description:"Brings more units to the field faster",
		        cost:2,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex,
		        abilities:abilities);
        }

        public void AddHydralisk()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.ChoiceNode);
	        metaCategories.Add(CardMetaCategory.TraderOffer);
	        metaCategories.Add(CardMetaCategory.Part3Random);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/hydralisk.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        int baseAttack = 3;
	        int baseHealth = 2;
	        NewCard.Add("Hydralisk", metaCategories, 
		        CardComplexity.Simple, 
		        CardTemple.Nature,
		        "Hydralisk",
		        baseAttack,
		        baseHealth,
		        description:"Great for taking out flyers",
		        cost:2,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex);
        }

        public void AddKerrigan()
        {
	        List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
	        metaCategories.Add(CardMetaCategory.Rare);
	        metaCategories.Add(CardMetaCategory.ChoiceNode);
	        metaCategories.Add(CardMetaCategory.TraderOffer);
	        metaCategories.Add(CardMetaCategory.Part3Random);

	        List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
	        appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

	        byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("CardLoaderMod.dll",""),"Artwork/kerrigan.png"));
	        Texture2D tex = new Texture2D(2,2);
	        tex.LoadImage(imgBytes);

	        List<Ability> abilities = new List<Ability> { RegestateAbility.ability };

	        int baseAttack = 3;
	        int baseHealth = 3;
	        NewCard.Add("Kerrigan", metaCategories, 
		        CardComplexity.Simple, 
		        CardTemple.Nature,
		        "Kerrigan",
		        baseAttack,
		        baseHealth,
		        description:"Hero that never dies",
		        cost:3,
		        abilities:abilities,
		        tribes:new List<Tribe> { Tribe.Insect },
		        appearanceBehaviour:appearanceBehaviour, 
		        tex:tex,
		        onePerDeck:true);
        }
    }

}
