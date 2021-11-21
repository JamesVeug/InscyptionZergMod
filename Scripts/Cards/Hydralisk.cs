using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Hydralisk
    {
        private const string DisplayName = "Hydralisk";
        private const string Description = "Great for taking out flyers";
        private const string TextureFile = "Artwork/hydralisk.png";

        private const int BaseAttack = 3;
        private const int BaseHealth = 2;
        private const int BloodCost = 2;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,TextureFile));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            NewCard.Add(DisplayName, metaCategories, 
                CardComplexity.Simple, 
                CardTemple.Nature,
                DisplayName,
                BaseAttack,
                BaseHealth,
                description:Description,
                cost:BloodCost,
                bonesCost:BoneCost,
                tribes:new List<Tribe> { Tribe.Insect },
                appearanceBehaviour:appearanceBehaviour, 
                tex:tex);
        }
    }
}