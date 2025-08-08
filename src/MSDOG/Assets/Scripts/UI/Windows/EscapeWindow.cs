using System;
using Core.Controllers;
using Gameplay.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Windows
{
    public class EscapeWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _closeButton;

        private GameStateMachine _gameStateMachine;
        private LevelFlowService _levelFlowService;
        private WindowController _windowController;
        private UpdateController _updateController;

        public GameObject GameObject => gameObject;
        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, LevelFlowService levelFlowService, WindowController windowController,
            UpdateController updateController)
        {
            _updateController = updateController;
            _windowController = windowController;
            _levelFlowService = levelFlowService;
            _gameStateMachine = gameStateMachine;
        }

        private void OnEnable()
        {
            _updateController.Pause(true);

            _restartButton.onClick.AddListener(Restart);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            _menuButton.onClick.AddListener(GoToMenu);
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _updateController.Unpause(true);

            _restartButton.onClick.RemoveListener(Restart);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            _menuButton.onClick.RemoveListener(GoToMenu);
            _closeButton.onClick.RemoveListener(Close);
        }

        private void Restart()
        {
            _gameStateMachine.Enter<GameplayState, int>(_levelFlowService.CurrentLevelIndex);
        }

        private void GoToMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void OnOptionsButtonClicked()
        {
            _windowController.ShowOptions();
        }

        private void Close()
        {
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}