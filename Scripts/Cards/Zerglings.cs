using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Zerglings
    {
        private const string DisplayName = "Zerglings";
        private const string Description = "Looks friendly but watch your face because your could lose it";
        private const string TextureFile = "Artwork/two_zergling.png";

        private const int BaseAttack = 1;
        private const int BaseHealth = 2;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            List<Ability> abilities = new List<Ability> {DoubleAttackAbility.ability};
            List<SpecialTriggeredAbility> specialAbilities = new List<SpecialTriggeredAbility> { ZerglingSpecialAbility.specialAbility };

            // Evolve into banelings
            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Banelings");
            EvolveIdentifier identifier = new EvolveIdentifier("Banelings", 1, new CardModificationInfo(cardInfo));

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
                abilities: abilities,
                specialAbilities: specialAbilities,
                evolveId: identifier,
                decals: Utils.GetDecals());
        }
    }
}