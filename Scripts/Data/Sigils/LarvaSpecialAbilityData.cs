using System;
using System.Collections.Generic;
using StarCraftCore.Scripts;
using StarCraftCore.Scripts.Data.Sigils;
using UnityEngine;

namespace ZergMod.Scripts.Data.Sigils
{
    [Serializable]
    public class LarvaSpecialAbilityData : SpecialAbilityData
    {
        [SerializeField]
        public List<WeightData> cardEvolutions = new List<WeightData>()
        {
            new WeightData()
            {
                weight = -1,
                cardName = ""
            }
        };

        public int turnsUntilEvolve;

        public int TotalWeights { get; private set; } 

        public override void OnPostLoad()
        {
            cardEvolutions.Sort((a,b)=>a.weight - b.weight);
            foreach (WeightData data in cardEvolutions)
            {
                TotalWeights += data.weight;
            }
        }
    }
}