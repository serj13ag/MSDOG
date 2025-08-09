using System;
using Gameplay.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Windows
{
    public class LoseWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _restartLevelButton;

        private IGameStateMachine _gameStateMachine;
        private ILevelFlowService _levelFlowService;
        private IInputService _inputService;

        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, IInputService inputService, ILevelFlowService levelFlowService)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _levelFlowService = levelFlowService;
        }

        private void OnEnable()
        {
            _inputService.LockInput();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.AddListener(OnRestartLevelButtonClicked);
        }

        private void OnRestartLevelButtonClicked()
        {
            _gameStateMachine.Enter<GameplayState, int>(_levelFlowService.CurrentLevelIndex);
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnToMainMenuButtonClicked()
        {
            _gameStateMachine.Enter<MainMenuState>();
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnDisable()
        {
            _toMainMenuButton.onClick.RemoveListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.RemoveListener(OnRestartLevelButtonClicked);
        }
    }
}