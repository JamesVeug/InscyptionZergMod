using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class InfestedTerran
    {
        private const string DisplayName = "Infested Terran";
        private const string Description = "Something horrible happened to that soldier";
        private const string TextureFile = "Artwork/infested_terran.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 2;
        private const int BloodCost = 1;
        private const int BoneCost = 1;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,TextureFile));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);
	        
            List<Ability> abilities = new List<Ability> { Ability.Brittle, Ability.DrawCopy };

            NewCard.Add(DisplayName, metaCategories, CardComplexity.Simple, CardTemple.Nature, DisplayName,
                BaseAttack,
                BaseHealth,
                description: Description,
                cost: BloodCost,
                bonesCost: BoneCost,
                tribes: new List<Tribe> { Tribe.Insect },
                appearanceBehaviour: appearanceBehaviour,
                tex: tex,
                abilities: abilities
                );
        }
    }
}