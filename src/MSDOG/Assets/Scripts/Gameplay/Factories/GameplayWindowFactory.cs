using System;
using Constants;
using Core.Models.Data;
using Core.Services;
using UI.Windows;
using UnityEngine;
using VContainer;

namespace Gameplay.Factories
{
    public class GameplayWindowFactory
    {
        private readonly IObjectResolver _container;
        private readonly IAssetProviderService _assetProviderService;

        public GameplayWindowFactory(IObjectResolver container, IAssetProviderService assetProviderService)
        {
            _container = container;
            _assetProviderService = assetProviderService;
        }

        public LoseWindow CreateLoseWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<LoseWindow>(AssetPaths.LoseWindowPath, canvasTransform, _container);
        }

        public WinWindow CreateWinWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<WinWindow>(AssetPaths.WinWindowPath, canvasTransform, _container);
        }

        public EscapeWindow CreateEscapeWindow(Transform canvasTransform)
        {
            return _assetProviderService.Instantiate<EscapeWindow>(AssetPaths.EscapeWindowPath, canvasTransform, _container);
        }

        public IWindow CreateDialogueWindow(DialogueData dialogueData, Action onDialogueCompleted, Transform canvasTransform)
        {
            var dialogueWindow =
                _assetProviderService.Instantiate<DialogueWindow>(AssetPaths.DialogueWindowPath, canvasTransform, _container);
            dialogueWindow.Init(dialogueData, onDialogueCompleted);
            return dialogueWindow;
        }

        public IWindow CreateTutorialWindow(TutorialEventData tutorialEventData, Transform canvasTransform)
        {
            var dialogueWindow =
                _assetProviderService.Instantiate<TutorialWindow>(AssetPaths.TutorialWindowPath, canvasTransform, _container);
            dialogueWindow.Init(tutorialEventData);
            return dialogueWindow;
        }
    }
}