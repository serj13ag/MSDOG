using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HealthBarHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _healthFillImage;

        private Player _player;

        public void Init(Player player)
        {
            _player = player;

            UpdateView();

            player.OnHealthChanged += OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _text.text = _player.CurrentHealth.ToString();
            _healthFillImage.fillAmount = (float)_player.CurrentHealth / _player.MaxHealth;
        }

        private void OnDestroy()
        {
            _player.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}