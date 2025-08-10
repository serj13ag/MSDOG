using System;
using Gameplay.Controllers;
using Gameplay.Enemies;
using TMPro;
using UnityEngine;
using VContainer;

namespace Gameplay.UI
{
    public class HealthBarDebugView : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private TMP_Text _text;

        private IDebugController _debugController;

        private Camera _mainCamera;

        [Inject]
        public void Construct(IDebugController debugController)
        {
            _debugController = debugController;
        }

        private void OnEnable()
        {
            _enemy.OnHealthChanged += OnEnemyHealthChanged;
        }

        private void OnDisable()
        {
            _enemy.OnHealthChanged -= OnEnemyHealthChanged;
        }

        private void Start()
        {
            _mainCamera = Camera.main; // TODO: use provider

            var showDebugHp = _debugController.DebugHpIsVisible;
            gameObject.SetActive(showDebugHp);

            if (showDebugHp)
            {
                UpdateText();
            }

            _debugController.OnShowDebugHealthBar += OnShowDebugHealthBar;
            _debugController.OnHideDebugHealthBar += OnHideDebugHealthBar;
        }

        private void Update()
        {
            transform.rotation = _mainCamera.transform.rotation;
        }

        private void OnHideDebugHealthBar(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void OnShowDebugHealthBar(object sender, EventArgs e)
        {
            UpdateText();

            gameObject.SetActive(true);
        }

        private void OnEnemyHealthChanged(object sender, EventArgs eventArgs)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = $"{_enemy.CurrentHealth}/{_enemy.MaxHealth}";
        }

        private void OnDestroy()
        {
            _debugController.OnShowDebugHealthBar -= OnShowDebugHealthBar;
            _debugController.OnHideDebugHealthBar -= OnHideDebugHealthBar;
        }
    }
}