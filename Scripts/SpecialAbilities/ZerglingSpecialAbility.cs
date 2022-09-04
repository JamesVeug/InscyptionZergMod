using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DiskCardGame;
using InscryptionAPI.Helpers;
using StarCraftCore;
using StarCraftCore.Scripts.SpecialAbilities;
using UnityEngine;
using ZergMod.Scripts.Data.Sigils;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class ZerglingSpecialAbility : ACustomSpecialAbilityBehaviour<ZerglingSpecialAbilityData>, IPortraitChanges
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbility.None;
        
        private static Dictionary<int, Sprite> m_zerglingImages = new Dictionary<int, Sprite>();
        private static int m_maxZerglingHealth = 0;

        public static void Initialize(Type declaringType)
        {
            specialAbility = InitializeBase(Plugin.PluginGuid, declaringType, Plugin.Directory);
            
            foreach (ZerglingSpecialAbilityData.PortraitChangeData changeData in LoadedData.portraitChanges)
            {
                InitializeTexture(changeData);
            }
        }

        private static void InitializeTexture(ZerglingSpecialAbilityData.PortraitChangeData data)
        {
            // Max Health cache
            m_maxZerglingHealth = Mathf.Max(m_maxZerglingHealth, data.health);
            
            // Texture
            byte[] texBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, data.portraitPath));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(texBytes);
            tex.name = "portrait_" + data.portraitPath;
            Sprite sprite = tex.ConvertTexture(TextureHelper.SpriteType.CardPortrait, FilterMode.Point);

            m_zerglingImages[data.health] = sprite;
            
            // Emit Texture
            byte[] emissiveImgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, data.portraitEmitPath));
            Texture2D emissiveTex = new Texture2D(2,2);
            emissiveTex.LoadImage(emissiveImgBytes);
            emissiveTex.name = tex.name + "_emission";
            sprite.RegisterEmissionForSprite(emissiveTex, TextureHelper.SpriteType.CardPortrait, FilterMode.Point);
        }

        private void Awake()
        {
            // Change portrait when the game starts and the ability is first added to the card
            RefreshPortrait();
        }

        public override bool RespondsToPlayFromHand()
        {
            return ShouldRefreshPortrait();
        }

        public override IEnumerator OnPlayFromHand()
        {
            RefreshPortrait();
            yield return null;
        }
        
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return ShouldRefreshPortrait();
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            RefreshPortrait();
            yield return null;
        }
        
        public override bool RespondsToResolveOnBoard()
        {
            return ShouldRefreshPortrait();
        }

        public override IEnumerator OnResolveOnBoard()
        {
            RefreshPortrait();
            yield return null;
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

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return ShouldRefreshPortrait();
        }

        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            RefreshPortrait();
            yield return null;
        }

        private int Health()
        {
            if (PlayableCard != null)
            {
                return PlayableCard.Health;
            }
            else
            {
                return Card.Info.Health;
            }
        }

        public bool ShouldRefreshPortrait()
        {
            return true;
        }

        public void RefreshPortrait()
        {
            int health = Mathf.Clamp(Health(), 0, m_maxZerglingHealth);
            if (health == 0)
            {
                return;
            }

            Sprite tex = m_zerglingImages[health];
            Card.Info.portraitTex = tex;
            Card.Info.portraitTex.name = tex.name;
            Card.Info.alternatePortrait = tex;
            Card.Info.alternatePortrait.name = tex.name;
            Card.RenderCard();
        }
    }
}