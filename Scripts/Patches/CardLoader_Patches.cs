using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (CardLoader), "CreateDeathCard", new System.Type[] {typeof (CardModificationInfo)})]
    public class CardLoader_CreateDeathCard
    {
        public static void Postfix(CardModificationInfo deathCardMod, ref CardInfo __result)
        {
            if (!(deathCardMod is DeathCardModificationInfo cardModificationInfo))
            {
                return;
            }
            
            foreach (CustomDeathCard deathCard in CustomDeathCard.DeathCards)
            {
                if (deathCard.id != cardModificationInfo.customCardId) 
                    continue;
                
                // Tail
                if (deathCard.tailIdentifier != null)
                {
                    __result.tailParams = new TailParams()
                    {
                        tail = (CardInfo)deathCard.tailIdentifier.tail.Clone(),
                        tailLostPortrait = deathCard.tailIdentifier.tailLostPortrait
                    };
                }

                // Base Portrait image
                if (deathCard.baseIndexOverride >= 0)
                {
                    if (CustomDeathCard.BaseLookup.TryGetValue(deathCard.baseIndexOverride, out CustomDeathCardBase cardBase))
                    {
                        Plugin.Log.LogInfo($"Overriding base card image for {deathCard.name} Card with {cardBase.Sprite.name}");
                        __result.portraitTex = cardBase.Sprite;
                    }
                }
                    
                return;
            }
            __result.portraitTex = ResourceBank.Get<Sprite>("Art/Cards/DeathcardPortraits/deathcard_base");
        }
    }
}