using System.Collections.Generic;
using System.IO;
using DiskCardGame;
using UnityEngine;

namespace ZergMod
{
    public static class Utils
    {
        private static List<Texture> s_defaultDecals = null;
        
        public static Texture2D GetTextureFromPath(string path)
        {
            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, path));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);

            return tex;
        }
        public static List<Texture> GetDecals()
        {
            if (s_defaultDecals == null)
            {
                Texture2D decal = Utils.GetTextureFromPath(Plugin.DecalPath);
                s_defaultDecals = new List<Texture> { decal };
            }
            
            return s_defaultDecals;
        }
        
        public static void PrintHierarchy(GameObject go)
        {
            List<Transform> hierarchy = new List<Transform>();

            Transform t = go.transform;
            while (t != null)
            {
                hierarchy.Add(t);
                t = t.parent;
            }

            string prefix = "";
            for (int i = hierarchy.Count - 1; i >= 0; i--)
            {
                Transform tran = hierarchy[i];
                string text = prefix + tran.gameObject.name + "(" + tran.gameObject.GetInstanceID() + ")";
                Plugin.Log.LogInfo(text);

                prefix += "\t";
            }
        }
        
        /// <summary>
        /// Some cards do not have Card.Slot assigned. So this is a work around
        /// </summary>
        public static CardSlot GetSlot(PlayableCard cardToGetSlot)
        {
            if (cardToGetSlot.Slot != null)
            {
                //Plugin.Log.LogInfo("[SplashDamageAbility][GetSlot] Slot cached");
                return cardToGetSlot.Slot;
            }

            CardSlot cardSlot = cardToGetSlot.transform.parent.GetComponent<CardSlot>();
            if (cardSlot != null)
            {
                //Plugin.Log.LogInfo("[SplashDamageAbility][GetSlot] Found slot in parent");
                return cardSlot;
            }

            int cardToGetSlotID = cardToGetSlot.gameObject.GetInstanceID();
            Plugin.Log.LogInfo("[SplashDamageAbility][GetSlot] Getting slot for " + cardToGetSlotID);
            
            List<CardSlot> allSlots = new List<CardSlot>();
            allSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
            allSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(true));

            for (int i = 0; i < allSlots.Count; i++)
            {
                CardSlot slot = allSlots[i];
                if (slot.Index != 2)
                {
                    continue;
                }
                
                PlayableCard card = slot.Card;
                if (card == null)
                    continue;
                
                //Plugin.Log.LogInfo("[SplashDamageAbility][GetSlot] Slot " + slot.Index + " has " + card.Info.displayedName + " from queue: " + card.OriginatedFromQueue);
                if (card.gameObject == cardToGetSlot.gameObject)
                {
                    //Plugin.Log.LogInfo("[SplashDamageAbility][GetSlot] Card is in slot " + slot.Index);
                    return slot;
                }
                else
                {
                    int slotCardID = card.gameObject.GetInstanceID();
                    //Plugin.Log.LogInfo("[SplashDamageAbility][GetSlot] " + cardToGetSlotID + " != " + slotCardID);
                }
            }

            Plugin.Log.LogInfo("[SplashDamageAbility][GetSlot] Could not find slot for " + cardToGetSlotID);
            return null;
        }
        
        public static List<CardSlot> GetAdjacentSlots(CardSlot slot)
        {
            int slotIndex = slot.Index;
            List<CardSlot> allSlots = Singleton<BoardManager>.Instance.GetSlots(!slot.opposingSlot);
            List<CardSlot> slots = new List<CardSlot>();
            for (int i = 0; i < allSlots.Count; i++)
            {
                CardSlot cardSlot = allSlots[i];
                if (cardSlot == null || cardSlot.Card == null || cardSlot.Card.Dead || cardSlot.Card.FaceDown)
                {
                    continue;
                }
                
                if (cardSlot.Index == slotIndex - 1 || cardSlot.Index == slotIndex + 1)
                {
                    slots.Add(cardSlot);
                }
            }

            return slots;
        }
    }
}