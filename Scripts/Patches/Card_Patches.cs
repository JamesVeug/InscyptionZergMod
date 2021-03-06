using APIPlugin;
using DiskCardGame;
using HarmonyLib;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (Card), "AttachAbilities", new System.Type[] {typeof (CardInfo)})]
    public class Card_AttachAbilities
    {
        public static bool Prefix(CardInfo info, Card __instance)
        {
            foreach (SpecialTriggeredAbility specialTriggeredAbility in info.SpecialAbilities)
            {
                bool customSpecialAbility = false;
                foreach (NewSpecialAbility ability in NewSpecialAbility.specialAbilities)
                {
                    if (specialTriggeredAbility == ability.specialTriggeredAbility)
                    {
                        Utils.AttachMonoBehaviour<SpecialCardBehaviour>(ability.abilityBehaviour, __instance.gameObject);
                        customSpecialAbility = true;
                        break;
                    }
                }

                if (!customSpecialAbility)
                {
                    CardTriggerHandler.AddReceiverToGameObject<SpecialCardBehaviour>(specialTriggeredAbility.ToString(), __instance.gameObject);
                }
            }

            return false;
        }
    }
    
    [HarmonyPatch(typeof (Card), "SetInfo", new System.Type[] {typeof (CardInfo)})]
    public class Card_SetInfo
    {
        public static void Postfix(CardInfo info, Card __instance)
        {
            // Fixes Zerglings portrait not changing from 2 to 4 when buffing the health at the campfire
            foreach (IPortraitChanges portraitChanges in __instance.gameObject.GetComponents<IPortraitChanges>())
            {
                if (portraitChanges.ShouldRefreshPortrait())
                {
                    portraitChanges.RefreshPortrait();
                }
            }
        }
    }
}