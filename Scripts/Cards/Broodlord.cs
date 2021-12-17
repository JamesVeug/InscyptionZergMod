using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Abilities;

namespace ZergMod.Scripts.Cards
{
    public static class Broodlord
    {
        public const string ID = "Brood lord";
        private const string DisplayName = "Brood lord";
        private const string Description = "Heavy Airborn unit at its max evolution stage";
        private const string TextureFile = "Artwork/Cards/broodlord.png";
        private const string EmitTextureFile = "Artwork/Cards/broodlord_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 6;
        private const int BloodCost = 3;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability> { SwarmSeedsAbility.ability };

            NewCard.Add(name: ID,
                displayedName: DisplayName,
                baseAttack: BaseAttack,
                baseHealth: BaseHealth,
                metaCategories: metaCategories,
                cardComplexity: CardComplexity.Intermediate,
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