﻿using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Overseer
    {
        private const string DisplayName = "Overseer";
        private const string Description = "The all seeing creature powered to reveal hidden creatures";
        private const string TextureFile = "Artwork/overseer.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 6;
        private const int BloodCost = 2;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            Texture2D tex = Utils.GetTextureFromPath(TextureFile);

            List<Ability> abilities = new List<Ability> { DetectorAbility.ability, Ability.DebuffEnemy };

            NewCard.Add(name: DisplayName,
                displayedName: DisplayName,
                baseAttack: BaseAttack,
                baseHealth: BaseHealth,
                metaCategories: metaCategories,
                cardComplexity: CardComplexity.Advanced,
                temple: CardTemple.Nature,
                description: Description,
                bloodCost: BloodCost,
                bonesCost: BoneCost,
                tribes: new List<Tribe> { Tribe.Insect },
                appearanceBehaviour: appearanceBehaviour,
                defaultTex: tex,
                abilities: abilities,
                decals: Utils.GetDecals());
        }
    }
}