using System;
using Gameplay.Controllers;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class HealthBarDebugView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private IDebugController _debugController;

        private HealthBlock _healthBlock;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Init(HealthBlock healthBlock, IDebugController debugController)
        {
            _debugController = debugController;
            _healthBlock = healthBlock;

            gameObject.SetActive(debugController.DebugHpIsVisible);

            UpdateText();

            healthBlock.OnHealthChanged += UpdateText;
            debugController.OnShowDebugHealthBar += OnShowDebugHealthBar;
            debugController.OnHideDebugHealthBar += OnHideDebugHealthBar;
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
            _debugController.OnShowDebugHealthBar -= OnShowDebugHealthBar;
            _debugController.OnHideDebugHealthBar -= OnHideDebugHealthBar;
        }
    }
}