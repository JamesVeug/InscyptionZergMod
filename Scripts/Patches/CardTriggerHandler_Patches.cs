using System;
using APIPlugin;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Patches
{
    [HarmonyPatch(typeof (CardTriggerHandler), "AddAbility", new System.Type[] {typeof (SpecialTriggeredAbility)})]
    public class CardTriggerHandler_AddAbility
    {
        public static bool Prefix(ref SpecialTriggeredAbility ability, CardTriggerHandler __instance)
        {
            //Plugin.Log.LogInfo("[CardTriggerHandler.AddAbility] " + ability);
            if (ability == SpecialTriggeredAbility.None)
            {
                //Plugin.Log.LogInfo("[CardTriggerHandler.AddAbility] Ability is None so replacing with Mirror");
                ability = SpecialTriggeredAbility.Mirror;
            }
            
            if (ability < SpecialTriggeredAbility.NUM_ABILITIES)
                return true;

            SpecialTriggeredAbility _ability = ability;
            var newSpecialAbility = NewSpecialAbility.specialAbilities.Find(x => x.specialTriggeredAbility == _ability); 
            if (newSpecialAbility != null)
            {
                //Plugin.Log.LogInfo("[CardTriggerHandler.AddAbility] Custom ability " + ability);
                SpecialCardBehaviour behaviour = Utils.AttachMonoBehaviour<SpecialCardBehaviour>(newSpecialAbility.abilityBehaviour, __instance.gameObject);
                __instance.specialAbilities.Add(new Tuple<SpecialTriggeredAbility, SpecialCardBehaviour>(ability, behaviour));
            }
            else
            {
                //Plugin.Log.Log(LogLevel.Warning, "[CardTriggerHandler.AddAbility] Special ability not found: " + ability + " for " + __instance.gameObject.name);
            }

            return false;
        }
    }
}