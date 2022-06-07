using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public abstract class ACustomAbilityBehaviour<Y, T> : AbilityBehaviour where T : AbilityData where Y : AbilityBehaviour
    {
        public T LoadedData => m_loadedData ?? (m_loadedData = (T)Utils.s_dataLookup[typeof(Y)]);
        protected T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;

        protected static Ability InitializeBase(Type declaringType)
        {
            Utils.InitializeAbility<T>(declaringType, out AbilityInfo newAbility);
            return newAbility.ability;
        }
    }
}