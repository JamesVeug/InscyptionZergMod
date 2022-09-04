using System;
using StarCraftCore.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Data.Sigils
{
    [Serializable]
    public class BroodLordSpecialAbilityData : SpecialAbilityData
    {
        public string cardCreatedName = "Broodling";
        public int maxCards = 3;
    }
}