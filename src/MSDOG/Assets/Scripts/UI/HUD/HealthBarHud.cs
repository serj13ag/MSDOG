using Services.Gameplay;
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

        private GameFactory _gameFactory;

        [Inject]
        public void Construct(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Init()
        {
            UpdateView();

            _gameFactory.Player.OnHealthChanged += OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var player = _gameFactory.Player;
            _text.text = player.CurrentHealth.ToString();
            _healthFillImage.fillAmount = (float)player.CurrentHealth / player.MaxHealth;
        }

        private void OnDestroy()
        {
            _gameFactory.Player.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}