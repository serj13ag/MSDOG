using Infrastructure;
using Infrastructure.StateMachine;
using Services.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _toNextLevelButton;

        private GameStateService _gameStateService;

        private void Awake()
        {
            _gameStateService = GameplayServices.GameStateService;
        }

        private void OnEnable()
        {
            GameplayServices.InputService.LockInput();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.AddListener(OnToNextLevelButtonClicked);
        }

        private void OnToNextLevelButtonClicked()
        {
            GlobalServices.GameStateMachine.Enter<GameplayState, int>(_gameStateService.CurrentLevelIndex + 1); // TODO: fix final level
        }

        private void OnToMainMenuButtonClicked()
        {
            GlobalServices.GameStateMachine.Enter<MainMenuState>();
        }

        private void OnDisable()
        {
            _toMainMenuButton.onClick.RemoveListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.RemoveListener(OnToNextLevelButtonClicked);
        }
    }
}