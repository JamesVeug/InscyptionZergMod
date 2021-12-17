using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;

namespace ZergMod.Scripts.Cards
{
    public static class Overlord
    {
        private const string DisplayName = "Overlord";
        private const string Description = "Lazy alien balloon";
        private const string TextureFile = "Artwork/Cards/overlord.png";
        private const string EmitTextureFile = "Artwork/Cards/overlord_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 4;
        private const int BloodCost = 2;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> { Ability.Sacrificial, Ability.Reach };
	        
            // Evolve into Overseer
            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Overseer");
            EvolveIdentifier identifier = new EvolveIdentifier("Overseer", 1, new CardModificationInfo(cardInfo));

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
                evolveId:identifier,
                decals: Utils.GetDecals());
        }
    }
}