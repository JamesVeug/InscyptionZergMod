using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof(ConsumableItemSlot), "CreateItem", new System.Type[] { typeof(ItemData), typeof(bool) })]
    public class ConsumableItemSlot_CreateItem
    {
        public static void Postfix(ConsumableItemSlot __instance)
        {
            if (__instance.Item.Data.name == "BiomassInABottle")
            {
                CardBottleItem bottle = __instance.Item as CardBottleItem;
                if (bottle != null)
                {
                    CardInfo cardByName = CardLoader.GetCardByName("Zerg_JSON_biomass");
                    bottle.cardInfo = cardByName;
                    bottle.GetComponentInChildren<SelectableCard>().SetInfo(cardByName);
                    bottle.gameObject.GetComponent<AssignCardOnStart>().enabled = false;
                }
            }
        }
    }
}