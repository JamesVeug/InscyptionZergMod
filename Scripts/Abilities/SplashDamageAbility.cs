using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod
{
    public class SplashDamageAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        private bool activated = false;

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Splash Damage";
            info.rulebookDescription = "When a card bearing this sigil deals damage it will also hit the adjacent cards";
            info.metaCategories = new List<AbilityMetaCategory>
                { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/splash_damage.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info, typeof(SplashDamageAbility), tex,
                AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            SplashDamageAbility.ability = newAbility.ability;
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return !Card.Dead && !activated;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();

            activated = true;
            
            CardSlot cardSlot = target.slot;
            List<CardSlot> adjacentSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(cardSlot);
            foreach (CardSlot slot in adjacentSlots)
            {
                if (slot.Card != null && !slot.Card.Dead)
                {
                    yield return slot.Card.TakeDamage(amount, Card);
                }
            }

            activated = false;

            yield return base.LearnAbility(0.0f);
        }
    }
}