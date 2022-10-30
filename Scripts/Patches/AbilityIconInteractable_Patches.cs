using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Helpers;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    /*[HarmonyPatch(typeof(CardTriggerHandler), "AddReceiverToGameObject",new System.Type[] {typeof(string), typeof(GameObject)})]
    public class CardTriggerHandler_AddReceiverToGameObject
    {
        public static bool Prefix<T>(CardTriggerHandler __instance, string typeString, GameObject obj, ref Color __result)
        {
            Plugin.Log.LogInfo("[CardTriggerHandler_AddReceiverToGameObject] " + typeString + " " + obj.name);
            return true;
        }
    }*/
}