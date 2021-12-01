using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Corruptor
    {
        private const string DisplayName = "Corruptor";
        private const string Description = "A squid-like zerg flying unit.";
        private const string TextureFile = "Artwork/corruptor.png";

        private const int BaseAttack = 2;
        private const int BaseHealth = 2;
        private const int BloodCost = 2;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { Ability.Flying };

            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Devourer");
            EvolveIdentifier identifier = new EvolveIdentifier("Devourer", 1, new CardModificationInfo(cardInfo));

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
                evolveId:identifier);
        }
    }
}