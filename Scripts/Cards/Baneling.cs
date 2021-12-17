using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Abilities;

namespace ZergMod.Scripts.Cards
{
    public static class Banelings
    {
        public const string ID = "Baneling";
        private const string DisplayName = "Baneling";
        private const string Description = "Those things are pretty nasty. Next time, you should try not to let them splash you!";
        private const string TextureFile = "Artwork/Cards/baneling.png";
        private const string EmitTextureFile = "Artwork/Cards/baneling_emit.png";

        private const int BaseAttack = 3;
        private const int BaseHealth = 1;
        private const int BloodCost = 0;
        private const int BoneCost = 2;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { ExplodeAbility.ability };

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
                abilities: abilities,
                decals: Utils.GetDecals());
        }
    }
}