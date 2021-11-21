using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Overlord
    {
        private const string DisplayName = "Overlord";
        private const string Description = "Lazy alien balloon";
        private const string TextureFile = "Artwork/overlord.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 4;
        private const int BloodCost = 1;
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

            List<Ability> abilities = new List<Ability> { Ability.Reach, Ability.Flying };
	        
            // Evolve into Overseer
            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Overseer");
            EvolveIdentifier identifier = new EvolveIdentifier("Overseer", 1, new CardModificationInfo(cardInfo));

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
                tex:tex, abilities:abilities,
                evolveId:identifier);
        }
    }
}