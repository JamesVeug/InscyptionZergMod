using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public abstract class ACustomCreateCardsAdjacent<T> : CreateCardsAdjacent where T : CreateCardsAbilityData
    {
        public static T LoadedData => m_loadedData;
        private static T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;
        public override string SpawnedCardId => LoadedData.spawnCardId;
        public override string CannotSpawnDialogue => LoadedData.cannotSpawnDialogue;

        protected static Ability InitializeBase(Type declaringType)
        {
            Utils.InitializeAbility(declaringType, out m_loadedData, out NewAbility newAbility);
            return newAbility.ability;
        }
    }
}