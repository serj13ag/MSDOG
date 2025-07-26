using System;
using UnityEngine;

namespace Services
{
    public class PlayerOptionsService
    {
        private const string IsMutedKey = "IsMuted";
        private const string MasterVolumeKey = "MasterVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SFXVolume";

        public bool IsMuted { get; private set; }
        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float SfxVolume { get; private set; }

        public event EventHandler<EventArgs> OnSoundOptionsUpdated;

        public PlayerOptionsService()
        {
            IsMuted = PlayerPrefs.GetInt(IsMutedKey, 0) == 1;
            MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
            MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.7f);
            SfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        }

        public void UpdateOptions(bool isMute, float masterVolume, float musicVolume, float sfxVolume)
        {
            IsMuted = isMute;
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            SfxVolume = sfxVolume;

            Save();

            OnSoundOptionsUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(IsMutedKey, IsMuted ? 1 : 0);
            PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
            PlayerPrefs.SetFloat(SfxVolumeKey, SfxVolume);
            PlayerPrefs.Save();
        }
    }
}