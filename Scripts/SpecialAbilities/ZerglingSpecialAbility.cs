using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using UnityEngine;
using ZergMod;
using Plugin = ZergMod.Plugin;

namespace DiskCardGame
{
    public class ZerglingSpecialAbility : SpecialCardBehaviour, IPortraitChanges
    {
        public SpecialTriggeredAbility SpecialAbility => specialAbility;
        public static SpecialTriggeredAbility specialAbility;

        public static int MaxZerglingsToSwarm = 6;
        public static int SwarmDamageBonus = 1;
        
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

            SpecialAbilityIdentifier identifier = SpecialAbilityIdentifier.GetID("ZerglingSpecialAbility", "ZerglingSpecialAbility");
            
            StatIconInfo iconInfo = new StatIconInfo();
            iconInfo.rulebookName = "Zergling Swarm";
            iconInfo.rulebookDescription = "Portrait changes as the health increases. Max 6";
            iconInfo.iconType = SpecialStatIcon.CardsInHand;
            iconInfo.iconGraphic = Utils.GetTextureFromPath("Artwork/six_zergling.png");
            iconInfo.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook };
            
            NewSpecialAbility newSpecialAbility = new NewSpecialAbility(typeof(ZerglingSpecialAbility), identifier, iconInfo);
            specialAbility = newSpecialAbility.specialTriggeredAbility;
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
            Card.RenderCard();
        }
    }
}