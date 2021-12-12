using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class CrawlerForest
    {
        private const string ID = "CrawlerForest";
        private const string DisplayName = "Crawler Forest";
        private const string Description = "A forest of Spine and Spore crawlers designed to defend you";
        private const string TextureFile = "Artwork/Cards/crawler_forest.png";
        private const string EmitTextureFile = "Artwork/Cards/crawler_forest_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 5;
        private const int BloodCost = 0;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.TerrainBackground);

            List<Ability> abilities = new List<Ability> { DetectorAbility.ability, Ability.Reach };
            List<SpecialTriggeredAbility> specialAbilities = new List<SpecialTriggeredAbility> { SpecialTriggeredAbility.Mirror };

            NewCard.Add(name: ID,
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
                emissionTex: Utils.GetTextureFromPath(EmitTextureFile),
                abilities: abilities,
                specialAbilities: specialAbilities,
                decals: Utils.GetDecals(),
                specialStatIcon: SpecialStatIcon.Mirror);
        }
    }
}