using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Abilities;

namespace ZergMod.Scripts.Cards
{
    public static class Roach
    {
        public const string ID = "Roach";
        private const string DisplayName = "Roach";
        private const string Description = "Armored unit that can heal";
        private const string TextureFile = "Artwork/Cards/roach.png";
        private const string EmitTextureFile = "Artwork/Cards/roach_emit.png";

        private const int BaseAttack = 2;
        private const int BaseHealth = 3;
        private const int BloodCost = 2;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { RegenerateAbility.ability };

            // Evolve into banelings
            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Ravager");
            EvolveIdentifier identifier = new EvolveIdentifier("Ravager", 1, new CardModificationInfo(cardInfo));

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
                evolveId:identifier,
                decals: Utils.GetDecals());
        }
    }
}