using Core.Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Windows
{
    public class OptionsWindow : BaseWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Toggle _muteToggle;
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        private IPlayerOptionsService _playerOptionsService;

        [Inject]
        public void Construct(IPlayerOptionsService playerOptionsService)
        {
            _playerOptionsService = playerOptionsService;

            _muteToggle.isOn = playerOptionsService.IsMuted;
            _masterVolumeSlider.value = playerOptionsService.MasterVolume;
            _musicVolumeSlider.value = playerOptionsService.MusicVolume;
            _sfxVolumeSlider.value = playerOptionsService.SfxVolume;
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Close);
            _muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
            _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Close);
            _muteToggle.onValueChanged.RemoveListener(OnMuteToggleChanged);
            _masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            _sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        }

        private void OnMuteToggleChanged(bool value)
        {
            SaveOptions();
        }

        private void OnMasterVolumeChanged(float value)
        {
            SaveOptions();
        }

        private void OnMusicVolumeChanged(float value)
        {
            SaveOptions();
        }

        private void OnSfxVolumeChanged(float value)
        {
            SaveOptions();
        }

        private void SaveOptions()
        {
            _playerOptionsService.UpdateOptions(_muteToggle.isOn, _masterVolumeSlider.value, _musicVolumeSlider.value,
                _sfxVolumeSlider.value);
        }
    }
}