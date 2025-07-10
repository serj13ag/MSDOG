using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthBarDebugView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private HealthBlock _healthBlock;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Init(HealthBlock healthBlock)
        {
            _healthBlock = healthBlock;

            UpdateText();

            healthBlock.OnHealthChanged += UpdateText;
        }

        private void Update()
        {
            transform.rotation = _mainCamera.transform.rotation;
        }

        private void UpdateText()
        {
            _text.text = $"{_healthBlock.CurrentHealth}/{_healthBlock.MaxHealth}";
        }

        private void OnDestroy()
        {
            _healthBlock.OnHealthChanged -= UpdateText;
        }
    }
}