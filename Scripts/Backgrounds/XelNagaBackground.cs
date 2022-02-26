﻿using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;

namespace ZergMod.Scripts.Backgrounds
{
    public class XelNagaBackground : CardAppearanceBehaviour
    {
        private const string TextureFile = "Artwork/card_xelnaga_empty.png";

        public static Appearance CustomAppearance;

        public static void Initialize()
        {
            CardAppearanceBehaviourManager.FullCardAppearanceBehaviour newBackgroundBehaviour = CardAppearanceBehaviourManager.Add(Plugin.PluginGuid, "XelNagaBackground", typeof(XelNagaBackground));
            CustomAppearance = newBackgroundBehaviour.Id;
        }
        
        public override void ApplyAppearance()
        {
            base.Card.RenderInfo.baseTextureOverride = Utils.GetTextureFromPath(TextureFile);
        }
    }
}