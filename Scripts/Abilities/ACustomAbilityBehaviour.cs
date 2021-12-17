using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public abstract class ACustomAbilityBehaviour<T> : AbilityBehaviour where T : AbilityData
    {
        public override Ability Ability => ability;
        public static Ability ability = Ability.None;
        
        public static T LoadedData => m_loadedData;
        private static T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;

        public static void Initialize(Type declaringType)
        {
            Utils.InitializeAbility(declaringType, out m_loadedData, out NewAbility newAbility);
            ability = newAbility.ability;
        }
    }
}