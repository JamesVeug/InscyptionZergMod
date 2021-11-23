using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class Overlord
    {
        private const string DisplayName = "Overlord";
        private const string Description = "Lazy alien balloon";
        private const string TextureFile = "Artwork/overlord.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 4;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();

            Texture2D tex = Utils.GetTextureFromPath(TextureFile);

            List<Ability> abilities = new List<Ability> { Ability.Reach, Ability.Evolve };
	        
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
                defaultTex: tex,
                abilities: abilities,
                evolveId:identifier,
                decals: Utils.GetDecals());
        }
    }
}