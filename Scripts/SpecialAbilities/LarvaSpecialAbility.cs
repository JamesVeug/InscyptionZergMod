using System;
using System.Collections;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;
using Random = UnityEngine.Random;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class LarvaSpecialAbility : ACustomSpecialAbilityBehaviour<LarvaSpecialAbilityData>
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;
        
        // First value = Weight for choice. Higher means larger chance to evolve into it
        // Second value = Name of what card to evolve into
        private static int m_totalWeights = 0;

        private bool m_randomized = false;
        
        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(declaringType);
            
            // Sort by ascending drop rates
            LoadedData.cardEvolutions.Sort((a,b)=>a.weight - b.weight);
            foreach (LarvaSpecialAbilityData.WeightData data in LoadedData.cardEvolutions)
            {
                m_totalWeights += data.weight;
            }
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
                evolution = GetRandomCard()
            };
            Card.SetInfo(cardInfo);
            
            m_randomized = true;
            yield return null;
        }
        
        private CardInfo GetRandomCard()
        {
            int expectedWeight = Random.Range(0, m_totalWeights);
            int currentWeight = 0;
            foreach (LarvaSpecialAbilityData.WeightData data in LoadedData.cardEvolutions)
            {
                currentWeight += data.weight;
                if (currentWeight >= expectedWeight)
                {
                    return CardLoader.GetCardByName(data.cardName);
                }
            }

            return CardLoader.GetCardByName(LoadedData.cardEvolutions[LoadedData.cardEvolutions.Count - 1].cardName);
        }
    }
}