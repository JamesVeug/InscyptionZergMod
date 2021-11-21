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

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory,TextureFile));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);
	        
            List<Ability> abilities = new List<Ability> {DoubleAttackAbility.ability};
            List<SpecialTriggeredAbility> specialAbilities = new List<SpecialTriggeredAbility> { ZerglingSpecialAbility.specialAbility };

            // Evolve into banelings
            CardInfo cardInfo = NewCard.cards.Find(info => info.displayedName == "Banelings");
            EvolveIdentifier identifier = new EvolveIdentifier("Banelings", 1, new CardModificationInfo(cardInfo));

            NewCard.Add(DisplayName, metaCategories, CardComplexity.Simple, CardTemple.Nature, DisplayName,
                BaseAttack,
                BaseHealth,
                description: Description,
                cost: BloodCost,
                bonesCost: BoneCost,
                tribes: new List<Tribe> { Tribe.Insect },
                appearanceBehaviour: appearanceBehaviour,
                tex: tex,
                abilities: abilities,
                specialAbilities: specialAbilities,
                evolveId:identifier);
        }
    }
}