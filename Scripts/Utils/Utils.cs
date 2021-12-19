using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data;

namespace ZergMod
{
    public static class Utils
    {
        private static List<Texture> s_defaultDecals = null;
        
        public static void InitializeAbility<T>(Type declaringType, out T loadedData, out NewAbility newAbility) where T : AbilityData
        {
            loadedData = DataUtil.LoadFromFile<T>("Data/Sigils/" + declaringType.Name);
            
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = loadedData.power;
            info.rulebookName = loadedData.ruleBookName;
            info.rulebookDescription = loadedData.ruleDescription;
            info.metaCategories = loadedData.metaCategories;

            if (!string.IsNullOrEmpty(loadedData.learnText))
            {
                List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
                DialogueEvent.Line line = new DialogueEvent.Line();
                line.text = loadedData.learnText;
                lines.Add(line);
                info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);
            }

            newAbility = new NewAbility(
                info: info, 
                abilityBehaviour: declaringType, 
                tex: Utils.GetTextureFromPath(loadedData.iconPath),
                id: AbilityIdentifier.GetID(Plugin.PluginGuid, loadedData.name)
            );
        }
        
        public static void InitializeSpecialAbility<T>(Type declaringType, out T loadedData, out NewSpecialAbility newSpecialAbility) where T : SpecialAbilityData
        {
            loadedData = DataUtil.LoadFromFile<T>("Data/SpecialAbilities/" + declaringType.Name);
            
            SpecialAbilityIdentifier identifier = SpecialAbilityIdentifier.GetID(Plugin.PluginGuid, loadedData.name);
            
            StatIconInfo iconInfo = new StatIconInfo();
            iconInfo.rulebookName = loadedData.ruleBookName;
            iconInfo.rulebookDescription = loadedData.ruleDescription;
            iconInfo.iconType = loadedData.iconType;
            iconInfo.iconGraphic = GetTextureFromPath(loadedData.iconPath);
            iconInfo.metaCategories = loadedData.metaCategories;
            
            newSpecialAbility = new NewSpecialAbility(declaringType, identifier, iconInfo);
        }

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
        
        /// <summary>
        /// Returns a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivatePropertyValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            PropertyInfo pi = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            return (T)pi.GetValue(obj, null);
        }

        /// <summary>
        /// Returns a private Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <returns>PropertyValue</returns>
        public static T GetPrivateFieldValue<T>(this object obj, string propName)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            return (T)fi.GetValue(obj);
        }

        /// <summary>
        /// Sets a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is set</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">Value to set.</param>
        /// <returns>PropertyValue</returns>
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            Type t = obj.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }

        /// <summary>
        /// Set a private Property Value on a given Object. Uses Reflection.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is returned</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">the value to set</param>
        /// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
        public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            fi.SetValue(obj, val);
        }
    }
}