using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class PrimalSpecialAbility : ACustomSpecialAbilityBehaviour<PrimalSpecialAbilityData>
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;

        private bool triggered = false;

        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(declaringType);
        }

        public override bool RespondsToSacrifice()
        {
            Plugin.Log.LogInfo("[RespondsToSacrifice]");
            if (triggered)
            {
                return false;
            }
            
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

            Plugin.Log.LogInfo("[RespondsToSacrifice] Responding");
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
        
        public override IEnumerator OnSacrifice()
        {
            Plugin.Log.LogInfo("[OnSacrifice] " + this.PlayableCard.Info.name);
            triggered = true;
            NextPrimalEvolution nextEvolution = GetNextEvolution();
            
            PlayableCard demandingCard = Singleton<BoardManager>.Instance.currentSacrificeDemandingCard;
            CardSlot slot = Utils.GetSlot(demandingCard);
            int slotIndex = slot != null ? slot.Index : -1;
            Plugin.Log.LogInfo("[OnSacrifice] With " + demandingCard.Info.name + " slot " + slotIndex);

            CardInfo evolvedCard = null;
            if (nextEvolution != null && !string.IsNullOrEmpty(nextEvolution.NextEvolutionName))
            {
                Plugin.Log.LogInfo("[OnSacrifice] Next Evolution: " + nextEvolution.NextEvolutionName);
                evolvedCard = CardLoader.GetCardByName(nextEvolution.NextEvolutionName);
            }
            else
            {
                // No next evolution. Let them stack though for fun
                CardInfo info = (CardInfo)demandingCard.Info.Clone();
                Plugin.Log.LogInfo($"[OnSacrifice] No next Evolution {info.baseAttack} => {info.baseAttack + 1}");
                info.baseAttack += 1;
                info.baseHealth += 2;

                info.name = "Primal " + info.name;
                evolvedCard = info;
            }

            List<CardModificationInfo> cardModificationInfos = new List<CardModificationInfo>();
            foreach (CardModificationInfo mod in demandingCard.Info.Mods)
            {
                cardModificationInfos.Add((CardModificationInfo)mod.Clone());
            }
            
            evolvedCard.mods.AddRange(cardModificationInfos);
            
            demandingCard.SetInfo(evolvedCard);
            Plugin.Log.LogInfo("[OnSacrifice] Done " + demandingCard.Info.name);
            yield return null;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            triggered = false;
            yield return null;
        }
    }
}