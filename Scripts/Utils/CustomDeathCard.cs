using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public class CustomDeathCardBase
    {
        public Sprite Sprite;
        public bool AllowRandomMouths;
        public bool AllowRandomEyes;
        public bool AllowRandomHeads;
    }
    
    public class CustomDeathCard
    {
        public static Dictionary<CompositeFigurine.FigurineType, Sprite> HeadSpriteLookup = new Dictionary<CompositeFigurine.FigurineType, Sprite>();
        public static Dictionary<int, Sprite> MouthSpriteLookup = new Dictionary<int, Sprite>();
        public static Dictionary<int, Sprite> EyesSpriteLookup = new Dictionary<int, Sprite>();
        public static Dictionary<int, Sprite> EyesEmissionSpriteLookup = new Dictionary<int, Sprite>();
        public static Dictionary<int, CustomDeathCardBase> BaseLookup = new Dictionary<int, CustomDeathCardBase>();
        public static Dictionary<int, Sprite> BaseEmitSpriteLookup = new Dictionary<int, Sprite>();

        public static List<CustomDeathCard> DeathCards = new List<CustomDeathCard>();

        private static Rect MouthRect = new Rect(0, 0, 33, 11);
        private static Rect EyesRect = new Rect(0, 0, 33, 16);
        private static Vector2 Pivot = new Vector2(0.5f, 0.5f);
        
        public DeathCardModificationInfo cardModificationInfo;
        public TailParams tailIdentifier;
        public string name;
        public string id;
        public int baseIndexOverride;
        
        public static void AddNewDeathCard(string name, 
            int attack, 
            int health, 
            int boneCost, 
            int bloodCost, 
            CompositeFigurine.FigurineType figurineType = CompositeFigurine.FigurineType.NUM_FIGURINES, 
            int mouthIndex = -1, 
            int eyesIndex = -1,
            int baseIndex=-1, 
            List<Ability> abilities=null,
            TailParams tail=null)
        {
            string id = "DEATHCARD_" + (DeathCards.Count + 1);
            
            DeathCardModificationInfo cardInfo = new DeathCardModificationInfo()
            {
                customCardId = id,
                attackAdjustment = attack,
                healthAdjustment = health,
                nameReplacement = name,
                abilities = abilities == null ? new List<Ability>() : new List<Ability>(abilities),
                bloodCostAdjustment = bloodCost,
                bonesCostAdjustment = boneCost,
                deathCardInfo = new DeathCardInfo(figurineType, mouthIndex, eyesIndex)
            };

            CustomDeathCard customDeathCard = new CustomDeathCard()
            {
                id = id,
                name = name,
                baseIndexOverride = baseIndex,
                cardModificationInfo = cardInfo,
                tailIdentifier = tail,
            };
            
            Plugin.Log.LogInfo("Adding Death Card " + name);
            DeathCards.Add(customDeathCard);
        }

        public static int AddNewBase(string name, string path, string emitPath=null, bool allowCustomHeads=true, bool allowCustomEyes=true, bool allowCustomMouths=true)
        {
            Texture2D tex = GetTexture(name + "_base", path);
            Sprite sprite = Sprite.Create(tex, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
            sprite.name = tex.name;

            int value = 100 + BaseLookup.Count;
            BaseLookup[value] = new CustomDeathCardBase()
            {
                Sprite = sprite,
                AllowRandomHeads = allowCustomHeads,
                AllowRandomEyes = allowCustomEyes,
                AllowRandomMouths = allowCustomMouths,
            };
            
            // Emit
            if (emitPath != null)
            {
                Texture2D emit = GetTexture(name + "_base_emit", emitPath);
                Sprite emitSprite = Sprite.Create(emit, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
                emitSprite.name = emit.name;
                BaseEmitSpriteLookup[value] = emitSprite;
            }

            Plugin.Log.LogInfo("Added new Base Card: " + name);

            return value;
        }

        public static CompositeFigurine.FigurineType AddNewHead(string name, string path)
        {
            Texture2D textureFromPath = GetTexture(name + "_head", path);
            Sprite sprite = Sprite.Create(textureFromPath, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
            sprite.name = textureFromPath.name;

            CompositeFigurine.FigurineType value = (CompositeFigurine.FigurineType)100 + HeadSpriteLookup.Count;
            HeadSpriteLookup[value] = sprite;
            
            Plugin.Log.LogInfo("Added new Death Card Head: " + name);

            return value;
        }

        public static int AddNewEyes(string name, string path, string emissionPath)
        {
            Texture2D tex = GetTexture(name + "_eyes", path);
            Sprite sprite = Sprite.Create(tex, EyesRect, Pivot);
            sprite.name = tex.name;
                
            Texture2D texEmission = GetTexture(name + "_eyes_emit", emissionPath);
            Sprite spriteEmission = Sprite.Create(texEmission, EyesRect, Pivot);
            spriteEmission.name = texEmission.name;

            int value = 100 + HeadSpriteLookup.Count;
            EyesSpriteLookup[value] = sprite;
            EyesEmissionSpriteLookup[value] = spriteEmission;
            
            Plugin.Log.LogInfo("Added new Death Card Head: " + name);

            return value;
        }
        
        public static int AddNewMouth(string name, string path)
        {
            Texture2D textureFromPath = GetTexture(name + "_mouth", path);
            
            Sprite sprite = Sprite.Create(textureFromPath, MouthRect, Pivot);
            sprite.name = textureFromPath.name;

            int value = 100 + HeadSpriteLookup.Count;
            MouthSpriteLookup[value] = sprite;
            
            Plugin.Log.LogInfo("Added new Death Card Mouth: " + name);

            return value;
        }

        private static Texture2D GetTexture(string name, string path)
        {
            Texture2D textureFromPath = Utils.GetTextureFromPath(path);
            textureFromPath.filterMode = FilterMode.Point;
            textureFromPath.name = name;
            return textureFromPath;
        }
    }
}