using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public abstract class ACustomStrafe<Y, T> : Strafe where T : StrafeAbilityData where Y : Strafe
    {
        public T LoadedData => m_loadedData ?? (m_loadedData = (T)Utils.s_dataLookup[typeof(Y)]);
        protected T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;
        
        protected static Ability InitializeBase(Type declaringType)
        {
            Utils.InitializeAbility<T>(declaringType, out NewAbility newAbility);
            return newAbility.ability;
        }
    }
}