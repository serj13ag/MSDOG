using Infrastructure;
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

        private GameStateService _gameStateService;
        private InputService _inputService;

        [Inject]
        public void Construct(InputService inputService, GameStateService gameStateService)
        {
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
            GlobalServices.GameStateMachine.Enter<GameplayState, int>(_gameStateService.CurrentLevelIndex);
        }

        private void OnToMainMenuButtonClicked()
        {
            GlobalServices.GameStateMachine.Enter<MainMenuState>();
        }

        private void OnDisable()
        {
            _toMainMenuButton.onClick.RemoveListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.RemoveListener(OnRestartLevelButtonClicked);
        }
    }
}