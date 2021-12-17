﻿using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;

namespace ZergMod.Scripts.Cards
{
    public static class Broodling
    {
        private const string DisplayName = "Broodling";
        private const string Description = "";
        private const string TextureFile = "Artwork/Cards/broodling.png";

        private const int BaseAttack = 2;
        private const int BaseHealth = 2;
        private const int BloodCost = 0;
        private const int BoneCost = 1;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { Ability.Brittle };

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
                decals: Utils.GetDecals());
        }
    }
}