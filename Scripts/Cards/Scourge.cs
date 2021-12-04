using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Scourge
    {
        private const string DisplayName = "Scourge";
        private const string Description = "Like little baby mutalisks. But uglier. And they smell way worse when they die.";
        private const string TextureFile = "Artwork/scourge.png";
        private const string EmitTextureFile = "Artwork/scourge_emit.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 1;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { ExplodeAbility.ability, Ability.Flying };

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