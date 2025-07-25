using Infrastructure.StateMachine;
using Services.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Windows
{
    public class LoseWindow : MonoBehaviour
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _restartLevelButton;

        private GameStateMachine _gameStateMachine;
        private GameStateService _gameStateService;
        private InputService _inputService;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine, InputService inputService, GameStateService gameStateService)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _gameStateService = gameStateService;
        }

        private void OnEnable()
        {
            _inputService.LockInput();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.AddListener(OnRestartLevelButtonClicked);
        }

        private void OnRestartLevelButtonClicked()
        {
            _gameStateMachine.Enter<GameplayState, int>(_gameStateService.CurrentLevelIndex);
        }

        private void OnToMainMenuButtonClicked()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void OnDisable()
        {
            _toMainMenuButton.onClick.RemoveListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.RemoveListener(OnRestartLevelButtonClicked);
        }
    }
}