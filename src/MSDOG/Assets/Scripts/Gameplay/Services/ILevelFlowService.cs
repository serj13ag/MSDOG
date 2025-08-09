namespace Gameplay.Services
{
    public interface ILevelFlowService
    {
        int CurrentLevelIndex { get; }
        bool IsLastLevel { get; }

        void InitLevel(int levelIndex);
    }
}