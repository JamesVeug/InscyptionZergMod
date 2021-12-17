using System;
using APIPlugin;
using DiskCardGame;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.Abilities
{
    public abstract class ACustomCreateCardsAdjacent<T> : CreateCardsAdjacent where T : CreateCardsAbilityData
    {
        public override Ability Ability => ability;
        public static Ability ability = Ability.None;
        
        public static T LoadedData => m_loadedData;
        private static T m_loadedData = null;
        
        public override int Priority => LoadedData.priority;
        public override string SpawnedCardId => LoadedData.spawnCardId;
        public override string CannotSpawnDialogue => LoadedData.cannotSpawnDialogue;

        public static void Initialize(Type declaringType)
        {
            Utils.InitializeAbility(declaringType, out m_loadedData, out NewAbility newAbility);
            ability = newAbility.ability;
        }
    }
}