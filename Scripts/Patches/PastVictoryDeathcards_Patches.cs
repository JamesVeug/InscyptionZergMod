using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (PastVictoryDeathcards), "Start", new System.Type[] {})]
    public class PastVictoryDeathcards_Start
    {
        public static bool Prefix(ref PastVictoryDeathcards __instance)
        {
            if (SaveManager.SaveFile.pastRuns != null)
            {
                int num = 0;
                for (var i = 0; i < SaveManager.SaveFile.pastRuns.Count && num < __instance.victoryDeathcards.Count; i++)
                {
                    RunState runState = SaveManager.SaveFile.pastRuns[i];
                    if (runState != null && runState.completed)
                    {
                        /*if (CustomSaveManager.SaveFile.DeathCardRunSaveData.TryGetValue(i, out var deathCardData))
                        {
                            InitializeDeathCard(__instance.victoryDeathcards[num], deathCardData, runState);
                        }
                        else*/
                        {
                            // Initialize like normal
                            __instance.InitializeDeathcard(__instance.victoryDeathcards[num], runState.deathcardName,
                                runState.playerAvatarHead, runState.eyeState == EyeballState.Missing);
                        }

                        num++;
                    }
                }
            }

            return false;
        }

        /*private static void InitializeDeathCard(SelectableCard deathcard, CustomDeathCardSaveData data, RunState runState)
        {
            string name = runState.deathcardName;
            int BaseIndex = data.BaseIndex;
            int HeadIndex = data.HeadIndex;
            int EyesIndex = data.EyesIndex;
            int MouthIndex = data.MouthIndex;
            bool eyeState = runState.eyeState == EyeballState.Missing;
        
            deathcard.gameObject.SetActive(true);
            CardInfo cardByName = CardLoader.GetCardByName("!DEATHCARD_VICTORY");
            CardModificationInfo cardModificationInfo = new CardModificationInfo();
            cardModificationInfo.deathCardInfo = new DeathCardInfo((CompositeFigurine.FigurineType)HeadIndex, eyeState);
            cardModificationInfo.deathCardInfo.eyesIndex = EyesIndex;
            cardModificationInfo.deathCardInfo.mouthIndex = MouthIndex;
            cardModificationInfo.nameReplacement = name;
            
            if(BaseIndex >= 0)
            {
                if (CustomDeathCard.BaseLookup.TryGetValue(BaseIndex, out CustomDeathCardBase cardBase))
                {
                    deathcard.RenderInfo.portraitOverride = cardBase.Sprite;
                    Plugin.Log.LogInfo($"PastVictoryDeathcards_Start.Overriding base card image for {name} Card with {cardBase.Sprite.name}");
                }
            }
            
            cardByName.Mods.Add(cardModificationInfo);
            deathcard.RenderInfo.nameOverride = name;
            deathcard.RenderInfo.hiddenAttack = (deathcard.RenderInfo.hiddenHealth = true);
            deathcard.SetInfo(cardByName);
            deathcard.SetEnabled(false);
            deathcard.SetInteractionEnabled(false);
        }*/
    }
}