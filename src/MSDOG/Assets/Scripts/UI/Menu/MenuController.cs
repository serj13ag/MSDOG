using Infrastructure;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _quitGameButton;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _quitGameButton.onClick.AddListener(Quit);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            _quitGameButton.onClick.RemoveListener(Quit);
        }

        private void StartGame()
        {
            GlobalServices.GameStateMachine.Enter<GameplayState>();
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}