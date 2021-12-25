using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (SaveFile), "GetChoosableDeathcardMods", new System.Type[] {})]
    public class SaveFile_GetChoosableDeathcardMods
    {
        public static void Postfix(ref List<CardModificationInfo> __result)
        {
            Plugin.Log.LogInfo($"Adding {CustomDeathCard.DeathCards.Count} death cards");

            List<string> ownedIDs = new List<string>(); 
            foreach (CardInfo cardInfo in RunState.Run.playerDeck.Cards)
            {
                foreach (CardModificationInfo modificationInfo in cardInfo.Mods)
                {
                    if (modificationInfo is DeathCardModificationInfo deathCardModificationInfo)
                    {
                        ownedIDs.Add(deathCardModificationInfo.customCardId);
                    }
                }
            }
            
            foreach (CustomDeathCard x in CustomDeathCard.DeathCards)
            {
                if (ownedIDs.Contains(x.id))
                {
                    continue;
                }
                
                __result.Add(x.cardModificationInfo);
            }
        }
    }
}