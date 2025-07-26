using System;
using Infrastructure.StateMachine;
using Services;
using Services.Gameplay;
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
        private GameStateService _gameStateService;
        private WindowService _windowService;

        public GameObject GameObject => gameObject;
        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, GameStateService gameStateService, WindowService windowService)
        {
            _windowService = windowService;
            _gameStateService = gameStateService;
            _gameStateMachine = gameStateMachine;
        }

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(Restart);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            _menuButton.onClick.AddListener(GoToMenu);
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(Restart);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            _menuButton.onClick.RemoveListener(GoToMenu);
            _closeButton.onClick.RemoveListener(Close);
        }

        private void Restart()
        {
            _gameStateMachine.Enter<GameplayState, int>(_gameStateService.CurrentLevelIndex);
        }

        private void GoToMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void OnOptionsButtonClicked()
        {
            _windowService.ShowOptions();
        }

        private void Close()
        {
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}