using Infrastructure;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class LoseWindow : MonoBehaviour
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _restartLevelButton;

        private void OnEnable()
        {
            GameplayServices.InputService.LockInput();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.AddListener(OnRestartLevelButtonClicked);
        }

        private void OnRestartLevelButtonClicked()
        {
            GlobalServices.GameStateMachine.Enter<GameplayState>();
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