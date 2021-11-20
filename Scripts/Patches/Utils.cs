using System;
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
    }
}