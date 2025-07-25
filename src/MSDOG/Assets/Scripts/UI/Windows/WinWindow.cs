using Infrastructure.StateMachine;
using Services;
using Services.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Windows
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _toNextLevelButton;

        private GameStateMachine _gameStateMachine;
        private DataService _dataService;
        private GameStateService _gameStateService;
        private InputService _inputService;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, DataService dataService, GameStateService gameStateService,
            InputService inputService)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _dataService = dataService;
            _gameStateService = gameStateService;
        }

        private void OnEnable()
        {
            _inputService.LockInput();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.AddListener(OnToNextLevelButtonClicked);
        }

        private void OnToNextLevelButtonClicked()
        {
            var nextLevelIndex = _gameStateService.CurrentLevelIndex + 1;
            if (nextLevelIndex < _dataService.GetNumberOfLevels())
            {
                _gameStateMachine.Enter<GameplayState, int>(nextLevelIndex);
            }
            else
            {
                _gameStateMachine.Enter<MainMenuState>();
            }
        }

        private void OnToMainMenuButtonClicked()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void OnDisable()
        {
            _toMainMenuButton.onClick.RemoveListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.RemoveListener(OnToNextLevelButtonClicked);
        }
    }
}