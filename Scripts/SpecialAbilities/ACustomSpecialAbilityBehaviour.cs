using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.SpecialAbilities
{
    public abstract class ACustomSpecialAbilityBehaviour<T> : SpecialCardBehaviour where T : SpecialAbilityData
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;
        
        public static T LoadedData => m_loadedData;
        private static T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;

        public static void Initialize(Type declaringType)
        {
            Utils.InitializeSpecialAbility(declaringType, out m_loadedData, out NewSpecialAbility newSpecialAbility);
            specialAbility = newSpecialAbility.specialTriggeredAbility;
        }
    }
}