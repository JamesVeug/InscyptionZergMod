using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Abilities;

namespace ZergMod.Scripts.Cards
{
    public static class SwarmHost
    {
        public const string ID = "Swarm Host";
        private const string DisplayName = "Swarm Host";
        private const string Description = "Those free units are nothing but a distraction";
        private const string TextureFile = "Artwork/Cards/swarmhost.png";
        private const string EmitTextureFile = "Artwork/Cards/swarmhost_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 2;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> {SpawnLocustAbility.ability, Ability.Submerge};

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