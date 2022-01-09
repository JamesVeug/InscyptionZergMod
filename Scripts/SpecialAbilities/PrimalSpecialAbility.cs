using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;
using UnityEngine;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class PrimalSpecialAbility : ACustomSpecialAbilityBehaviour<PrimalSpecialAbilityData>
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;

        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(declaringType);
        }

        public override bool RespondsToSacrifice()
        {
            if (PlayableCard == null)
            {
                return false;
            }
            
            PlayableCard demandingCard = Singleton<BoardManager>.Instance.currentSacrificeDemandingCard;
            if (demandingCard == null ||
                demandingCard.name != PlayableCard.name ||
                !demandingCard.Info.specialAbilities.Contains(specialAbility) || 
                demandingCard == PlayableCard)
            {
                return false;
            }

            return true;
        }

        private NextPrimalEvolution GetNextEvolution()
        {
            for (int i = 0; i < LoadedData.evolutions.Count; i++)
            {
                if (LoadedData.evolutions[i].Name == this.PlayableCard.Info.name)
                {
                    return LoadedData.evolutions[i];
                }
            }

            return null;
        }

        private List<CardModificationInfo> GetMods(PlayableCard card)
        {
            List<CardModificationInfo> mods = new List<CardModificationInfo>();
            foreach (CardModificationInfo mod in card.Info.Mods)
            {
                // Only transfer abilities
                if (mod.abilities.Count > 0)
                {
                    /*foreach (Ability ability in mod.abilities)
                    {
                        Plugin.Log.LogInfo("[OnSacrifice][Mod] " + (int)ability + " " + ability.ToString());
                    }*/

                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    clone.fromCardMerge = true;
                    mods.Add(clone);
                }
            }
            
            foreach (CardModificationInfo mod in card.TemporaryMods)
            {
                // Only transfer abilities
                if (mod.abilities.Count > 0)
                {
                    /*foreach (Ability ability in mod.abilities)
                    {
                        Plugin.Log.LogInfo("[OnSacrifice][Mod] " + (int)ability + " " + ability.ToString());
                    }*/

                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    clone.fromCardMerge = true;
                    mods.Add(clone);
                }
            }

            return mods;
        }
        
        public override IEnumerator OnSacrifice()
        {
            NextPrimalEvolution nextEvolution = GetNextEvolution();
            
            PlayableCard demandingCard = Singleton<BoardManager>.Instance.currentSacrificeDemandingCard;
            List<CardModificationInfo> mods = GetMods(PlayableCard);

            CardInfo cardInfo;
            if (nextEvolution != null && !string.IsNullOrEmpty(nextEvolution.NextEvolutionName))
            {
                // Turn the card in the players hand into another card
                cardInfo = CardLoader.GetCardByName(nextEvolution.NextEvolutionName);
                //Plugin.Log.LogInfo("[OnSacrifice][Mod] New evolution");

                mods.AddRange(GetMods(demandingCard));
            }
            else
            {
                // No next evolution. Let them stack though for fun
                cardInfo = demandingCard.Info;
                
                CardModificationInfo mod = new CardModificationInfo();
                mod.attackAdjustment = LoadedData.DefaultAttackBuff;
                mod.healthAdjustment = LoadedData.DefaultHealthBuff;
                mods.Add(mod);
                //Plugin.Log.LogInfo("[OnSacrifice][Mod] Upgraded");
            }

            yield return new WaitForSeconds(0.2f);
            
            foreach (CardModificationInfo mod in mods)
            {
                demandingCard.AddTemporaryMod(mod);
            }
            demandingCard.SetInfo(cardInfo);
            demandingCard.Anim.StrongNegationEffect();
            
            yield return new WaitForSeconds(0.2f);
        }
    }
}