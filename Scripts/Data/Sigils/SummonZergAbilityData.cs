﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZergMod.Scripts.Data.Sigils
{
    [Serializable]
    public class WeightData
    {
        public string cardName = "";
        public int weight = 0;
    }
    
    [Serializable]
    public class SummonZergAbilityData : AbilityData
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