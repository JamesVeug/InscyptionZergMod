using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace CardLoaderMod.Scripts.Cards
{
    public static class Egg
    {
        private const string DisplayName = "Egg";
        private const string Description = "Egg that is gestating into something else";
        private const string TextureFile = "Artwork/egg.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 3;
        private const int BloodCost = 0;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,TextureFile));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            List<Ability> abilities = new List<Ability> {Ability.Evolve};
	        
            NewCard.Add(DisplayName, metaCategories, CardComplexity.Simple, CardTemple.Nature,DisplayName,
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