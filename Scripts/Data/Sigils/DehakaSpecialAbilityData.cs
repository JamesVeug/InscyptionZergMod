using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZergMod.Scripts.Data.Sigils
{

    
    [Serializable]
    public class DehakaSpecialAbilityData : SpecialAbilityData
    {
        [Serializable]
        public class PortraitChangeData
        {
            public int minimumKills = 0;
            public string portraitPath = "Artwork/Cards/dehaka_1.png";
            public string portraitEmitPath = "Artwork/Cards/dehaka_1_emit.png";
        }
        
        [SerializeField]
        public List<PortraitChangeData> portraitChanges = new List<PortraitChangeData>()
        {
            new PortraitChangeData()
        };
    }
}