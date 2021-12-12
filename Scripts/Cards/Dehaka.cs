using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Dehaka
    {
        private const string DisplayName = "Dehaka";
        private const string Description = "Collects, Kills, takes the essence of its prey.";
        private const string TextureFile = "Artwork/Cards/dehaka_1.png";
        private const string EmitTextureFile = "Artwork/Cards/dehaka_1_emit.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 2;
        private const int BloodCost = 2;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability> { RegenerateAbility.ability };
            List<SpecialTriggeredAbility> specialAbilities = new List<SpecialTriggeredAbility> { DehakaSpecialAbility.specialAbility };

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
                specialAbilities: specialAbilities,
                decals: Utils.GetDecals(),
                onePerDeck:true);
        }
    }
}