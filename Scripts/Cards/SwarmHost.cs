using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ZergMod.Scripts.Cards
{
    public static class SwarmHost
    {
        private const string DisplayName = "Swarm Host";
        private const string Description = "Those free units are nothing but an distraction";
        private const string TextureFile = "Artwork/swarm_host.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 2;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            Texture2D tex = Utils.GetTextureFromPath(TextureFile);
            Texture2D decal = Utils.GetTextureFromPath(Plugin.DecalPath);

	        
            List<Ability> abilities = new List<Ability> {Draw2LocustsAbility.ability, Ability.Submerge};

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
                decals:new List<Texture>{decal});
        }
    }
}