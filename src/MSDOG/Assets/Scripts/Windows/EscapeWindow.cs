using System;
using Core.Controllers;
using Gameplay.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Windows
{
    public class EscapeWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _closeButton;

        private IGameStateMachine _gameStateMachine;
        private ILevelFlowService _levelFlowService;
        private IWindowController _windowController;

        public GameObject GameObject => gameObject;
        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, ILevelFlowService levelFlowService,
            IWindowController windowController)
        {
            _windowController = windowController;
            _levelFlowService = levelFlowService;
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
            _gameStateMachine.Enter<GameplayState, int>(_levelFlowService.CurrentLevelIndex);
        }

        private void GoToMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void OnOptionsButtonClicked()
        {
            _windowController.ShowOptionsWindow();
        }

        private void Close()
        {
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}