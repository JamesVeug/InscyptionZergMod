using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Leviathan
    {
        private const string DisplayName = "Leviathan";
        private const string Description = "Heavy air transport unit";
        private const string TextureFile = "Artwork/leviathan.png";

        private const int BaseAttack = 2;
        private const int BaseHealth = 10;
        private const int BloodCost = 0;
        private const int BoneCost = 10;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            Texture2D tex = Utils.GetTextureFromPath(TextureFile);
            Texture2D decal = Utils.GetTextureFromPath(Plugin.DecalPath);


            List<Ability> abilities = new List<Ability> { Ability.WhackAMole, Ability.Sharp };
	        
            NewCard.Add(DisplayName, metaCategories, CardComplexity.Simple, CardTemple.Nature, DisplayName,
                BaseAttack,
                BaseHealth,
                description: Description,
                cost: BloodCost,
                bonesCost: BoneCost,
                tribes: new List<Tribe> { Tribe.Insect },
                appearanceBehaviour: appearanceBehaviour,
                tex: tex,
                abilities:abilities,
                decals:new List<Texture>{decal});
        }
    }
}