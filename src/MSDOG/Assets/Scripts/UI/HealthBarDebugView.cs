using System;
using Gameplay;
using Gameplay.Controllers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthBarDebugView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private DebugService _debugService;

        private HealthBlock _healthBlock;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Init(HealthBlock healthBlock, DebugService debugService)
        {
            _debugService = debugService;
            _healthBlock = healthBlock;

            if (!debugService.DebugHpIsVisible)
            {
                gameObject.SetActive(false);
            }

            UpdateText();

            healthBlock.OnHealthChanged += UpdateText;
            debugService.OnShowDebugHealthBar += OnShowDebugHealthBar;
            debugService.OnHideDebugHealthBar += OnHideDebugHealthBar;
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

        private void UpdateText()
        {
            _text.text = $"{_healthBlock.CurrentHealth}/{_healthBlock.MaxHealth}";
        }

        private void OnDestroy()
        {
            _healthBlock.OnHealthChanged -= UpdateText;
            _debugService.OnShowDebugHealthBar -= OnShowDebugHealthBar;
            _debugService.OnHideDebugHealthBar -= OnHideDebugHealthBar;
        }
    }
}