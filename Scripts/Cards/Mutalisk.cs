﻿using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Mutalisk
    {
        private const string DisplayName = "Mutalisk";
        private const string Description = "Flying devil";
        private const string TextureFile = "Artwork/mutalisk.png";

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

            Texture2D tex = Utils.GetTextureFromPath(TextureFile);
            Texture2D decal = Utils.GetTextureFromPath(Plugin.DecalPath);


            List<Ability> abilities = new List<Ability> { Ability.Flying, HealAbility.ability };

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
                tex:tex, 
                abilities:abilities,
                decals:new List<Texture>{decal});
        }
    }
}