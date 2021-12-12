using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Backgrounds;

namespace ZergMod.Scripts.Cards
{
    public static class XelNagaArtifact
    {
        private const string ID = "Xel'naga Artifact 1";
        private const string DisplayName = "Strange Artifact";
        private const string Description = "What the hell is that?";
        private const string TextureFile = "Artwork/Cards/xelnaga_artifact.png";
        private const string EmitTextureFile = "Artwork/Cards/xelnaga_artifact_emit.png";

        private const int BaseAttack = 0;
        private const int BaseHealth = 1;
        private const int BloodCost = 1;
        private const int BoneCost = 0;

        public static void Initialize()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(XelNagaBackground.CustomAppearance);
            
            List<Ability> abilities = new List<Ability>{ };
            abilities.Add(Ability.QuadrupleBones);
            abilities.Add(Ability.TripleBlood);
            
            List<Trait> traits = new List<Trait> { Trait.Goat };

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
                abilities: abilities,
                appearanceBehaviour: appearanceBehaviour,
                defaultTex: Utils.GetTextureFromPath(TextureFile),
                emissionTex: Utils.GetTextureFromPath(EmitTextureFile),
                traits: traits,
                decals: Utils.GetDecals(),
                onePerDeck:true);
        }
    }
}