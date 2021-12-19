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

        public override bool RespondsToPlayFromHand()
        {
            return Card.Info.HasAbility(Ability.Evolve);
        }

        public override IEnumerator OnPlayFromHand()
        {
            EvolveParams infoEvolveParams = Card.Info.evolveParams;
            if (infoEvolveParams == null)
            {
                infoEvolveParams = new EvolveParams();
                infoEvolveParams.turnsToEvolve = LoadedData.turnsUntilEvolve;
                Card.Info.evolveParams = infoEvolveParams;
            }
            infoEvolveParams.evolution = GetRandomCard();
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