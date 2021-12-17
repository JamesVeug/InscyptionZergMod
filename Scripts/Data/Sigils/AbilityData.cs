using System;

namespace ZergMod.Scripts.Data
{
    [Serializable]
    public class AbilityData : AData
    {
        public int power = 0;
        public int priority;
        public string learnText = "";
    }
}