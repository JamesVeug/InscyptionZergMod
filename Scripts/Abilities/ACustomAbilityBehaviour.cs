using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public abstract class ACustomAbilityBehaviour<T> : AbilityBehaviour where T : AbilityData
    {
        public static T LoadedData => m_loadedData;
        private static T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;

        protected static Ability InitializeBase(Type declaringType)
        {
            Utils.InitializeAbility(declaringType, out m_loadedData, out NewAbility newAbility);
            return newAbility.ability;
        }
    }
}