using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;

namespace ZergMod.Scripts.Cards
{
    public static class Hydralisk
    {
        public const string ID = "Hydralisk";
        private const string DisplayName = "Hydralisk";
        private const string Description = "Great for taking out flyers";
        private const string TextureFile = "Artwork/Cards/hydralisk.png";
        private const string EmitTextureFile = "Artwork/Cards/hydralisk_emit.png";

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

            List<Ability> abilities = new List<Ability> { Ability.Sniper };

            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Lurker");
            EvolveIdentifier identifier = new EvolveIdentifier("Lurker", 1, new CardModificationInfo(cardInfo));

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
                evolveId:identifier,
                abilities:abilities,
                decals: Utils.GetDecals());
        }
    }
}