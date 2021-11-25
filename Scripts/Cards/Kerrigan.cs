using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Kerrigan
    {
        private const string DisplayName = "Queen of Blades";
        private const string Description = "The Hero that never dies";
        private const string TextureFile = "Artwork/kerrigan.png";

        private const int BaseAttack = 4;
        private const int BaseHealth = 4;
        private const int BloodCost = 3;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            Texture2D tex = Utils.GetTextureFromPath(TextureFile);

            List<Ability> abilities = new List<Ability> { RegestateAbility.ability };

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
                decals: Utils.GetDecals(),
                onePerDeck:true);
        }
    }
}