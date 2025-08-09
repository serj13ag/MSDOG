using System;
using Core.Models.SaveData;

namespace Core.Services
{
    public class PlayerOptionsService : IPlayerOptionsService
    {
        private const string PlayerOptionsSaveDataKey = "PlayerOptionsSaveData";

        private readonly ISaveLoadService _saveLoadService;

        public bool IsMuted { get; private set; }
        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float SfxVolume { get; private set; }

        public event EventHandler<EventArgs> OnSoundOptionsUpdated;

        public PlayerOptionsService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            var playerOptionsSave = _saveLoadService.Load<PlayerOptionsSaveData>(PlayerOptionsSaveDataKey);
            IsMuted = playerOptionsSave.IsMuted;
            MasterVolume = playerOptionsSave.MasterVolume;
            MusicVolume = playerOptionsSave.MusicVolume;
            SfxVolume = playerOptionsSave.SfxVolume;
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
            var saveData = new PlayerOptionsSaveData(IsMuted, MasterVolume, MusicVolume, SfxVolume);
            _saveLoadService.Save(saveData, PlayerOptionsSaveDataKey);
        }
    }
}