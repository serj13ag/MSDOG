using System;
using Core.Controllers;
using Core.Models.Data;
using Gameplay.Factories;

namespace Gameplay.Services
{
    public class GameplayWindowService : IGameplayWindowService
    {
        private readonly IWindowController _windowController;
        private readonly IGameplayWindowFactory _gameplayWindowFactory;

        public GameplayWindowService(IWindowController windowController, IGameplayWindowFactory gameplayWindowFactory)
        {
            _gameplayWindowFactory = gameplayWindowFactory;
            _windowController = windowController;
        }

        public void ShowWinWindow()
        {
            var winWindow = _gameplayWindowFactory.CreateWinWindow(_windowController.UiCanvasRootTransform);
            _windowController.ShowWindow(winWindow);
        }

        public void ShowLoseWindow()
        {
            var loseWindow = _gameplayWindowFactory.CreateLoseWindow(_windowController.UiCanvasRootTransform);
            _windowController.ShowWindow(loseWindow);
        }

        public void ShowCreditsWindow()
        {
            _windowController.ShowCreditsWindow();
        }

        public void ShowEscapeWindow()
        {
            var escapeWindow = _gameplayWindowFactory.CreateEscapeWindow(_windowController.UiCanvasRootTransform);
            _windowController.ShowWindow(escapeWindow);
        }

        public void ShowTutorialWindow(TutorialEventData tutorialEventData)
        {
            var dialogueWindow =
                _gameplayWindowFactory.CreateTutorialWindow(tutorialEventData, _windowController.UiCanvasRootTransform);
            _windowController.ShowWindow(dialogueWindow);
        }

        public void ShowDialogueWindow(DialogueData dialogueData, Action onDialogueCompleted)
        {
            var dialogueWindow = _gameplayWindowFactory.CreateDialogueWindow(dialogueData, onDialogueCompleted,
                _windowController.UiCanvasRootTransform);
            _windowController.ShowWindow(dialogueWindow);
        }
    }
}