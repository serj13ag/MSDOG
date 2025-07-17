using Infrastructure;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _toNextLevelButton;

        private void OnEnable()
        {
            GameplayServices.InputService.LockInput();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.AddListener(OnToNextLevelButtonClicked);
        }

        private void OnToNextLevelButtonClicked()
        {
            GlobalServices.GameStateMachine.Enter<GameplayState>(); // TODO: to next level
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