namespace Core.Services
{
    public interface IProgressService
    {
        int LastPassedLevel { get; }
        bool EasyModeEnabled { get; }

        void UnlockAllLevels();
        void SetLastPassedLevel(int level);
        void SetEasyMode(bool easyModeEnabled);
    }
}