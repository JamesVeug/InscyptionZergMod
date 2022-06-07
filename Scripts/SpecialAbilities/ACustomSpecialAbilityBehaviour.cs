using System;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
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
            SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility newSpecialAbility;
            Utils.InitializeSpecialAbility(declaringType, out m_loadedData, out newSpecialAbility);
            return newSpecialAbility.Id;
        }
    }
}