﻿using System;
using System.Collections;
using DiskCardGame;
using StarCraftCore.Scripts.SpecialAbilities;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class LarvaSpecialAbility : ACustomSpecialAbilityBehaviour<LarvaSpecialAbilityData>
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;

        private bool m_randomized = false;
        
        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
        }

        public override bool RespondsToDrawn()
        {
            return ShouldRandomize();
        }

        public override IEnumerator OnDrawn()
        {
            // Drawn in hand
            yield return Randomize();
        }

        public override bool RespondsToResolveOnBoard()
        {
            // Placed on board by a spell
            return ShouldRandomize();
        }

        public override IEnumerator OnResolveOnBoard()
        {
            yield return Randomize();
        }

        private bool ShouldRandomize()
        {
            return !m_randomized && Card.Info.HasAbility(Ability.Evolve);
        }

        private IEnumerator Randomize()
        {
            CardInfo cardInfo = Card.Info;
            cardInfo.evolveParams = new EvolveParams
            {
                turnsToEvolve = LoadedData.turnsUntilEvolve,
                evolution = StarCraftCore.Utils.GetRandomWeightedCard(LoadedData.cardEvolutions, LoadedData.TotalWeights)
            };
            Card.SetInfo(cardInfo);
            
            m_randomized = true;
            yield return null;
        }
    }
}