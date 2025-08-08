using Gameplay.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.HUD
{
    public class HealthBarHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _healthFillImage;

        private PlayerService _playerService;

        [Inject]
        public void Construct(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Init()
        {
            UpdateView();

            _playerService.Player.OnHealthChanged += OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var player = _playerService.Player;
            _text.text = player.CurrentHealth.ToString();
            _healthFillImage.fillAmount = (float)player.CurrentHealth / player.MaxHealth;
        }

        private void OnDestroy()
        {
            _playerService.Player.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}