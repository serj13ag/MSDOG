using System;

namespace Core.Models.SaveData
{
    [Serializable]
    public class PlayerOptionsSaveData
    {
        public bool IsMuted = false;
        public float MasterVolume = 0.5f;
        public float MusicVolume = 0.7f;
        public float SfxVolume = 1f;

        public PlayerOptionsSaveData()
        {
        }

        public PlayerOptionsSaveData(bool isMuted, float masterVolume, float musicVolume, float sfxVolume)
        {
            IsMuted = isMuted;
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            SfxVolume = sfxVolume;
        }
    }
}