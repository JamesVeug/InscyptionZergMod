using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Leviathan
    {
        private const string DisplayName = "Leviathan";
        private const string Description = "Heavy air transport unit";
        private const string TextureFile = "Artwork/Cards/leviathan.png";
        private const string EmitTextureFile = "Artwork/Cards/leviathan_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 20;
        private const int BloodCost = 0;
        private const int BoneCost = 8;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability> { BloodBankAbility.ability, SummonZergAbility.ability };

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
                defaultTex: Utils.GetTextureFromPath(TextureFile),
                emissionTex: Utils.GetTextureFromPath(EmitTextureFile),
                abilities: abilities,
                decals: Utils.GetDecals(),
                onePerDeck:true);
        }
    }
}