using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace ZergMod.Patches
{
    [HarmonyPatch(typeof (BoardManager), "GetValueOfSacrifices")]
    public class BoardManager_GetValueOfSacrifices
    {
        public static void Postfix(List<CardSlot> sacrifices, ref BoardManager __instance, ref int __result)
        {
            int num = __result;
            foreach (CardSlot cardSlot in sacrifices)
            {
                if (cardSlot != null && cardSlot.Card != null)
                {
                    if (cardSlot.Card.HasAbility(BloodBankAbility.ability))
                    {
                        ProgressionData.SetAbilityLearned(BloodBankAbility.ability);
                        num += cardSlot.Card.Health;
                        num -= cardSlot.Card.HasAbility(Ability.TripleBlood) ? 3 : 1; // Because original method already treated this as 1
                    }
                }
            }

            __result = num;
        }
    }
    
    [HarmonyPatch(typeof (BoardManager), "SacrificesCreateRoomForCard")]
    public class BoardManager_SacrificesCreateRoomForCard
    {
        public static bool Prefix(PlayableCard card, List<CardSlot> sacrifices, ref BoardManager __instance, ref bool __result)
        {
            bool result = false;
            foreach (CardSlot cardSlot in __instance.PlayerSlotsCopy)
            {
                if (cardSlot.Card == null)
                {
                    result = true;
                    break;
                }

                if (card.Info.BloodCost < 1) continue;
                if (!sacrifices.Contains(cardSlot)) continue;
                if (cardSlot.Card.HasAbility(Ability.Sacrificial)) continue;
                if (!cardSlot.Card.CanBeSacrificed) continue;
                if (cardSlot.Card.HasAbility(BloodBankAbility.ability) && cardSlot.Card.Health > card.Info.BloodCost) continue;
                
                result = true;
                break;
            }
            
            __result = result;
            return false;
        }
    }
}