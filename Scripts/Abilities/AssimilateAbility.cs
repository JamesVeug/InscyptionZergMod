﻿using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.Abilities
{
    public class AssimilateAbility : ACustomAbilityBehaviour<AssimilateAbility, AssimilateAbilityData>
    {
        public override Ability Ability => ability;
        public static Ability ability = Ability.None;
		
        public static void Initialize(Type declaringType)
        {
            ability = InitializeBase(declaringType);
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return target != null && target.Dead;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return base.OnDealDamage(amount, target);
            
            CardInfo evolution = this.GetTransformCardInfo();
            foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
            {
                CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                bool hasEvolve = clone.HasAbility(Ability.Evolve);
                if (hasEvolve)
                {
                    clone.abilities.Remove(Ability.Evolve);
                }
                bool hasAssimilate = clone.HasAbility(Ability);
                if (hasAssimilate)
                {
                    clone.abilities.Remove(Ability);
                }
                evolution.Mods.Add(clone);
            }
            
            yield return PreSuccessfulTriggerSequence();
            yield return Card.TransformIntoCard(evolution, RemoveTemporaryModsWithEvolve);
            yield return new WaitForSeconds(0.5f);
            yield return LearnAbility(0.5f);
        }
        
        protected virtual CardInfo GetTransformCardInfo()
        {
            return (Card.Info.evolveParams != null) ? (Card.Info.evolveParams.evolution.Clone() as CardInfo) : EvolveParams.GetDefaultEvolution(base.Card.Info);
        }
        private void RemoveTemporaryModsWithEvolve()
        {
            bool hasAbilities = Card.Info.Abilities.Contains(Ability.Evolve) || Card.Info.Abilities.Contains(Ability);
            if (hasAbilities)
            {
                CardModificationInfo temporaryEvolveMod = GetTemporaryEvolveMod();
                while (temporaryEvolveMod != null)
                {
                    Card.RemoveTemporaryMod(temporaryEvolveMod, true);
                    temporaryEvolveMod = GetTemporaryEvolveMod();
                }
            }
        }
        
        private CardModificationInfo GetTemporaryEvolveMod()
        {
            CardModificationInfo evolveMod = Card.TemporaryMods.Find((x) => x.abilities.Contains(Ability.Evolve));
            if (evolveMod != null)
            {
                return evolveMod;
            }
            
            CardModificationInfo assimilateMod = Card.TemporaryMods.Find((x) => x.abilities.Contains(Ability));
            return assimilateMod;
        }
    }
}