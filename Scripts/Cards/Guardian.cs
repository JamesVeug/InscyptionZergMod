using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;

namespace ZergMod.Scripts.Cards
{
    public static class Guardian
    {
        public const string ID = "Guardian";
        private const string DisplayName = "Guardian";
        private const string Description = "Despite being devastating in terrestrial combat, guardians are next to useless in space combat";
        private const string TextureFile = "Artwork/Cards/guardian.png";
        private const string EmitTextureFile = "Artwork/Cards/guardian_emit.png";

        private const int BaseAttack = 2;
        private const int BaseHealth = 6;
        private const int BloodCost = 3;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { Ability.Sniper };

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