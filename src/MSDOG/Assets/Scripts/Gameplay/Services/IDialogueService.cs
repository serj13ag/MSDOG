using System;

namespace Gameplay.Services
{
    public interface IDialogueService
    {
        bool TryShowStartLevelDialogue(int levelIndex, Action onDialogueCompleted);
        bool TryShowEndLevelDialogue(int levelIndex, Action onDialogueCompleted);
    }
}