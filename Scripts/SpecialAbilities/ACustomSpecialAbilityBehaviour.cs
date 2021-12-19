using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.SpecialAbilities
{
    public abstract class ACustomSpecialAbilityBehaviour<T> : SpecialCardBehaviour where T : SpecialAbilityData
    {
        public static T LoadedData => m_loadedData;
        private static T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;

        public static SpecialTriggeredAbility InitializeBase(Type declaringType)
        {
            Utils.InitializeSpecialAbility(declaringType, out m_loadedData, out NewSpecialAbility newSpecialAbility);
            return newSpecialAbility.specialTriggeredAbility;
        }
    }
}