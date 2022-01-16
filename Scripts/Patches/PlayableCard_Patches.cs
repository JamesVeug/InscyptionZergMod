using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using ZergMod.Scripts.Abilities;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (PlayableCard), "Sacrifice")]
    public class PlayableCard_Sacrifice
    {
        public static bool Prefix(PlayableCard __instance, ref IEnumerator __result)
        {
            if (__instance.HasAbility(BloodBankAbility.ability))
            {
                __result = SacrificeOverride(__instance);
                return false;
            }

            return true;
        }

        private static IEnumerator SacrificeOverride(PlayableCard __instance)
        {
            __instance.Anim.SetSacrificeHoverMarkerShown(false);
            __instance.Anim.SetMarkedForSacrifice(false);
            __instance.Anim.PlaySacrificeParticles();
            ProgressionData.SetAbilityLearned(Ability.Sacrificial);
            if (__instance.TriggerHandler.RespondsToTrigger(Trigger.Sacrifice, Array.Empty<object>()))
            {
                yield return __instance.TriggerHandler.OnTrigger(Trigger.Sacrifice, Array.Empty<object>());
            }

            PlayableCard currentSacrificeDemandingCard = Singleton<BoardManager>.Instance.currentSacrificeDemandingCard;
            int totalBloodRequired = currentSacrificeDemandingCard.Info.BloodCost;

            List<CardSlot> currentSacrifices = Singleton<BoardManager>.Instance.currentSacrifices;
            int totalBloodSacrificed = 0;
            for (int i = 0; i < currentSacrifices.Count; i++)
            {
                PlayableCard playableCard = currentSacrifices[i].Card;
                if (playableCard == null)
                {
                    continue;
                }
                
                if(playableCard.HasAbility(BloodBankAbility.ability))
                {
                    int totalHealthToGive = Mathf.Clamp(playableCard.Health, 0, totalBloodRequired - totalBloodSacrificed);
                    totalBloodSacrificed += totalHealthToGive;
                    if (playableCard == __instance)
                    {
                        if (__instance.Health <= totalBloodRequired)
                        {
                            yield return __instance.Die(true, null, true);
                        }
                        else
                        {
                            __instance.Status.damageTaken += totalHealthToGive;
                        }
                        break;
                    }
                }
                else
                {
                    totalBloodSacrificed += playableCard.HasAbility(Ability.TripleBlood) ? 3 : 1;
                }
            }
        }
    }
    
    [HarmonyPatch(typeof (PlayableCard), "TransformIntoCard", new System.Type[] {typeof(CardInfo), typeof(Action)})]
    public class PlayableCard_TransformIntoCard
    {
        public static IEnumerator Postfix(IEnumerator result, PlayableCard __instance, CardInfo evolvedInfo, Action onTransformedCallback = null)
        {
            yield return result;
            if (__instance.HasAbility(DetectorAbility.ability))
            {
                DetectorAbility detectorAbility = __instance.GetComponent<DetectorAbility>();
                if (detectorAbility.ShouldRevealCards())
                {
                    yield return detectorAbility.RevealSubmurgedCards();
                }
            }
        }
    }
}