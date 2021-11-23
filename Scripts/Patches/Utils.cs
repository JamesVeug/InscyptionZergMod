using System;
using System.Collections.Generic;
using APIPlugin;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Patches
{
    public static class Utils
    {
        public static T AttachMonoBehaviour<T>(Type type, GameObject gameObject)
        {
            SpecialCardBehaviour t = gameObject.GetComponent(type) as SpecialCardBehaviour;
            if (t == null)
            {
                return (T)(object)gameObject.AddComponent(type);
            }
            else
            {
                return (T)(object)t;
            }
        }
        
        public static bool OpponentHasADetector(PlayableCard card)
        {
            List<CardSlot> opponentSlots = Singleton<BoardManager>.Instance.GetSlots(card.OpponentCard);
            foreach (CardSlot slot in opponentSlots)
            {
                if (slot != null && slot.Card != null && slot.Card.HasAbility(DetectorAbility.ability))
                {
                    return true;
                }
            }

            return false;
        }
    }
}