using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class InfestedTerran
    {
        private const string DisplayName = "Infested Terran";
        private const string Description = "Must be a result of a space virus infesting a human turning them Zerg.";
        private const string TextureFile = "Artwork/Cards/infested_terran.png";
        private const string EmitTextureFile = "Artwork/Cards/infested_terran_emit.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 2;
        private const int BloodCost = 0;
        private const int BoneCost = 2;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { Ability.Brittle, Ability.DrawCopy };

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
                emissionTex: Utils.GetTextureFromPath(EmitTextureFile),
                abilities: abilities,
                decals: Utils.GetDecals());
        }
    }
}