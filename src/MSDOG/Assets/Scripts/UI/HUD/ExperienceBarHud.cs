using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class ExperienceBarHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Button _craftButton;

        private Player _player;

        public void Init(Player player)
        {
            _player = player;

            UpdateView();

            _craftButton.onClick.AddListener(OnCraftButtonClick);
            player.OnExperienceChanged += OnPlayerExperienceChanged;
        }

        private void OnCraftButtonClick()
        {
            Debug.Log("CRAFT!");
            _player.ResetExperience();
        }

        private void OnPlayerExperienceChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _text.text = $"{_player.CurrentExperience}/{_player.MaxExperience}";
            _fillImage.fillAmount = (float)_player.CurrentExperience / _player.MaxExperience;

            _craftButton.interactable = _player.CurrentExperience >= _player.MaxExperience;
        }

        private void OnDestroy()
        {
            _craftButton.onClick.RemoveListener(OnCraftButtonClick);
            _player.OnExperienceChanged -= OnPlayerExperienceChanged;
        }
    }
}