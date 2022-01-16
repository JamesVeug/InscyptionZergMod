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

        private int totalCardsSacrificed = 0;

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
            
            string dehaka = "Dehaka";
            foreach (CardModificationInfo mod in playableCard.Info.Mods)
            {
                if(mod.fromTotem) continue;
                if(mod.fromCardMerge) continue;
                if(mod.fromDuplicateMerge) continue;
                if(mod.fromLatch) continue;
                if(mod.fromOverclock) continue;

                for (int i = 0; i < mod.abilities.Count; i++)
                {
                    if (!mods.Contains(mod.abilities[i]))
                    {
                        mods.Add(mod.abilities[i]);
                    }
                }
            }
            /*foreach (CardModificationInfo mod in playableCard.TemporaryMods)
            {
                if(mod.fromTotem) continue;
                
                for (int i = 0; i < mod.abilities.Count; i++)
                {
                    if (!mods.Contains(mod.abilities[i]))
                    {
                        mods.Add(mod.abilities[i]);
                    }
                }
            }*/

            if (mods.Count == 0)
            {
                yield break;
            }

            int random = UnityEngine.Random.Range(0, mods.Count);
            Ability newAbility = mods[random];
            CardModificationInfo clone = new CardModificationInfo()
            {
                abilities = new List<Ability>()
                {
                    newAbility
                }
            };

            //Plugin.Log.LogInfo("[PrimalSpecialAbility]");
            
            bool isDehaka = PlayableCard.Info.name == dehaka;
            if (isDehaka)
            {
                //Plugin.Log.LogInfo("[PrimalSpecialAbility] Dehaka");
                clone.singletonId = dehaka + totalCardsSacrificed++;
                
                // Gross. What if we add a second?
                CardInfo cardInfo = PlayableCard.Info;
                List<CardModificationInfo> infoMods = cardInfo.Mods;
                RemoveDehakaMod(infoMods, clone.singletonId);
                
                cardInfo.Mods.Add(clone);
                PlayableCard.SetInfo(cardInfo);

                CardInfo info = RunState.Run.playerDeck.Cards.Find((a) => a.name == dehaka);
                if (info != null)
                {
                    //Plugin.Log.LogInfo("[PrimalSpecialAbility] Updating deck");
                    List<CardModificationInfo> modificationInfos = info.Mods;
                    RemoveDehakaMod(modificationInfos, clone.singletonId);
                    modificationInfos.Add((CardModificationInfo)clone.Clone());
                    RunState.Run.playerDeck.UpdateModDictionary();
                }
                else
                {
                    //Plugin.Log.LogInfo("[PrimalSpecialAbility] Dehaka not in deck");
                }
            }
            else
            {
                //Plugin.Log.LogInfo("[PrimalSpecialAbility] Not Dehaka " + PlayableCard.Info.name);
                PlayableCard.AddTemporaryMod(clone);
            }
            
            PlayableCard.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);
            //Plugin.Log.LogInfo("[PrimalSpecialAbility] Done");
        }

        private static void RemoveDehakaMod(List<CardModificationInfo> infoMods, string cloneSingletonId)
        {
            CardModificationInfo dehakaAbility = infoMods.Find((a) => a.singletonId == cloneSingletonId);
            if (dehakaAbility != null)
            {
                infoMods.Remove(dehakaAbility);
            }
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

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return !PlayableCard.Dead;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            totalCardsSacrificed = 0;
            yield break;
        }
    }
}