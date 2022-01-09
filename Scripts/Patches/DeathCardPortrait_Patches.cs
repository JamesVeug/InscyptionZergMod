using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ZergMod.Scripts.Patches
{
    [HarmonyPatch(typeof (DeathCardPortrait), "DisplayFace", new System.Type[] {typeof (CompositeFigurine.FigurineType), typeof(int), typeof(int), typeof(bool)})]
    public class DeathCardPortrait_DisplayFace
    {
        public static bool Prefix(DeathCardPortrait __instance, ref CompositeFigurine.FigurineType headType, int mouthIndex, int eyesIndex, bool lostEye)
        {
            // Head
            if (headType < 0 || headType == CompositeFigurine.FigurineType.NUM_FIGURINES)
            {
                __instance.head.enabled = false;
            }
            else
            {
                if (headType < CompositeFigurine.FigurineType.NUM_FIGURINES)
                {
                    __instance.head.sprite = ResourceBank.Get<Sprite>("Art/Cards/DeathcardPortraits/deathcard_head_" + headType.ToString().ToLowerInvariant());
                }
                else
                {
                    __instance.head.sprite = CustomDeathCard.HeadSpriteLookup[headType];
                }
                __instance.head.enabled = true;
            }

            // Eyes
            if (eyesIndex < 0)
            {
                __instance.eyes.enabled = false;
            }
            else
            {
                if (eyesIndex < DeathCardPortrait.NUM_EYES)
                {
                    __instance.eyes.sprite =
                        ResourceBank.Get<Sprite>("Art/Cards/DeathcardPortraits/deathcard_eyes_" +
                                                 (eyesIndex + 1).ToString());
                    __instance.eyesEmission.sprite = ResourceBank.Get<Sprite>(
                        "Art/Cards/DeathcardPortraits/deathcard_eyes_" + (eyesIndex + 1).ToString() + "_emission");
                }
                else
                {
                    __instance.eyes.sprite = CustomDeathCard.EyesSpriteLookup[eyesIndex];
                    __instance.eyesEmission.sprite = CustomDeathCard.EyesEmissionSpriteLookup[eyesIndex];
                }
                __instance.eyes.enabled = true;
            }

            // Mouth
            if (mouthIndex < 0 || mouthIndex == DeathCardPortrait.NUM_MOUTHS)
            {
                __instance.mouth.enabled = false;
            }
            else
            {
                if (mouthIndex < DeathCardPortrait.NUM_MOUTHS)
                {
                    __instance.mouth.sprite = ResourceBank.Get<Sprite>("Art/Cards/DeathcardPortraits/deathcard_mouth_" +
                                                                       (mouthIndex + 1).ToString());
                }
                else
                {
                    __instance.mouth.sprite = CustomDeathCard.MouthSpriteLookup[mouthIndex];
                }
                __instance.mouth.enabled = true;
            }

            // Dunno what this is for but it's here
            __instance.eyeBlocker.SetActive(lostEye);
            
            return false;
        }
    }
    
    [HarmonyPatch(typeof (DeathCardPortrait), "ApplyCardInfo", new System.Type[] {typeof (CardInfo)})]
    public class DeathCardPortrait_ApplyCardInfo
    {
        public static void Postfix(DeathCardPortrait __instance, CardInfo card)
        {
            Transform transform = __instance.transform.Find("Body");
            if (transform != null)
            {
                if (transform.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    if (card.portraitTex != null && !card.portraitTex.name.Contains("adder"))
                    {
                        spriteRenderer.sprite = card.portraitTex;
                    }
                }
            }
        }
    }
}