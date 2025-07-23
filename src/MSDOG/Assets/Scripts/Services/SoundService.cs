using System;
using Sounds;
using UnityEngine;

namespace Services
{
    public class SoundService : BaseMonoService
    {
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        [SerializeField] private SoundClip _musicSoundClip;

        [SerializeField] private SoundClip _actionSoundClip;
        [SerializeField] private SoundClip _activateSoundClip;

        private float _masterVolume = 1f;
        private float _musicVolume = 0.7f;
        private float _sfxVolume = 1f;

        private void Start()
        {
            LoadVolumeSettings();
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            UpdateMusicVolume();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            UpdateMusicVolume();
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
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
            var soundClip = type switch
            {
                SfxType.Action => _actionSoundClip,
                SfxType.Activate => _activateSoundClip,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

            _sfxAudioSource.PlayOneShot(soundClip.Clip, soundClip.Volume * _sfxVolume * _masterVolume);
        }

        private void UpdateMusicVolume()
        {
            _musicAudioSource.volume = _musicVolume * _masterVolume;
        }

        private void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
            PlayerPrefs.Save();
        }

        private void LoadVolumeSettings()
        {
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            UpdateMusicVolume();
        }
    }
}