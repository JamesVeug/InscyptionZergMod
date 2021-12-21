using System;
using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public class RegestateAbility : ACustomAbilityBehaviour<RegestateAbility, RegestateAbilityData>
    {
        public override Ability Ability => ability;
        public static Ability ability = Ability.None;
		
        public static void Initialize(Type declaringType)
        {
            ability = InitializeBase(declaringType);
        }
        
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            if (wasSacrifice) 
                return false;
            
            bool isADeathCard = Card.Info.mods.Find((x)=>x.deathCardInfo != null) != null;
            if (isADeathCard)
                return false;
            
            return true;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            
            CardInfo whatToMutateInto = CardLoader.GetCardByName(this.Card.Info.name);
            int totalHealth = this.Card.Health + this.Card.Status.damageTaken;
            int totalEvolves = Mathf.Clamp(Mathf.FloorToInt((float)totalHealth / LoadedData.healthPerEvolutions), 1, LoadedData.maxEvolutions) + 1;
            
            CardInfo egg = (CardInfo)NewCard.cards.Find(info => info.displayedName == LoadedData.cardName).Clone();
            egg.baseHealth = totalHealth;
            egg.abilities = new List<Ability> { Ability.Evolve };
            egg.evolveParams = new EvolveParams{turnsToEvolve = totalEvolves, evolution = whatToMutateInto};
            
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(egg, Card.slot, 0.15f, true);

            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}