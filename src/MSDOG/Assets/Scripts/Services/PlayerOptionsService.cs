using System;
using SaveData;

namespace Services
{
    public class PlayerOptionsService
    {
        private const string PlayerOptionsSaveDataKey = "PlayerOptionsSaveData";

        private readonly SaveLoadService _saveLoadService;

        public bool IsMuted { get; private set; }
        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float SfxVolume { get; private set; }

        public event EventHandler<EventArgs> OnSoundOptionsUpdated;

        public PlayerOptionsService(SaveLoadService saveLoadService)
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