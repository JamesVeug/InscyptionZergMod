using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (LeshyBossOpponent), "CreateUsableDeathcards", new System.Type[] {})]
    public class LeshyBossOpponent_CreateUsableDeathcards
    {
        public static void Postfix(ref List<CardInfo> __result)
        {
            foreach (CustomDeathCard deathCard in CustomDeathCard.DeathCards)
            {
                CardModificationInfo cardModificationInfo = deathCard.cardModificationInfo;
                if (!cardModificationInfo.abilities.Exists((Ability x) => !AbilitiesUtil.GetInfo(x).opponentUsable))
                {
                    CardInfo item = CardLoader.CreateDeathCard(cardModificationInfo);
                    __result.Add(item);
                }
            }
        }
    }
}