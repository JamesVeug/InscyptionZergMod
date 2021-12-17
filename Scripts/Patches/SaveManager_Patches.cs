using HarmonyLib;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (SaveManager), "SaveToFile", new System.Type[] {typeof (bool)})]
    public class SaveManager_SaveToFile
    {
        public static void Postfix()
        {
            float secondsSinceLastSave = Time.time - SaveManager.lastSaveTime;
            if (secondsSinceLastSave < 1)
            {
                CustomSaveManager.SaveToFile();
            }
            else
            {
                Plugin.Log.LogInfo("[SaveManager_SaveToFile] Did not save CustomSave data. Save must have failed. " + secondsSinceLastSave);
            }
        }
    }
}