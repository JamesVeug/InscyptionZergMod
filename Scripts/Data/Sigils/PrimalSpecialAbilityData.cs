using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZergMod.Scripts.Data.Sigils
{
    [Serializable]
    public class NextPrimalEvolution
    {
        public string Name = "";
        public string NextEvolutionName = "";
    }
    
    [Serializable]
    public class PrimalSpecialAbilityData : SpecialAbilityData
    {
        [SerializeField]
        public List<NextPrimalEvolution> evolutions = new List<NextPrimalEvolution>(){};

        public int DefaultAttackBuff;
        public int DefaultHealthBuff;
    }
}