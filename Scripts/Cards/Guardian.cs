using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Guardian
    {
        private const string DisplayName = "Guardian";
        private const string Description = "Despite being devastating in terrestrial combat, guardians are next to useless in space combat";
        private const string TextureFile = "Artwork/guardian.png";

        private const int BaseAttack = 4;
        private const int BaseHealth = 6;
        private const int BloodCost = 3;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> {  };

            NewCard.Add(name: DisplayName,
                displayedName: DisplayName,
                baseAttack: BaseAttack,
                baseHealth: BaseHealth,
                metaCategories: metaCategories,
                cardComplexity: CardComplexity.Intermediate,
                temple: CardTemple.Nature,
                description: Description,
                bloodCost: BloodCost,
                bonesCost: BoneCost,
                tribes: new List<Tribe> { Tribe.Insect },
                appearanceBehaviour: appearanceBehaviour,
                defaultTex: Utils.GetTextureFromPath(TextureFile),
                abilities: abilities,
                decals: Utils.GetDecals());
        }
    }
}