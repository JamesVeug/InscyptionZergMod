using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Banelings
    {
        private const string DisplayName = "Banelings";
        private const string Description = "Explosive in their nature";
        private const string TextureFile = "Artwork/two_banelings.png";

        private const int BaseAttack = 3;
        private const int BaseHealth = 1;
        private const int BloodCost = 0;
        private const int BoneCost = 2;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.TraderOffer);
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,TextureFile));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);
	        
            List<Ability> abilities = new List<Ability> {Ability.TriStrike, Ability.Brittle};
	        
            NewCard.Add(DisplayName, metaCategories, CardComplexity.Simple, CardTemple.Nature,DisplayName,
                BaseAttack,
                BaseHealth,
                description: Description,
                cost:BloodCost,
                bonesCost:BoneCost,
                tribes:new List<Tribe> { Tribe.Insect },
                appearanceBehaviour:appearanceBehaviour, 
                tex:tex,
                abilities:abilities);
        }
    }
}