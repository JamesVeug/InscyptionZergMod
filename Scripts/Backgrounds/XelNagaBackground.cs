using DiskCardGame;

namespace ZergMod.Scripts.Backgrounds
{
    public class XelNagaBackground : CardAppearanceBehaviour
    {
        private const string TextureFile = "Artwork/card_xelnaga_empty.png";

        public static Appearance CustomAppearance;

        public static void Initialize()
        {
            NewBackgroundBehaviour newBackgroundBehaviour = NewBackgroundBehaviour.AddNewBackground(typeof(XelNagaBackground));
            CustomAppearance = newBackgroundBehaviour.Appearance;
        }
        
        // Token: 0x060014EE RID: 5358 RVA: 0x00046502 File Offset: 0x00044702
        public override void ApplyAppearance()
        {
            base.Card.RenderInfo.baseTextureOverride = Utils.GetTextureFromPath(TextureFile);
        }
    }
}