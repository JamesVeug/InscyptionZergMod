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
    public class BombardAbility : AbilityBehaviour
    {
        public override Ability Ability => ability;
        public static Ability ability;

        public static void Initialize()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 0;
            info.rulebookName = "Bombard";
            info.rulebookDescription = "When a card bearing this sigil deals damage directly it will damage the opposing card for 1 damage.";
            info.metaCategories = new List<AbilityMetaCategory>
                { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, "Artwork/bombard.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            NewAbility newAbility = new NewAbility(info, typeof(BombardAbility), tex,
                AbilityIdentifier.GetAbilityIdentifier(Plugin.PluginGuid, info.rulebookName));
            BombardAbility.ability = newAbility.ability;
        }

        public override bool RespondsToDealDamageDirectly(int amount)
        {
            PlayableCard opposingSlotCard = Card.slot.opposingSlot.Card;
            return opposingSlotCard != null && !opposingSlotCard.Dead && !opposingSlotCard.FaceDown;
        }

        public override IEnumerator OnDealDamageDirectly(int amount)
        {
            PlayableCard opposingSlotCard = Card.slot.opposingSlot.Card;
            
            
            yield return new WaitForSeconds(0.1f);
            yield return opposingSlotCard.TakeDamage(1, this.Card);
            yield return new WaitForSeconds(0.1f);
            
            yield return base.LearnAbility(0f);
        }
    }
}