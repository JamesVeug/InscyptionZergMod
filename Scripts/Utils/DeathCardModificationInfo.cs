using System.Collections.Generic;
using DiskCardGame;

namespace ZergMod
{
    public class DeathCardModificationInfo : CardModificationInfo
    {
        public string customCardId = "";
        
        public new object Clone()
        {
            object clone = base.Clone();
            if (clone is DeathCardModificationInfo cardModificationInfo)
            {
                Plugin.Log.LogInfo("[DeathCardModificationInfo] cardModificationInfo.customCardName");
            }
            else
            {
                Plugin.Log.LogInfo("[DeathCardModificationInfo] did not close to DeathCardModificationInfo");
            }
            
            return clone;
        }
    }
}