using System;
using Sounds;
using UnityEngine;
using VContainer;

namespace Services
{
    public class SoundService : PersistentMonoService
    {
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        [SerializeField] private SoundClip _musicSoundClip;

        [SerializeField] private SoundClip _actionSoundClip;
        [SerializeField] private SoundClip _activateSoundClip;

        private PlayerOptionsService _playerOptionsService;

        [Inject]
        public void Construct(PlayerOptionsService playerOptionsService)
        {
            _playerOptionsService = playerOptionsService;

            playerOptionsService.OnSoundOptionsUpdated += OnSoundOptionsUpdated;

            UpdateMusicVolume();
        }

        public void PlayMusic()
        {
            _musicAudioSource.clip = _musicSoundClip.Clip;
            _musicAudioSource.Play();
        }

        public void StopMusic()
        {
            _musicAudioSource.Stop();
        }

        public void PauseMusic()
        {
            _musicAudioSource.Pause();
        }

        public void ResumeMusic()
        {
            _musicAudioSource.UnPause();
        }

        public void PlaySfx(SfxType type)
        {
            if (_playerOptionsService.IsMuted)
            {
                return;
            }

            var soundClip = type switch
            {
                SfxType.Action => _actionSoundClip,
                SfxType.Activate => _activateSoundClip,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

            PlaySfx(soundClip);
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