using Gameplay.Providers;
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

        private IPlayerProvider _playerProvider;

        [Inject]
        public void Construct(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public void Init()
        {
            UpdateView();

            _playerProvider.Player.OnHealthChanged += OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var player = _playerProvider.Player;
            _text.text = player.CurrentHealth.ToString();
            _healthFillImage.fillAmount = (float)player.CurrentHealth / player.MaxHealth;
        }

        private void OnDestroy()
        {
            _playerProvider.Player.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}