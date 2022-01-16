using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;
using UnityEngine;
using Random = System.Random;

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

        public override IEnumerator OnSacrifice()
        {
            NextPrimalEvolution nextEvolution = GetNextEvolution();
            
            PlayableCard demandingCard = Singleton<BoardManager>.Instance.currentSacrificeDemandingCard;
            List<CardModificationInfo> mods = new List<CardModificationInfo>();

            CardInfo cardInfo;
            if (nextEvolution != null && !string.IsNullOrEmpty(nextEvolution.NextEvolutionName))
            {
                // Turn the card in the players hand into another card
                cardInfo = CardLoader.GetCardByName(nextEvolution.NextEvolutionName);
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
            }

            yield return new WaitForSeconds(0.2f);
            
            demandingCard.SetInfo(cardInfo);
            foreach (CardModificationInfo mod in mods)
            {
                demandingCard.AddTemporaryMod(mod);
            }
            demandingCard.Anim.StrongNegationEffect();
            
            yield return new WaitForSeconds(0.2f);
        }

        public IEnumerator OnOtherSacrificed(PlayableCard playableCard)
        {
            List<Ability> mods = new List<Ability>();
            foreach (Ability ability in playableCard.Info.abilities)
            {
                if (!mods.Contains(ability))
                {
                    mods.Add(ability);
                }
            }
            foreach (CardModificationInfo mod in playableCard.Info.Mods)
            {
                for (int i = 0; i < mod.abilities.Count; i++)
                {
                    if (!mods.Contains(mod.abilities[i]))
                    {
                        mods.Add(mod.abilities[i]);
                    }
                }
            }
            foreach (CardModificationInfo mod in playableCard.TemporaryMods)
            {
                for (int i = 0; i < mod.abilities.Count; i++)
                {
                    if (!mods.Contains(mod.abilities[i]))
                    {
                        mods.Add(mod.abilities[i]);
                    }
                }
            }

            if (mods.Count == 0)
            {
                yield break;
            }

            int random = UnityEngine.Random.Range(0, mods.Count);
            CardModificationInfo clone = new CardModificationInfo()
            {
                abilities = new List<Ability>()
                {
                    mods[random]
                }
            };
            
            PlayableCard.AddTemporaryMod(clone);
            PlayableCard.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);
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
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    clone.fromCardMerge = true;
                    mods.Add(clone);
                }
            }

            return mods;
        }
    }
}