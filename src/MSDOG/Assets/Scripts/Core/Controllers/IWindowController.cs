using System;
using Core.Models.Data;
using Gameplay.Factories;
using UI.Windows;

namespace Core.Controllers
{
    public interface IWindowController
    {
        void RegisterGameplayWindowFactory(IGameplayWindowFactory gameplayWindowFactory);
        void RemoveGameplayWindowFactory();

        bool WindowIsActive<T>() where T : IWindow;

        void ShowOptions();
        void ShowCreditsWindow();
        void ShowEscapeWindow();
        void ShowWinWindow();
        void ShowLoseWindow();
        void ShowDialogueWindow(DialogueData dialogueData, Action onDialogueCompleted);
        void ShowTutorialWindow(TutorialEventData tutorialEventData);

        void CloseAllWindows();
        void CloseActiveWindow();
    }
}