using System;
using Windows;
using Core.Models.Data;
using Gameplay.Factories;

namespace Core.Controllers
{
    public interface IWindowController
    {
        bool HasActiveWindows { get; }

        event EventHandler<EventArgs> OnWindowShowed;
        event EventHandler<EventArgs> OnWindowClosed;

        void RegisterGameplayWindowFactory(IGameplayWindowFactory gameplayWindowFactory);
        void RemoveGameplayWindowFactory();

        bool WindowIsActive<T>() where T : IWindow;

        void ShowOptionsWindow();
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