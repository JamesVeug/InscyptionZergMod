using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Drone
    {
        private const string DisplayName = "Drone";
        private const string Description = "Worker unit will gather resources for you";
        private const string TextureFile = "Artwork/drone.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 1;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { Ability.BoneDigger, Ability.Submerge };
            
            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Crawler Forest");
            EvolveIdentifier identifier = new EvolveIdentifier("CrawlerForest", 1, new CardModificationInfo(cardInfo));

            NewCard.Add(name: DisplayName,
                displayedName: DisplayName,
                baseAttack: BaseAttack,
                baseHealth: BaseHealth,
                metaCategories: metaCategories,
                cardComplexity: CardComplexity.Simple,
                temple: CardTemple.Nature,
                description: Description,
                bloodCost: BloodCost,
                bonesCost: BoneCost,
                tribes: new List<Tribe> { Tribe.Insect },
                appearanceBehaviour: appearanceBehaviour,
                defaultTex: Utils.GetTextureFromPath(TextureFile),
                abilities: abilities,
                decals: Utils.GetDecals(),
                evolveId: identifier);
        }
    }
}