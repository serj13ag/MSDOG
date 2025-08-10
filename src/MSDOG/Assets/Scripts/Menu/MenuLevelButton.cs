using System;
using Core.Models.Data;
using Infrastructure.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Menu
{
    public class MenuLevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private IGameStateMachine _gameStateMachine;

        private int _levelIndex;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Init(LevelData levelData, bool isAvailable)
        {
            _levelIndex = levelData.LevelIndex;

            _text.text = levelData.LevelName;

            var textColor = _text.color;
            textColor.a = isAvailable ? 1f : 0.1f;
            _text.color = textColor;

            _button.interactable = isAvailable;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(EnterLevel);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(EnterLevel);
        }

        public void UpdateIsAvailable(bool isAvailable)
        {
            var textColor = _text.color;
            textColor.a = isAvailable ? 1f : 0.1f;
            _text.color = textColor;

            _button.interactable = isAvailable;
        }

        private void EnterLevel()
        {
            _gameStateMachine.Enter<GameplayState, int>(_levelIndex);
        }
    }
}