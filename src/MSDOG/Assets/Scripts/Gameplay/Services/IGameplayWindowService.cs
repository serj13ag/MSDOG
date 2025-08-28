using System;
using Core.Models.Data;

namespace Gameplay.Services
{
    public interface IGameplayWindowService
    {
        void ShowLoseWindow();
        void ShowWinWindow();
        void ShowCreditsWindow();
        void ShowEscapeWindow();
        void ShowTutorialWindow(TutorialEventData tutorialEventData);
        void ShowDialogueWindow(DialogueData dialogueData, Action onDialogueCompleted);
    }
}