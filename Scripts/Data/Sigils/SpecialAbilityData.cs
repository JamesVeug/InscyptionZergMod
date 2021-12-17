using System;
using DiskCardGame;

namespace ZergMod.Scripts.Data
{
    [Serializable]
    public class SpecialAbilityData : AbilityData
    {
        public SpecialStatIcon iconType = SpecialStatIcon.CardsInHand;
    }
}