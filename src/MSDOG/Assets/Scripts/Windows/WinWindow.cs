using System;
using Core.Services;
using Gameplay.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Windows
{
    public class WinWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _toNextLevelButton;

        private IGameStateMachine _gameStateMachine;
        private IDataService _dataService;
        private ILevelFlowService _levelFlowService;
        private IInputService _inputService;

        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, IDataService dataService, ILevelFlowService levelFlowService,
            IInputService inputService)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _dataService = dataService;
            _levelFlowService = levelFlowService;

            _toNextLevelButton.gameObject.SetActive(!levelFlowService.IsLastLevel);
        }

        private void OnEnable()
        {
            _inputService.LockInput();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.AddListener(OnToNextLevelButtonClicked);
        }

        private void OnToNextLevelButtonClicked()
        {
            var nextLevelIndex = _levelFlowService.CurrentLevelIndex + 1;
            if (nextLevelIndex < _dataService.GetNumberOfLevels())
            {
                _gameStateMachine.Enter<GameplayState, int>(nextLevelIndex);
            }
            else
            {
                Debug.LogWarning("This is last level! Going to main menu.");
                _gameStateMachine.Enter<MainMenuState>();
            }

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
            _toNextLevelButton.onClick.RemoveListener(OnToNextLevelButtonClicked);
        }
    }
}