using System;

namespace ZergMod.Scripts.Data
{
    [Serializable]
    public abstract class AData
    {
        public string name = "";

        public virtual void OnPostLoad()
        {
            
        }
    }
}