using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZergMod.Scripts.Data
{
    [Serializable]
    public class ZerglingSpecialAbilityData : SpecialAbilityData
    {
        [Serializable]
        public class PortraitChangeData
        {
            public int health = 0;
            public string portraitPath = "Artwork/Cards/zergling_1.png";
            public string portraitEmitPath = "Artwork/Cards/dehaka_1_emit.png";
        }
        
        [SerializeField]
        public List<PortraitChangeData> portraitChanges = new List<PortraitChangeData>()
        {
            new PortraitChangeData()
        };
    }
}