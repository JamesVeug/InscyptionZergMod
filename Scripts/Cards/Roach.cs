﻿using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Roach
    {
        private const string DisplayName = "Roach";
        private const string Description = "Armored unit that can heal";
        private const string TextureFile = "Artwork/roach.png";

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

            Texture2D tex = Utils.GetTextureFromPath(TextureFile);

            List<Ability> abilities = new List<Ability> { HealAbility.ability };

            // Evolve into banelings
            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Ravager");
            EvolveIdentifier identifier = new EvolveIdentifier("Ravager", 1, new CardModificationInfo(cardInfo));

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
                defaultTex: tex,
                abilities: abilities,
                evolveId:identifier,
                decals: Utils.GetDecals());
        }
    }
}