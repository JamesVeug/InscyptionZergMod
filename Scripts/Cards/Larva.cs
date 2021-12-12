using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Larva
    {
        public const string ID = "Larva";
        private const string DisplayName = "Larva";
        private const string Description = "It's hard carapace. Not recommended for consumption ";
        private const string TextureFile = "Artwork/Cards/larva.png";
        private const string EmitTextureFile = "Artwork/Cards/larva_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 3;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Trait> traits = new List<Trait> { Trait.KillsSurvivors };
            List<SpecialTriggeredAbility> specialTriggeredAbilities = new List<SpecialTriggeredAbility> { LarvaSpecialAbility.specialAbility };
            List<Ability> abilities = new List<Ability> { Ability.Evolve };
            
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
                traits: traits,
                specialAbilities: specialTriggeredAbilities,
                abilities: abilities,
                onePerDeck:true,
                decals: Utils.GetDecals());
        }
    }
}