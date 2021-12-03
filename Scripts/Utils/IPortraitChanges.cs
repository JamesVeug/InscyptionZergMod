namespace ZergMod
{
    public interface IPortraitChanges
    {
        bool ShouldRefreshPortrait();
        void RefreshPortrait();
    }
}