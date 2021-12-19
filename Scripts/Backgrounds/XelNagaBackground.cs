using APIPlugin;
using DiskCardGame;

namespace ZergMod.Scripts.Backgrounds
{
    public class XelNagaBackground : CardAppearanceBehaviour
    {
        private const string TextureFile = "Artwork/card_xelnaga_empty.png";

        public static Appearance CustomAppearance;

        public static void Initialize()
        {
            NewCardAppearanceBehaviour newBackgroundBehaviour = NewCardAppearanceBehaviour.AddNewBackground(typeof(XelNagaBackground), "XelNagaBackground");
            CustomAppearance = newBackgroundBehaviour.Appearance;
        }
        
        public override void ApplyAppearance()
        {
            base.Card.RenderInfo.baseTextureOverride = Utils.GetTextureFromPath(TextureFile);
        }
    }
}