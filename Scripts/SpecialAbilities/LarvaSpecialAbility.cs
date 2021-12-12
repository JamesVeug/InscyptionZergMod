using System;
using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using ZergMod;
using ZergMod.Scripts.Cards;
using Plugin = ZergMod.Plugin;
using Random = UnityEngine.Random;

namespace DiskCardGame
{
    public class LarvaSpecialAbility : SpecialCardBehaviour
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;
        
        // First value = Weight for choice. Higher means larger chance to evolve into it
        // Second value = Name of what card to evolve into
        private static int m_totalWeights = 0; 
        private static List<Tuple<int, string>> m_dropDates = new List<Tuple<int, string>>()
        {
            new Tuple<int, string>(40, Zerglings.ID),
            new Tuple<int, string>(20, Roach.ID),
            new Tuple<int, string>(20, Drone.ID),
            new Tuple<int, string>(20, Scourge.ID),
            new Tuple<int, string>(10, Mutalisk.ID),
            new Tuple<int, string>(10, Hydralisk.ID),
            new Tuple<int, string>(10, Corruptor.ID),
            new Tuple<int, string>(5, Banelings.ID),
            new Tuple<int, string>(1, Ultralisk.ID),
        };

        public static void Initialize()
        {
            // Sort by ascending drop rates
            m_dropDates.Sort((a,b)=>a.Item1 - b.Item1);
            foreach (Tuple<int,string> data in m_dropDates)
            {
                m_totalWeights += data.Item1;
            }

            SpecialAbilityIdentifier identifier = SpecialAbilityIdentifier.GetID(Plugin.PluginGuid, "LarvaSpecialAbility");
            
            StatIconInfo iconInfo = new StatIconInfo();
            iconInfo.rulebookName = "Larva";
            iconInfo.rulebookDescription = "When a Larva evolves it will transform into a random Zerg card";
            iconInfo.iconType = SpecialStatIcon.CardsInHand;
            iconInfo.iconGraphic = Utils.GetTextureFromPath("Artwork/Cards/larva.png");
            iconInfo.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook };
            
            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(LarvaSpecialAbility), identifier, iconInfo);
            specialAbility = newSpecialAbility.specialTriggeredAbility;
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
                infoEvolveParams.turnsToEvolve = 1;
                Card.Info.evolveParams = infoEvolveParams;
            }
            infoEvolveParams.evolution = GetRandomCard();
            yield return null;
        }
        
        private CardInfo GetRandomCard()
        {
            int expectedWeight = Random.Range(0, m_totalWeights);
            int currentWeight = 0;
            foreach (Tuple<int,string> data in m_dropDates)
            {
                currentWeight += data.Item1;
                if (currentWeight >= expectedWeight)
                {
                    return CardLoader.GetCardByName(data.Item2);
                }
            }

            return CardLoader.GetCardByName(m_dropDates[m_dropDates.Count - 1].Item2);
        }
    }
}