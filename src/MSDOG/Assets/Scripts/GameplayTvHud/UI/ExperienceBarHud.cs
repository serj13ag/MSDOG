using Gameplay.Providers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameplayTvHud.UI
{
    public class ExperienceBarHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _fillImage;
        [SerializeField] private CraftButtonHud _craftButton;

        private IPlayerProvider _playerProvider;

        [Inject]
        public void Construct(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;

            UpdateView();

            playerProvider.Player.OnExperienceChanged += OnPlayerExperienceChanged;
        }

        private void OnPlayerExperienceChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var player = _playerProvider.Player;

            _text.text = $"{player.CurrentExperience}/{player.MaxExperience}";
            _fillImage.fillAmount = (float)player.CurrentExperience / player.MaxExperience;

            var canCraft = player.CurrentExperience >= player.MaxExperience;
            _craftButton.SetCanCraft(canCraft);
        }

        private void OnDestroy()
        {
            _playerProvider.Player.OnExperienceChanged -= OnPlayerExperienceChanged;
        }
    }
}