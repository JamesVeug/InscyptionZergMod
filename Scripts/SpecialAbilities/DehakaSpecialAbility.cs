using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DiskCardGame;
using InscryptionAPI.Helpers;
using InscryptionAPI.Saves;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class DehakaSpecialAbility : ACustomSpecialAbilityBehaviour<DehakaSpecialAbilityData>, IPortraitChanges
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;

        public static int SavedDehakaKills
        {
            get => ModdedSaveManager.SaveData.GetValueAsInt(Plugin.PluginGuid, "DehakaKills");
            set => ModdedSaveManager.SaveData.SetValue(Plugin.PluginGuid, "DehakaKills", value);
        }
        
        private static Dictionary<int, Sprite> m_dehakaImages = new Dictionary<int, Sprite>();
        private static int m_minDehakakillsForImages = int.MaxValue;
        private static int m_maxDehakakillsForImages = int.MinValue;

        private bool m_activated = false;

        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(declaringType);
            
            foreach (DehakaSpecialAbilityData.PortraitChangeData data in LoadedData.portraitChanges)
            {
                InitializeTexture(data.minimumKills, data.portraitPath, data.portraitEmitPath);
            }
        }

        private static void InitializeTexture(int kills, string fileName, string emissiveFileName)
        {
            byte[] imgBytes = File.ReadAllBytes(Path.Combine(ZergMod.Plugin.Directory, fileName));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);
            tex.name = "portrait_" + fileName;
            tex.filterMode = FilterMode.Point;

            Sprite sprite = tex.ConvertTexture(TextureHelper.SpriteType.CardPortrait, FilterMode.Point);

            byte[] emissiveImgBytes = File.ReadAllBytes(Path.Combine(ZergMod.Plugin.Directory, emissiveFileName));
            Texture2D emissiveTex = new Texture2D(2,2);
            emissiveTex.LoadImage(emissiveImgBytes);
            emissiveTex.name = tex.name + "_emission";
            emissiveTex.filterMode = FilterMode.Point;
            sprite.RegisterEmissionForSprite(emissiveTex, TextureHelper.SpriteType.CardPortrait, FilterMode.Point);
            
            m_dehakaImages[kills] = sprite;
            m_minDehakakillsForImages = Mathf.Min(m_minDehakakillsForImages, kills);
            m_maxDehakakillsForImages = Mathf.Max(m_maxDehakakillsForImages, kills);
        }

        private void Awake()
        {
            // Change portrait when the game starts and the ability is first added to the card
            RefreshPortrait();
        }

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            if (m_activated || !target.Dead)
            {
                return false;
            }

            if (target.Info.Health >= Card.Info.Health)
            {
                return true;
            }
            if (target.Info.Attack >= Card.Info.Attack)
            {
                return true;
            }

            return false;
        }
        
        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            m_activated = true;
            SavedDehakaKills = SavedDehakaKills + 1;

            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);
            base.Card.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
            RefreshPortrait();
            yield return new WaitForSeconds(0.5f);
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return m_activated;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            m_activated = false;
            yield break;
        }

        public override bool RespondsToDrawn()
        {
            return ShouldRefreshPortrait();
        }

        public override IEnumerator OnDrawn()
        {
            RefreshPortrait();
            yield return null;
        }

        public bool ShouldRefreshPortrait()
        {
            return PlayableCard != null && !PlayableCard.Dead;
        }

        private Sprite GetCurrentKillsPortrait(out int portraitKills)
        {
            int kills = SavedDehakaKills;
            Sprite tex = null;
            int max = int.MinValue;
            foreach (KeyValuePair<int,Sprite> pair in m_dehakaImages)
            {
                int imageMinKills = pair.Key;
                if (imageMinKills <= kills && imageMinKills >= max)
                {
                    max = imageMinKills;
                    tex = pair.Value;
                }
            }

            portraitKills = max;
            return tex;
        }
        
        public void RefreshPortrait()
        {
            Sprite tex = GetCurrentKillsPortrait(out int kills);
            if (tex == null)
            {
                return;
            }

            Card.Info.portraitTex = tex;
            Card.Info.portraitTex.name = tex.name;
            Card.Info.alternatePortrait = tex;
            Card.Info.alternatePortrait.name = tex.name;
            Card.RenderCard();
        }
    }
}