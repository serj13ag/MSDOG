using System;
using Windows;
using Core.Models.Data;
using Core.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class GameplayWindowFactory : IGameplayWindowFactory
    {
        private readonly IObjectResolver _container;
        private readonly WindowsData _windowsData;

        public GameplayWindowFactory(IObjectResolver container, IDataService dataService)
        {
            _container = container;
            _windowsData = dataService.GetSettingsData().WindowsData;
        }

        public LoseWindow CreateLoseWindow(Transform canvasTransform)
        {
            return _container.Instantiate(_windowsData.LoseWindowPrefab, canvasTransform);
        }

        public WinWindow CreateWinWindow(Transform canvasTransform)
        {
            return _container.Instantiate(_windowsData.WinWindowPrefab, canvasTransform);
        }

        public EscapeWindow CreateEscapeWindow(Transform canvasTransform)
        {
            return _container.Instantiate(_windowsData.EscapeWindowPrefab, canvasTransform);
        }

        public IWindow CreateDialogueWindow(DialogueData dialogueData, Action onDialogueCompleted, Transform canvasTransform)
        {
            var dialogueWindow = _container.Instantiate(_windowsData.DialogueWindowPrefab, canvasTransform);
            dialogueWindow.Init(dialogueData, onDialogueCompleted);
            return dialogueWindow;
        }

        public IWindow CreateTutorialWindow(TutorialEventData tutorialEventData, Transform canvasTransform)
        {
            var dialogueWindow = _container.Instantiate(_windowsData.TutorialWindowPrefab, canvasTransform);
            dialogueWindow.Init(tutorialEventData);
            return dialogueWindow;
        }
    }
}