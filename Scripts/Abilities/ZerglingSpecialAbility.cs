﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CardLoaderPlugin.lib;
using UnityEngine;
using ZergMod;

namespace DiskCardGame
{
    public class ZerglingSpecialAbility : SpecialCardBehaviour
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;

        private static Dictionary<int, Texture2D> m_zerglingImages = new Dictionary<int, Texture2D>();
        private static int m_maxZerglingHealth = 0;

        public static void Initialize()
        {
            InitializeTexture(1, "Artwork/zergling.png");
            InitializeTexture(2, "Artwork/two_zergling.png");
            InitializeTexture(3, "Artwork/three_zergling.png");
            InitializeTexture(4, "Artwork/four_zergling.png");
            InitializeTexture(5, "Artwork/five_zergling.png");
            InitializeTexture(6, "Artwork/six_zergling.png");

            NewSpecialAbility newAbility = new NewSpecialAbility(typeof(ZerglingSpecialAbility));
            ZerglingSpecialAbility.specialAbility = newAbility.SpecialAbility;
        }

        private static void InitializeTexture(int health, string fileName)
        {
            byte[] imgBytes = File.ReadAllBytes(Path.Combine(ZergMod.Plugin.Directory, fileName));
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(imgBytes);
            tex.name = "portrait_" + fileName;
            tex.filterMode = FilterMode.Point;

            m_zerglingImages[health] = tex;
            m_maxZerglingHealth = Mathf.Max(m_maxZerglingHealth, health);
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

        private bool ShouldRefreshPortrait()
        {
            return !PlayableCard.Dead;
        }

        private void RefreshPortrait()
        {
            int health = Mathf.Clamp(PlayableCard.Health, 0, m_maxZerglingHealth);
            Plugin.Log.LogInfo("[ZerglingSpecialAbility] RefreshPortrait health: " + health);
            if (health == 0)
            {
                return;
            }

            Texture2D tex = m_zerglingImages[health];
            Card.Info.portraitTex = Sprite.Create(tex, CardUtils.DefaultCardArtRect, CardUtils.DefaultVector2);
            Card.Info.portraitTex.name = tex.name;
            Card.RenderCard();
        }
    }
}