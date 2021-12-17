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
            Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Blood banked! " + __instance);
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
            Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Total blood to sacrifice! " + totalBloodRequired);

            List<CardSlot> currentSacrifices = Singleton<BoardManager>.Instance.currentSacrifices;
            int totalBloodSacrificed = 0;
            Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Total sacrificing: " + currentSacrifices.Count);
            for (int i = 0; i < currentSacrifices.Count; i++)
            {
                Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Index: " + i);
                PlayableCard playableCard = currentSacrifices[i].Card;
                if (playableCard == null)
                {
                    Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Already dead/destroyed?");
                    continue;
                }
                
                Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Getting blood available for: " + playableCard);
                Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Getting blood available for: " + playableCard.Info.displayedName);
                if(playableCard.HasAbility(BloodBankAbility.ability))
                {
                    Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Found Blood Bank! " + totalBloodSacrificed + " already sacced");
                    int totalHealthToGive = Mathf.Clamp(playableCard.Health, 0, totalBloodRequired - totalBloodSacrificed);
                    totalBloodSacrificed += totalHealthToGive;
                    if (playableCard == __instance)
                    {
                        Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Found Card with Blood Bank!");
                        if (__instance.Health <= totalBloodRequired)
                        {
                            Plugin.Log.LogInfo("[PlayableCard_Sacrificed] Killing card. " + __instance.Health + " < " + totalBloodRequired);
                            yield return __instance.Die(true, null, true);
                        }
                        else
                        {
                            Plugin.Log.LogInfo("[PlayableCard_Sacrificed] Applying damage card. " + totalHealthToGive);
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

        /*public static MethodBase TargetMethod => AccessTools.Property(typeof(PlayableCard), nameof(PlayableCard.Sacrifice)).GetMethod;
        
        static IEnumerator CustomSacrifice(PlayableCard __instance)
        {
            Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Transpilered! " + __instance);
            
            __instance.Anim.PlaySacrificeSound();
            if (__instance.HasAbility(BloodBankAbility.ability))
            {
                Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Blood banked! " + __instance);
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
                Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Total blood to sacrifice! " + totalBloodRequired);

                List<CardSlot> currentSacrifices = Singleton<BoardManager>.Instance.currentSacrifices;
                int totalBloodSacrificed = 0;
                for (int i = 0; i < currentSacrifices.Count; i++)
                {
                    PlayableCard playableCard = currentSacrifices[i].Card;
                    if(playableCard.HasAbility(BloodBankAbility.ability))
                    {
                        Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Found Blood Bank! " + totalBloodSacrificed + " already sacced");
                        int totalHealthToGive = Mathf.Clamp(playableCard.Health, 0, totalBloodRequired - totalBloodSacrificed);
                        totalBloodSacrificed += totalHealthToGive;
                        if (playableCard == __instance)
                        {
                            Plugin.Log.LogInfo("[PlayableCard_Sacrifice] Found Card with Blood Bank!");
                            if (__instance.Health <= totalBloodRequired)
                            {
                                Plugin.Log.LogInfo("[PlayableCard_Sacrificed] Killing card. " + __instance.Health + " < " + totalBloodRequired);
                                yield return __instance.Die(true, null, true);
                            }
                            else
                            {
                                Plugin.Log.LogInfo("[PlayableCard_Sacrificed] Applying damage card. " + totalHealthToGive);
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
            else if (__instance.HasAbility(Ability.Sacrificial))
            {
                __instance.Anim.SetSacrificeHoverMarkerShown(false);
                __instance.Anim.SetMarkedForSacrifice(false);
                __instance.Anim.PlaySacrificeParticles();
                ProgressionData.SetAbilityLearned(Ability.Sacrificial);
                if (__instance.TriggerHandler.RespondsToTrigger(Trigger.Sacrifice, Array.Empty<object>()))
                {
                    yield return __instance.TriggerHandler.OnTrigger(Trigger.Sacrifice, Array.Empty<object>());
                }
            }
            else
            {
                __instance.Anim.DeactivateSacrificeHoverMarker();
                if (__instance.TriggerHandler.RespondsToTrigger(Trigger.Sacrifice, Array.Empty<object>()))
                {
                    yield return __instance.TriggerHandler.OnTrigger(Trigger.Sacrifice, Array.Empty<object>());
                }
                yield return __instance.Die(true, null, true);
            }
            yield break;
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            return new[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch), nameof(CustomSacrifice), new[]{typeof(PlayableCard)})),
                new CodeInstruction(OpCodes.Ret)
            };
        }*/
    }
}