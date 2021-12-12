using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Queen
    {
        public const string ID = "Queen";
        private const string DisplayName = "Queen";
        private const string Description = "Brings more units to the field faster";
        private const string TextureFile = "Artwork/Cards/queen.png";
        private const string EmitTextureFile = "Artwork/Cards/queen_emit.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 3;
        private const int BloodCost = 2;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { SpawnLarvaAbility.ability };

            NewCard.Add(name: ID,
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
                emissionTex: Utils.GetTextureFromPath(EmitTextureFile),
                abilities: abilities,
                decals: Utils.GetDecals());
        }
    }
}