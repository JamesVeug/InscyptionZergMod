using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Locust
    {
        private const string DisplayName = "Locust";
        private const string Description = "";
        private const string TextureFile = "Artwork/locust.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 1;
        private const int BloodCost = 0;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,TextureFile));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            List<Ability> abilities = new List<Ability> { Ability.Brittle };

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
                tex:tex,
                abilities:abilities);
        }
    }
}