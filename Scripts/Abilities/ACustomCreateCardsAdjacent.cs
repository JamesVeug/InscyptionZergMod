using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public abstract class ACustomCreateCardsAdjacent<Y, T> : CreateCardsAdjacent where T : CreateCardsAbilityData where Y : CreateCardsAdjacent
    {
        public T LoadedData => m_loadedData ?? (m_loadedData = (T)Utils.s_dataLookup[typeof(Y)]);
        protected T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;
        public override string SpawnedCardId
        {
            get
            {
                Plugin.Log.LogInfo("[ACustomCreateCardsAdjacent] " + LoadedData.name + " " + LoadedData.spawnCardId + " " + Ability);
                return LoadedData.spawnCardId;
            }
        }

        public override string CannotSpawnDialogue => LoadedData.cannotSpawnDialogue;

        protected static Ability InitializeBase(Type declaringType)
        {
            Utils.InitializeAbility<T>(declaringType, out NewAbility newAbility);
            return newAbility.ability;
        }
    }
}