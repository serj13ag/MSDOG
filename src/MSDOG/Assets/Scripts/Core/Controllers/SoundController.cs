using System;
using Core.Models.Data;
using Core.Services;
using Core.Sounds;
using UnityEngine;
using VContainer;

namespace Core.Controllers
{
    public class SoundController : BasePersistentController, ISoundController
    {
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        private IPlayerOptionsService _playerOptionsService;
        private IDataService _dataService;

        [Inject]
        public void Construct(IPlayerOptionsService playerOptionsService, IDataService dataService)
        {
            _dataService = dataService;
            _playerOptionsService = playerOptionsService;

            playerOptionsService.OnSoundOptionsUpdated += OnSoundOptionsUpdated;

            UpdateMusicVolume();
        }

        public void PlayMusic(AudioClip musicClip)
        {
            _musicAudioSource.clip = musicClip;
            _musicAudioSource.Play();
        }

        public void PlaySfx(SfxType type)
        {
            if (_playerOptionsService.IsMuted)
            {
                return;
            }

            var clip = _dataService.GetEffectSoundClip(type);
            PlaySfx(clip);
        }

        public void PlayAbilityActivationSfx(SoundClip abilityActivationSoundClip)
        {
            PlaySfx(abilityActivationSoundClip);
        }

        private void PlaySfx(SoundClip soundClip)
        {
            _sfxAudioSource.PlayOneShot(soundClip.Clip,
                soundClip.Volume * _playerOptionsService.MasterVolume * _playerOptionsService.SfxVolume);
        }

        private void OnSoundOptionsUpdated(object sender, EventArgs e)
        {
            UpdateMusicVolume();
        }

        private void UpdateMusicVolume()
        {
            _musicAudioSource.volume = _playerOptionsService.IsMuted
                ? 0f
                : _playerOptionsService.MasterVolume * _playerOptionsService.MusicVolume;
        }

        private void OnDestroy()
        {
            _playerOptionsService.OnSoundOptionsUpdated -= OnSoundOptionsUpdated;
        }
    }
}