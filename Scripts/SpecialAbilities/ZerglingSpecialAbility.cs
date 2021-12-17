using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using ZergMod.Scripts.Data;

namespace ZergMod.Scripts.SpecialAbilities
{
    public class ZerglingSpecialAbility : ACustomSpecialAbilityBehaviour<ZerglingSpecialAbilityData>, IPortraitChanges
    {
        private static Dictionary<int, Texture2D> m_zerglingImages = new Dictionary<int, Texture2D>();
        private static int m_maxZerglingHealth = 0;

        public new static void Initialize(Type declaringType)
        {
            ACustomSpecialAbilityBehaviour<ZerglingSpecialAbilityData>.Initialize(declaringType);
            
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
            tex.filterMode = FilterMode.Point;

            m_zerglingImages[data.health] = tex;
            
            // Emit Texture
            byte[] emissiveImgBytes = File.ReadAllBytes(Path.Combine(Plugin.Directory, data.portraitEmitPath));
            Texture2D emissiveTex = new Texture2D(2,2);
            emissiveTex.LoadImage(emissiveImgBytes);
            emissiveTex.name = tex.name + "_emission";
            emissiveTex.filterMode = FilterMode.Point;
                
            Sprite emissiveSprite = Sprite.Create(emissiveTex, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
            emissiveSprite.name = tex.name + "_emission";
            NewCard.emissions.Add(tex.name, emissiveSprite);
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

            Texture2D tex = m_zerglingImages[health];
            Card.Info.portraitTex = Sprite.Create(tex, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
            Card.Info.portraitTex.name = tex.name;
            Card.Info.alternatePortrait = Sprite.Create(tex, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
            Card.Info.alternatePortrait.name = tex.name;
            Card.RenderCard();
        }
    }
}