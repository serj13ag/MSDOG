using Data;
using Infrastructure;
using Infrastructure.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MenuLevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private GameStateMachine _gameStateMachine;

        private int _levelIndex;

        private void Awake()
        {
            _gameStateMachine = GlobalServices.GameStateMachine;
        }

        public void Init(LevelData levelData)
        {
            _levelIndex = levelData.LevelIndex;

            _text.text = levelData.LevelName;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(EnterLevel);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(EnterLevel);
        }

        private void EnterLevel()
        {
            _gameStateMachine.Enter<GameplayState, int>(_levelIndex);
        }
    }
}